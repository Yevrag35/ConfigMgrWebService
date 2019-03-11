using ConfigMgr.Enums;
using ConfigMgr.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace ConfigMgr.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            WebApiConfig.Helper = new ADHelper();
            WebApiConfig.Helper.LogTriggered += LogTriggered_Go;
        }

        private void LogTriggered_Go(object sender, LogTriggerEventArgs e)
        {
            if (e.Action == LogTriggerAction.Begin)
            {
                WebApiConfig.Helper.MethodBegin(e.Method, e.UserHostAddress, e.UserName);
            }
            else
            {
                WebApiConfig.Helper.MethodEnd(e.Method);
            }
        }
    }
}
