using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.Owin.Security.OAuth;
namespace RMSExternalApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // config.SuppressDefaultHostAuthentication();
            // config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            string origins = ConfigurationManager.AppSettings["Cors.Origins"] ?? "*";
            string headers = ConfigurationManager.AppSettings["Cors.Headers"] ?? "*";
            string methods = ConfigurationManager.AppSettings["Cors.Methods"] ?? "*";

            var cors = new EnableCorsAttribute(origins, headers, methods);

            //Áp dụng chính sách CORS này cho toàn bộ Web API
            config.EnableCors(cors);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Formatters.Remove(config.Formatters.XmlFormatter);
        }
    }
}
