using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WEB {

    public class WebApiConfig {

        public static void Register(HttpConfiguration config) {
            
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.JsonFormatter.Indent = true;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            // Web API routes
            config.MapHttpAttributeRoutes();
 
            config.Routes.MapHttpRoute(
                "DefaultApi",
                "Api/{controller}/{id}",
                new { id = RouteParameter.Optional }
            );
            
            // Ativar Cors
            /*var enableCorsAttribute = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(enableCorsAttribute);*/
            
        }

    }

}