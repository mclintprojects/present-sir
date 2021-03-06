﻿using PresentSir.Web.Utils;
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

        public HttpResponseMessage GetInstitutions(string name)
        {
            if (name == null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ApplicationDbContext.Instance.Institutions.FindAll());
            }

            return Request.CreateResponse(HttpStatusCode.OK, ApplicationDbContext.Instance.Institutions.Find(x => x.Name.ToLower().Contains(name.ToLower())));
        }
    }
}