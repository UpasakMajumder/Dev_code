using DryIoc.WebApi;
using DryIoc;
using System.Web.Http;
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
            ConfigureMapper();
            ConfigureContainer(apiConfig);
            apiConfig.EnsureInitialized();
        }

        private static void ConfigureFilters(HttpConfiguration config)
        {
            GlobalConfiguration.Configuration.Filters.Add(new ExceptionFilter());
            GlobalConfiguration.Configuration.Filters.Add(new ValidateModelStateAttribute());
        }
        

        // TODO refactor mappping non-automappable 
        // TODO get rid of CMS.ECommerce dependency
        // TODO think about the best way how to initialize mappings. DONT forget testability
        private static void ConfigureMapper()
        {
            MapperBuilder.InitializeAll();
        }

        private static void ConfigureContainer(HttpConfiguration apiConfig)
        {
            new Container()
                .RegisterInfrastructure()
                .RegisterKentico()
                .RegisterBLL()
                .RegisterMicroservices()
                .RegisterFactories()
                .WithWebApi(apiConfig);
        }

        /// <summary>
        /// Register api routes.
        /// </summary>
        /// <param name="config">The configuration holder object.</param>
        private static void RegisterApiRoutes(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}",
                defaults: new { action = RouteParameter.Optional }
            );
        }
    }
}
