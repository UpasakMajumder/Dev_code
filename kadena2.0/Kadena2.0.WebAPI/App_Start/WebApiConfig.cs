using DryIoc.WebApi;
using DryIoc;
using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web.Http;
using System.Net.Http.Headers;
using Kadena.WebAPI.Infrastructure.Filters;

namespace Kadena.WebAPI
{
    public static class WebApiConfig
    {
        /// <summary>
        /// Run api configuration.
        /// </summary>
        public static void Configure(HttpConfiguration apiConfig)
        {
            RegisterApiRoutes(apiConfig);
            ConfigureFilters(apiConfig);
            ConfigureJsonSerialization(apiConfig);
            ConfigureContainer(apiConfig);

        }

        private static void ConfigureFilters(HttpConfiguration config)
        {
            GlobalConfiguration.Configuration.Filters.Add(new ExceptionFilter());
            GlobalConfiguration.Configuration.Filters.Add(new AuthorizationFilter());
            GlobalConfiguration.Configuration.Filters.Add(new ValidateModelStateAttribute());
        }

        private static void ConfigureContainer(HttpConfiguration apiConfig)
        {
            var container = new Container();
            container.Register<IShoppingCartService,ShoppingCartService>();
            container.WithWebApi(apiConfig);
        }

        /// <summary>
        /// Configure json serialization.
        /// </summary>
        /// <param name="config">The configuration holder object.</param>
        private static void ConfigureJsonSerialization(HttpConfiguration config)
        {
            config.Formatters.Clear();
            config.Formatters.Add(new System.Net.Http.Formatting.JsonMediaTypeFormatter());

            var jsonFormatter = config.Formatters.JsonFormatter;
            jsonFormatter.UseDataContractJsonSerializer = false;

            var settings = jsonFormatter.SerializerSettings;
            settings.Formatting = Formatting.Indented;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
        }

        /// <summary>
        /// Register api routes.
        /// </summary>
        /// <param name="config">The configuration holder object.</param>
        private static void RegisterApiRoutes(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            //config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
