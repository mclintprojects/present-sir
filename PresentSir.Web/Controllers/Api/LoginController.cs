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
                    user = user
                });
            }
            else
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Username or password is invalid!");
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