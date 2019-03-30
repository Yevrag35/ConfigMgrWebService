using ConfigMgr.Enums;
using ConfigMgr.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Configuration;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace ConfigMgr.WebApi
{
    public static class WebApiConfig
    {
        private const string API_KEY = "ApiKey";
        internal static ADHelper Helper = new ADHelper();

        public static string GetUserHostAddress()
        {
            return HttpContext.Current.Request.UserHostAddress;
        }

        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Filters.Add(new SimpleAuth());
            config.Filters.Add(new WindowsAuth());

            config.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                TypeNameHandling = TypeNameHandling.None,
                PreserveReferencesHandling = PreserveReferencesHandling.None
            };
            

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
