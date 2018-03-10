using PresentSir.Web.Models;
using PresentSir.Web.Utils;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PresentSir.Web.Controllers.Api
{
    public class RegisterController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Register([FromBody] ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                var response = UserManager.Instance.AddUser(user);

                if (string.IsNullOrEmpty(response.ErrorMessage))

                {
                    switch (user.AccountType)
                    {
                        case AccountType.Student:
                            return CreateStudent(response.User);

                        case AccountType.Teacher:
                            return CreateTeacher(response.User);

                        default:
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Something went wrong!");
                    }
                }
                else
                    return Request.CreateResponse(HttpStatusCode.BadRequest, response.ErrorMessage);
            }
            else
                return Request.CreateResponse(HttpStatusCode.BadRequest, "One or more required fields missing");
        }

        private HttpResponseMessage CreateTeacher(ApplicationUser newUser)
        {
            var newTeacher = new Teacher { User = newUser };

            ApplicationDbContext.Instance.Teachers.Insert(newTeacher);

            return Request.CreateResponse(HttpStatusCode.OK, newUser);
        }

        private HttpResponseMessage CreateStudent(ApplicationUser newUser)
        {
            if (!string.IsNullOrEmpty(newUser.IndexNumber))
            {
                var newStudent = new Student
                {
                    User = newUser
                };

                ApplicationDbContext.Instance.Students.Insert(newStudent);

                return Request.CreateResponse(HttpStatusCode.OK, newUser);
            }
            else
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Index number cannot be empty");
        }
    }
}