using PresentSir.Web.Utils;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PresentSir.Web.Controllers.Api
{
    public class RegisteredCourseController : ApiController
    {
        public HttpResponseMessage DeleteCourse(int courseId)
        {
            var deleted = ApplicationDbContext.Instance.RegisteredClasses.Delete(courseId);

            if (deleted)
                return Request.CreateResponse(HttpStatusCode.OK);
            return Request.CreateResponse(HttpStatusCode.NotFound, "A registered course with that id does not exist.");
        }
    }
}