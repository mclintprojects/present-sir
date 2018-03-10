using PresentSir.Web.Utils;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PresentSir.Web.Controllers.Api
{
    [JWTAuthorize]
    public class InstitutionsController : ApiController
    {
        public HttpResponseMessage GetInstitutions()
        {
            return Request.CreateResponse(HttpStatusCode.OK, ApplicationDbContext.Instance.Institutions.FindAll());
        }
    }
}