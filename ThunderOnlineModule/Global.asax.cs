using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;
using System.Text;

namespace ThunderOnlineModule
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Encoding latinEncoding = Encoding.GetEncoding(65001);
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SupportedEncodings.Add(latinEncoding);
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SupportedEncodings.RemoveAt(0);


            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
