using DryIoc.WebApi;
using DryIoc;
using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web.Http;

namespace Kadena.WebAPI
{
    public static class WebApiConfig
    {
        /// <summary>
        /// Run api configuration.
        /// </summary>
        public static void Configure(HttpConfiguration apiConfig)
        {
            //var apiConfig = GlobalConfiguration.Configuration;
            RegisterApiRoutes(apiConfig);
            ConfigureJsonSerialization(apiConfig);
            ConfigureContainer(apiConfig);
        }

        private static void ConfigureContainer(HttpConfiguration apiConfig)
        {
            var container = new DryIoc.Container();
            container.Register<IShoppingCartService,ShoppingCartService>();
            container.WithWebApi(apiConfig);
        }

        /// <summary>
        /// Configure json serialization.
        /// </summary>
        /// <param name="config">The configuration holder object.</param>
        private static void ConfigureJsonSerialization(HttpConfiguration config)
        {
            var jsonFormatter = new System.Net.Http.Formatting.JsonMediaTypeFormatter();
            jsonFormatter.SerializerSettings.Formatting = Formatting.Indented;
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            config.Formatters.Clear();
            config.Formatters.Add(jsonFormatter);
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
