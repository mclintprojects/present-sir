using PresentSir.Web.Models;
using PresentSir.Web.Utils;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PresentSir.Web.Controllers.Api
{
    public class LoginController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage AuthUser(string username, string password)
        {
            var user = UserManager.Instance.FindUser(username, password);

            if (user != null)
            {
                var token = JWTManager.GetToken(user);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    token = token,
                    user = user,
                    details = GetDetails(user.Id, user.AccountType)
                });
            }
            else
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Username or password is invalid!");
        }

        private object GetDetails(int id, AccountType accountType)
        {
            if (accountType == AccountType.Student)
                return ApplicationDbContext.Instance.Students.Include(x => x.User).FindOne(x => x.User.Id == id);
            else
                return ApplicationDbContext.Instance.Teachers.Include(x => x.User).FindOne(x => x.User.Id == id);
        }

        [HttpPost]
        public HttpResponseMessage RefreshToken(string expiredToken)
        {
            var userId = JWTManager.DecodeToken(expiredToken).Item3;

            var user = ApplicationDbContext.Instance.Users.FindOne(x => x.Id == userId);
            if (user != null)
            {
                var token = JWTManager.GetToken(user);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    token = token,
                    user = user
                });
            }
            else
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid token.");
        }
    }
}