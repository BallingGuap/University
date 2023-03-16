using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Xml;
using System.Xml.Linq;
using University.Models;
using static System.Net.WebRequestMethods;

namespace University
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            //HttpContext.Current.Request.Cookies.Get("someKey");

         

            string path = Server.MapPath("~/App_Data/PageInfo.xml");
            Utils.Helper = new Utils.XmlHelper(path, "MainPageAddress");
            Utils.DIfileLoader = new Utils.HttpFileLoader(Utils.Helper.Tag); 
            LoginSingelton.Type = LoginType.unregistered_login;
            Database.SetInitializer(new UniversityDbInitializer());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
