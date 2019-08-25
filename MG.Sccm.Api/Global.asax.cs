using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MG.Sccm.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            SmsConnectionManager.ConnectedSessions = new ConnectionCollection();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
