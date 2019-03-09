using System;
using System.Configuration;
using System.Web;
using System.Web.Http;

namespace ConfigMgr.WebApi
{
    public static class WebApiConfig
    {
        private const string API_KEY = "ApiKey";

        public static string GetUserHostAddress()
        {
            return HttpContext.Current.Request.UserHostAddress;
        }

        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Filters.Add(new SimpleAuth());
            config.Filters.Add(new WindowsAuth());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        internal static bool IsKeyValid(string inputKey)
        {
            string thisKey = ConfigurationManager.AppSettings[API_KEY];
            return thisKey.Equals(inputKey);
        }
    }
}
