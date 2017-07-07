using DryIoc.WebApi;
using DryIoc;
using System.Web.Http;
using Kadena.WebAPI.Infrastructure.Filters;
using AutoMapper;
using CMS.Ecommerce;
using Kadena.Models;
using System.Linq;
using Kadena.Dto.Checkout;
using Kadena.Models.SubmitOrder;
using PaymentMethod = Kadena.Models.PaymentMethod;
using Kadena.Models.CustomerData;
using Kadena.Dto.CustomerData;
using Kadena.Dto.SubmitOrder.Requests;
using Kadena.Dto.SubmitOrder.Responses;
using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using Kadena2.MicroserviceClients.MicroserviceResponses;
using Kadena.Dto.Settings;
using System.Collections.Generic;
using Kadena.Models.Settings;
using Kadena.Dto.Search.Responses;
using Kadena.Dto.General;
using Kadena.Dto.Order;
using Kadena.Models.RecentOrders;
using Kadena.Dto.RecentOrders;
using Kadena.Models.Search;
using Kadena.Models.OrderDetail;
using Kadena.Dto.ViewOrder.Responses;
using Kadena.Models.Checkout;

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
