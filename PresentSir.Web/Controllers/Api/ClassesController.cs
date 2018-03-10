using PagedList;
using PresentSir.Web.Models;
using PresentSir.Web.Utils;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PresentSir.Web.Controllers.Api
{
    [JWTAuthorize]
    public class ClassesController : ApiController
    {
        public HttpResponseMessage GetClasses(int pageNumber, int perPage)
        {
            var classes = ApplicationDbContext.Instance.Classes.Include(x => x.Institution).FindAll().ToPagedList(pageNumber, perPage);

            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                Data = classes,
                Pagination = new Pagination
                {
                    HasNextPage = classes.HasNextPage,
                    HasPreviousPage = classes.HasPreviousPage,
                    PageNumber = classes.PageNumber
                }
            });
        }

        public HttpResponseMessage GetClasses(int pageNumber, int perPage, string name)
        {
            var classes = ApplicationDbContext.Instance.Classes.Include(x => x.Institution)
                .Find(x => x.Institution.Name.ToLower().Contains(name.ToLower())).ToPagedList(pageNumber, perPage);

            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                Data = classes,
                Pagination = new Pagination
                {
                    HasNextPage = classes.HasNextPage,
                    HasPreviousPage = classes.HasPreviousPage,
                    PageNumber = classes.PageNumber
                }
            });
        }

        public HttpResponseMessage PostClass([FromBody] Class @class)
        {
            if (ModelState.IsValid)
            {
                var existingClass = ApplicationDbContext.Instance.Classes.
                    FindOne(x => x.CourseCode.ToLower() == @class.CourseCode.ToLower() && x.InstitutionId == @class.InstitutionId);
                if (existingClass != null)
                {
                    @class.Institution = ApplicationDbContext.Instance.Institutions.FindOne(x => x.Id == @class.InstitutionId);
                    var classId = ApplicationDbContext.Instance.Classes.Insert(@class);

                    @class.Id = classId;
                    return Request.CreateResponse(HttpStatusCode.Created, @class);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "An existing class already exists!");
            }
            else
                return Request.CreateResponse(HttpStatusCode.BadRequest, "One or more required fields is empty.");
        }

        public HttpResponseMessage PutClass([FromBody] Class @class)
        {
            if (ModelState.IsValid)
            {
                @class.Institution = ApplicationDbContext.Instance.Institutions.FindOne(x => x.Id == @class.InstitutionId);
                var updated = ApplicationDbContext.Instance.Classes.Update(@class);

                if (updated)
                    return Request.CreateResponse(HttpStatusCode.OK);
                else
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "A class with that id does not exist.");
            }
            else
                return Request.CreateResponse(HttpStatusCode.BadRequest, "One or more required fields is empty.");
        }

        public HttpResponseMessage DeleteClass(int id)
        {
            var deleted = ApplicationDbContext.Instance.Classes.Delete(id);

            if (deleted)
                return Request.CreateResponse(HttpStatusCode.OK);
            else
                return Request.CreateResponse(HttpStatusCode.BadRequest, "A class with that id does not exist.");
        }

        [HttpPost]
        [Route("api/classes/register/{studentId}/{classId}")]
        public HttpResponseMessage RegisterForClass(int studentId, int classId)
        {
            var existingStudent = ApplicationDbContext.Instance.Students.Include(x => x.User).Include(x => x.RegisteredClasses).FindById(studentId);
            var existingClass = ApplicationDbContext.Instance.Classes.FindById(classId);

            if (existingStudent != null & existingClass != null)
            {
                if (!existingStudent.RegisteredClasses.Any(x => x.Id == existingClass.Id))
                    existingStudent.RegisteredClasses.Add(existingClass);

                ApplicationDbContext.Instance.Students.Update(existingStudent);

                return Request.CreateResponse(HttpStatusCode.OK, existingStudent);
            }
            else
                return Request.CreateResponse(HttpStatusCode.BadRequest, "A student or class with that id does not exist.");
        }
    }
}