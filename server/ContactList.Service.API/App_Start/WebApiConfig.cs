using Microsoft.OData.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using ContactList.Domain.Entities;
using ContactList.Service.API.Converters;

namespace ContactList.Service.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var builder = new ODataConventionModelBuilder();
            /*habilitar o camelcase no ODATA*/
            builder.EnableLowerCamelCase();
            builder.EntitySet<Person>("ContactList");
            builder.EntitySet<ContactValue>("ContactValueList");
            
            var model = builder.GetEdmModel();

            //setando um conversor customizado para Datas
            model.SetPayloadValueConverter(new ODataCustomFormatConverter());

            config.MapODataServiceRoute(
                routeName: "ODataRoute",
                routePrefix: "odata",
                model: model);

            // Web API enable CORS
            var cors = new EnableCorsAttribute("*", "*", "*", "*");
            cors.ExposedHeaders.Add("Content-Disposition");
            config.EnableCors(cors);

            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            config.EnableEnumPrefixFree(true);
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}