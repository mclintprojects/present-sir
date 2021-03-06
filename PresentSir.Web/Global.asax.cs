﻿using LiteDB;
using PresentSir.Web.Utils;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PresentSir.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var dbPath = Server.MapPath("~/App_Data/present-sir.db");
            CreateDatabase(dbPath);

            SeedDb();
        }

        private void SeedDb()
        {
        }

        private void CreateDatabase(string dbPath)
        {
            ApplicationDbContext.Instance.SetDatabase(new LiteDatabase(dbPath));
        }
    }
}