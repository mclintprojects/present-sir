using PresentSir.Web.Utils;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PresentSir.Web.Controllers.Api
{
    public class UserController : ApiController
    {
        [HttpGet]
        [Route("api/user/courses/{userId}")]
        public HttpResponseMessage GetRegisteredCourses(int userId)
        {
            var student = ApplicationDbContext.Instance.Students.Include(x => x.User).FindOne(x => x.User.Id == userId);

            if (student != null)
            {
                var registeredClasses = ApplicationDbContext.Instance.RegisteredClasses.Include(x => x.Class).Include(x => x.Class.Institution).Find(x => x.StudentId == student.Id);
                return Request.CreateResponse(HttpStatusCode.OK, registeredClasses);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, "No student with that user id exists.");
        }
    }
}