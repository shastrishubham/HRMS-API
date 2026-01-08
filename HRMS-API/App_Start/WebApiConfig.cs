using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;

namespace HRMS_API
{
    public static class WebApiConfig
    {
        public static string ConnectionString => ConfigurationManager.ConnectionStrings["MyDbConnection"].ConnectionString;

        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.EnableCors();

            config.Filters.Add(new AuthorizeAttribute()); // Secures all APIs by default

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Make enums serialize as strings
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new StringEnumConverter());

            // Remove XML formatter if you only want JSON
            config.Formatters.Remove(config.Formatters.XmlFormatter);

        }
    }
}
