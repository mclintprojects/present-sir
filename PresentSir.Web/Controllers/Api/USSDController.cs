using PresentSir.Web.Extensions;
using PresentSir.Web.Models;
using PresentSir.Web.Utils;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace PresentSir.Web.Controllers.Api
{
    public class USSDController : ApiController
    {
        [HttpPost]
        [Route("api/handle_ussd/at")]
        public HttpResponseMessage ProcessUSSDRequest([FromBody] USSDRequest request)
        {
            return ParseRequest(request.Text);
        }

        private HttpResponseMessage ParseRequest(string text)
        {
            try
            {
                var cleanText = text.Replace("#", string.Empty);
                var data = cleanText.Split(new[] { '*' }, StringSplitOptions.RemoveEmptyEntries);

                var existingSession = ApplicationDbContext.Instance.MarkingSessions.FindOne(x => x.ClassId == int.Parse(data[1]));

                if (existingSession != null)
                    return MarkAttendance(data[0], int.Parse(data[1]));
                else
                    return MakeUSSDResponse(EndSession(SessionConvos.NoSessionInProgress));
            }
            catch (Exception e)
            {
                return MakeUSSDResponse(EndSession(SessionConvos.SomethingWentWrong));
            }
        }

        private HttpResponseMessage MarkAttendance(string studentIndexNumber, int classId)
        {
            // Need to fix the find one indexing error here
            var student = ApplicationDbContext.Instance.Students.Include(x => x.User).FindAll().FirstOrDefault(x => x.User.IndexNumber == studentIndexNumber);

            if (student != null)
            {
                var registeredClass = ApplicationDbContext.Instance.RegisteredClasses.Include(x => x.Class).FindOne(x => x.Class.Id == classId);

                if (registeredClass != null)
                {
                    var existingAttendance = ApplicationDbContext.Instance.Attendance.Include(x => x.Student).Include(x => x.Student.User)
                        .FindAll().FirstOrDefault(x => x.Student.User.IndexNumber == studentIndexNumber && x.ClassId == classId && x.ClassDate.IsSameDay(DateTime.Now));

                    if (existingAttendance == null)
                    {
                        var attendance = new Attendance
                        {
                            ClassDate = DateTime.Now,
                            Student = student,
                            ClassId = registeredClass.Class.Id
                        };

                        ApplicationDbContext.Instance.Attendance.Insert(attendance);

                        return MakeUSSDResponse(EndSession($"{SessionConvos.AttendanceMarked} for {registeredClass.Class.CourseCode}."));
                    }
                    else
                        return MakeUSSDResponse(EndSession(SessionConvos.StudentAlreadyMarkedPresent));
                }
                else
                    return MakeUSSDResponse(EndSession(SessionConvos.StudentIsntRegisteredForClass));
            }
            else
                return MakeUSSDResponse(EndSession(SessionConvos.StudentIsntRegistered));
        }

        public string StartSession(string message) => $"CON {message}";

        public string EndSession(string message) => $"END {message}";

        private HttpResponseMessage MakeUSSDResponse(string message)
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(message, Encoding.UTF8, "text/plain");
            return response;
        }
    }
}