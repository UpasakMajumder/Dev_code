using DryIoc.WebApi;
using DryIoc;
using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web.Http;
using Kadena.WebAPI.Infrastructure.Filters;
using AutoMapper;
using CMS.Ecommerce;
using Kadena.WebAPI.Models;
using System.Linq;
using Kadena.Dto.Checkout;

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
            ConfigureMapper();
            ConfigureContainer(apiConfig);
        }

        private static void ConfigureFilters(HttpConfiguration config)
        {
            GlobalConfiguration.Configuration.Filters.Add(new ExceptionFilter());
            GlobalConfiguration.Configuration.Filters.Add(new AuthorizationFilter());
            GlobalConfiguration.Configuration.Filters.Add(new ValidateModelStateAttribute());
        }

        private static void ConfigureMapper()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<AddressInfo, DeliveryAddress>().ProjectUsing(ai => new DeliveryAddress()
                {
                    Id = ai.AddressID,
                    Checked = false,
                    City = ai.AddressCity,
                    State = ai.GetStateCode(),
                    Street = new[] { ai.AddressLine1 }.ToList(),
                    Zip = ai.AddressZip
                });

                config.CreateMap<CarrierInfo, DeliveryMethod>().ProjectUsing(ci => new DeliveryMethod()
                {
                    Id = ci.CarrierID,
                    Opened = false,
                    Title = ci.CarrierName
                });

                config.CreateMap<ShippingOptionInfo, DeliveryService>().ProjectUsing(s => new DeliveryService()
                {
                    Id = s.ShippingOptionID,
                    CarrierId = s.ShippingOptionCarrierID,
                    Title = s.ShippingOptionName
                });

                config.CreateMap<Total, TotalDTO>();
                config.CreateMap<Totals, TotalsDTO>();
                config.CreateMap<DeliveryService, DeliveryServiceDTO>();
                config.CreateMap<DeliveryMethods, DeliveryMethodsDTO>();
                config.CreateMap<DeliveryMethod, DeliveryMethodDTO>();
                config.CreateMap<DeliveryAddresses, DeliveryAddressesDTO>();
                config.CreateMap<DeliveryAddress, DeliveryAddressDTO>();
                config.CreateMap<CheckoutPage, CheckoutPageDTO>();
            });
        }

        private static void ConfigureContainer(HttpConfiguration apiConfig)
        {
            var container = new Container();
            container.Register<IShoppingCartService,ShoppingCartService>();
            container.Register<IKenticoProviderService, KenticoProviderService>();
            container.RegisterInstance(typeof(IMapper), Mapper.Instance);
            container.WithWebApi(apiConfig);
        }

        /// <summary>
        /// Configure json serialization.
        /// </summary>
        /// <param name="config">The configuration holder object.</param>
        private static void ConfigureJsonSerialization(HttpConfiguration config)
        {
            //config.Formatters.Clear();
            //config.Formatters.Add(new System.Net.Http.Formatting.JsonMediaTypeFormatter());

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
            //config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
