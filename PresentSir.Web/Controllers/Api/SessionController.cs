using PresentSir.Web.Models;
using PresentSir.Web.Utils;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PresentSir.Web.Controllers.Api
{
    public class SessionController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage ControlSession(string action, int classId)
        {
            if (action == "start")
            {
                ApplicationDbContext.Instance.MarkingSessions.Insert(new MarkingSession
                {
                    ClassId = classId
                });

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else if (action == "finish")
            {
                var deleted = ApplicationDbContext.Instance.MarkingSessions.Delete(classId);

                if (deleted)
                    return Request.CreateResponse(HttpStatusCode.OK);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        public HttpResponseMessage GetSession(int classId)
        {
            if (ApplicationDbContext.Instance.MarkingSessions.Exists(x => x.ClassId == classId))
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }

            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        public HttpResponseMessage GetAttendance(int classId)
        {
            var count = ApplicationDbContext.Instance.Attendance.Count(x => x.ClassId == classId && x.ClassDate.ToShortDateString() == DateTime.Now.ToShortDateString());

            return Request.CreateResponse(HttpStatusCode.OK, count);
        }
    }
}