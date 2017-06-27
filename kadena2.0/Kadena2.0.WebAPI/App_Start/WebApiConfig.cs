using DryIoc.WebApi;
using DryIoc;
using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.Services;
using System.Web.Http;
using Kadena.WebAPI.Infrastructure.Filters;
using AutoMapper;
using CMS.Ecommerce;
using Kadena.WebAPI.Models;
using System.Linq;
using Kadena.Dto.Checkout;
using Kadena.WebAPI.Models.SubmitOrder;
using PaymentMethod = Kadena.WebAPI.Models.PaymentMethod;
using Kadena.WebAPI.Models.CustomerData;
using Kadena.Dto.CustomerData;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.MicroserviceClients.Clients;
using Kadena.Dto.SubmitOrder.Requests;
using Kadena.Dto.SubmitOrder.Responses;
using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using Kadena2.MicroserviceClients.MicroserviceResponses;
using Kadena.Dto.Settings;
using System.Collections.Generic;
using Kadena.WebAPI.Models.Settings;
using Kadena.Dto.Search.Responses;
using Kadena.Dto.General;
using Kadena.Dto.Order;
using Kadena.WebAPI.Models.RecentOrders;
using Kadena.Dto.RecentOrders;
using Kadena.WebAPI.Models.Search;
using Kadena.WebAPI.Models.OrderDetail;
using Kadena.Dto.ViewOrder.Responses;
using Kadena.WebAPI.Factories;

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
                    Country = ai.GetCountryTwoLetterCode(),
                    StateId = ai.AddressStateID,
                    CountryId = ai.AddressCountryID,
                    Street = new[] { ai.AddressLine1 }.ToList(),
                    Zip = ai.AddressZip
                });

                config.CreateMap<CarrierInfo, DeliveryCarrier>().ProjectUsing(ci => new DeliveryCarrier()
                {
                    Id = ci.CarrierID,
                    Opened = false,
                    Title = ci.CarrierDisplayName
                });

                config.CreateMap<ShippingOptionInfo, DeliveryOption>().ProjectUsing(s => new DeliveryOption()
                {
                    Id = s.ShippingOptionID,
                    CarrierId = s.ShippingOptionCarrierID,
                    Title = s.ShippingOptionDisplayName,
                    Service = s.ShippingOptionCarrierServiceName,
                });

                config.CreateMap<PaymentOptionInfo, PaymentMethod>().ProjectUsing(p => new PaymentMethod()
                {
                    Id = p.PaymentOptionID,
                    Checked = false,
                    Disabled = !p.PaymentOptionEnabled,
                    Icon = p.GetStringValue("IconResource", string.Empty),
                    Title = p.PaymentOptionDisplayName,
                    ClassName = p.PaymentOptionClassName,
                    IsUnpayable = p.GetBooleanValue("IsUnpayable", false)
                });

                config.CreateMap<CartItem, OrderItemDTO>().ProjectUsing(p => new OrderItemDTO(p.ProductType)
                {
                    DesignFilePath = p.DesignFilePath,
                    LineNumber = p.LineNumber,
                    MailingList = new MailingListDTO()
                    {
                        MailingListID = p.MailingListGuid
                    },
                    SKU = new SKUDTO()
                    {
                        KenticoSKUID = p.SKUID,
                        Name = p.SKUName,
                        SKUNumber = p.SKUNumber
                    },
                    TotalPrice = p.TotalPrice,
                    TotalTax = p.TotalTax,
                    UnitCount = p.Quantity,
                    UnitOfMeasure = p.UnitOfMeasure,
                    UnitPrice = p.UnitPrice
                });

                config.CreateMap<CustomerData, CustomerDataDTO>();
                config.CreateMap<CustomerAddress, CustomerAddressDTO>();
                config.CreateMap<CartItems, CartItemsDTO>();
                config.CreateMap<CartItem, CartItemDTO>()
                    .AfterMap((src, dest) => dest.Price = src.PriceText)
                    .AfterMap((src, dest) => dest.MailingList = src.MailingListName);
                config.CreateMap<PaymentMethod, PaymentMethodDTO>();
                config.CreateMap<PaymentMethods, PaymentMethodsDTO>();
                config.CreateMap<Total, TotalDTO>();
                config.CreateMap<Totals, TotalsDTO>();
                config.CreateMap<DeliveryOption, DeliveryServiceDTO>();
                config.CreateMap<DeliveryCarriers, DeliveryMethodsDTO>();
                config.CreateMap<DeliveryCarrier, DeliveryMethodDTO>();
                config.CreateMap<DeliveryAddresses, DeliveryAddressesDTO>();
                config.CreateMap<DeliveryAddress, DeliveryAddressDTO>();
                config.CreateMap<CheckoutPage, CheckoutPageDTO>();
                config.CreateMap<SubmitButton, SubmitButtonDTO>();
                config.CreateMap<SubmitRequestDto, SubmitOrderRequest>();
                config.CreateMap<SubmitOrderResult, SubmitOrderResponseDto>();
                config.CreateMap<SubmitOrderServiceResponseDto, SubmitOrderResult>();
                config.CreateMap<SubmitOrderErrorDto, SubmitOrderError>();
                config.CreateMap<BaseResponseDto<string>, SubmitOrderResult>();
                config.CreateMap<BaseErrorDto, SubmitOrderError>();				
                config.CreateMap<PaymentMethodDto, Models.SubmitOrder.PaymentMethod>();
                config.CreateMap<DeliveryAddress, AddressDto>()
                    .AfterMap((d, a) =>
                    {
                        a.Street1 = d.Street.Count > 0 ? d.Street[0] : null;
                        a.Street2 = d.Street.Count > 1 ? d.Street[1] : null;
                        a.IsRemoveButton = false;
                    });
                config.CreateMap<AddressDto, DeliveryAddress>()
                    .AfterMap((a, d) => d.Street = new List<string> { a.Street1, a.Street2 });
                config.CreateMap<DeliveryAddress, IdDto>();
                config.CreateMap<PageButton, PageButtonDto>();
                config.CreateMap<AddressList, AddressListDto>();
                config.CreateMap<DialogButton, DialogButtonDto>();
                config.CreateMap<DialogType, DialogTypeDto>();
                config.CreateMap<DialogField, DialogFieldDto>();
                config.CreateMap<AddressDialog, AddressDialogDto>();
                config.CreateMap<SettingsAddresses, SettingsAddressesDto>();
                config.CreateMap<OrderedItem, OrderedItemDTO>();
                config.CreateMap<OrderedItems, OrderedItemsDTO>();
                config.CreateMap<OrderDetail, OrderDetailDTO>();
                config.CreateMap<CommonInfo, CommonInfoDTO>();
                config.CreateMap<ShippingInfo, ShippingInfoDTO>();
                config.CreateMap<PaymentInfo,PaymentInfoDTO>();
                config.CreateMap<PricingInfo,PricingInfoDTO>();                
                config.CreateMap<Tracking,TrackingDTO>();
                config.CreateMap<PricingInfoItem,PricingInfoItemDTO>();
				config.CreateMap<SearchResultPage, SearchResultPageResponseDTO>();
                config.CreateMap<ResultItemPage, PageDTO>();
                config.CreateMap<ResultItemProduct, ProductDTO>();
                config.CreateMap<UseTemplateBtn, UseTemplateBtnDTO>();
                config.CreateMap<Stock, StockDTO>();
                config.CreateMap<AutocompleteResponse, AutocompleteResponseDTO>();
                config.CreateMap<AutocompleteProducts, AutocompleteProductsDTO>();
                config.CreateMap<AutocompletePages, AutocomletePagesDTO>();
                config.CreateMap<AutocompleteProduct, AutocompleteProductDTO>();
                config.CreateMap<AutocompletePage, AutocompletePageDTO>();
                config.CreateMap<ResultItemPage, AutocompletePage>();
                config.CreateMap<Pagination, PaginationDto>();
                config.CreateMap<OrderHead, OrderHeadDto>();
                config.CreateMap<Dto.Order.OrderItemDto, CartItem>()
                    .ProjectUsing(s => new CartItem { SKUName = s.Name, Quantity = s.Quantity });
                config.CreateMap<OrderDto, Order>();
                config.CreateMap<OrderListDto, OrderList>();
                config.CreateMap<CartItem, Dto.RecentOrders.OrderItemDto>()
                    .ProjectUsing(s => new Dto.RecentOrders.OrderItemDto { Name = s.SKUName, Quantity = s.Quantity.ToString() });
                config.CreateMap<Button, ButtonDto>();
                config.CreateMap<Order, OrderRowDto>()
                    .AfterMap((s, d) =>
                    {
                        d.OrderNumber = s.Id;
                        d.OrderDate = s.CreateDate;
                        d.OrderStatus = s.Status;
                    });
                config.CreateMap<OrderBody, OrderBodyDto>();
            });
        }

        private static void ConfigureContainer(HttpConfiguration apiConfig)
        {
            var container = new Container();
            container.RegisterInstance(typeof(IMapper), Mapper.Instance);
            container.Register<IOrderListServiceFactory, OrderListServiceFactory>();			
            
            // BLL
            container.Register<IShoppingCartService, ShoppingCartService>();
            container.Register<ISearchService, SearchService>();
            container.Register<ICustomerDataService, CustomerDataService>();
            container.Register<ITaxEstimationService, TaxEstimationServiceClient>();
            container.Register<ISettingsService, SettingsService>();
			
			// microservice clients
			container.Register<IMailingListClient, MailingListClient>();
			container.Register<IOrderSubmitClient, OrderSubmitClient>();
			container.Register<IOrderViewClient, OrderViewClient>();
            container.Register<IMailingListClient, MailingListClient>();
            container.Register<ITemplatedProductService, TemplatedProductService>();			

            // Kentico
            container.Register<IKenticoProviderService, KenticoProviderService>();
            container.Register<IKenticoResourceService, KenticoResourceService>();
            container.Register<IKenticoSearchService, KenticoSearchService>();
            container.Register<IKenticoLogger, KenticoLogger>();
            
            container.WithWebApi(apiConfig);
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
