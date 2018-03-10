using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace PresentSir.Web.Utils
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class JWTAuthorize : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext httpContext)
        {
            try
            {
                var jwt = httpContext.Request.Headers.Authorization?.Parameter;
                if (jwt == null)
                    return false;
                else
                    return JWTManager.DecodeToken(jwt).Item1;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Forbidden,
                Content = new StringContent($"Authorization token is invalid.")
            };
        }
    }
}