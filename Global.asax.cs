using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebAPIPrueba.Models;
using WebAPIPrueba.Migrations;
using WebAPIPrueba.Models;

namespace WebAPIPrueba
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<WebApiPruebaContext, Configuration>());
            CheckRolesAndSuperUsers();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private void CheckRolesAndSuperUsers()
        {
            UsersHelper.CheckRole("Admin");
            UsersHelper.CheckRole("User");
            UsersHelper.CheckRole("Boss");
            UsersHelper.CheckRole("Link");
            UsersHelper.CheckRole("Coordinator");
            UsersHelper.CheckRole("Leader");
            UsersHelper.CheckRole("Reunion");
            UsersHelper.CheckRole("Digitador");
            UsersHelper.CheckRole("Secretario");
            UsersHelper.CheckSuperUser();
        }
    }
}
