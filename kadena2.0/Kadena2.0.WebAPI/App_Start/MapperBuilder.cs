using AutoMapper;
using Kadena.Dto.Brands;
using Kadena.Dto.BusinessUnits;
using Kadena.Dto.Checkout;
using Kadena.Dto.CustomerData;
using Kadena.Dto.General;
using Kadena.Dto.Logon.Requests;
using Kadena.Dto.Logon.Responses;
using Kadena.Dto.MailingList;
using Kadena.Dto.MailingList.MicroserviceResponses;
using Kadena.Dto.MailTemplate.Responses;
using Kadena.Dto.Order;
using Kadena.Dto.Product;
using Kadena.Dto.Product.Responses;
using Kadena.Dto.RecentOrders;
using Kadena.Dto.Search.Responses;
using Kadena.Dto.Settings;
using Kadena.Dto.Site.Responses;
using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using Kadena.Dto.SubmitOrder.Requests;
using Kadena.Dto.SubmitOrder.Responses;
using Kadena.Dto.TemplatedProduct.Responses;
using Kadena.Dto.ViewOrder.Responses;
using Kadena.Models;
using Kadena.Models.Brand;
using Kadena.Models.BusinessUnit;
using Kadena.Models.Checkout;
using Kadena.Models.CustomerData;
using Kadena.Models.Login;
using Kadena.Models.OrderDetail;
using Kadena.Models.Product;
using Kadena.Models.RecentOrders;
using Kadena.Models.Search;
using Kadena.Models.Settings;
using Kadena.Models.Site;
using Kadena.Models.SubmitOrder;
using Kadena.Models.TemplatedProduct;
using Kadena2.MicroserviceClients.MicroserviceResponses;
using Kadena2.WebAPI.KenticoProviders;

namespace Kadena.WebAPI
{
    public static class MapperBuilder
    {
        public static void InitializeAll() // todo consider separating aka builder after discussion 
        {
            Mapper.Initialize(config =>
            {
                config.AddProfile<KenticoModelMappingsProfile>();

                config.CreateMap<CartItem, OrderItemDTO>().ProjectUsing(p => new OrderItemDTO()
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
                    UnitPrice = p.UnitPrice,
                    ChiliTaskId = p.DesignFilePathTaskId,
                    ChiliTemplateId = p.ChiliEditorTemplateId
                });

                config.CreateMap<CustomerData, CustomerDataDTO>();
                config.CreateMap<CustomerAddress, CustomerAddressDTO>();
                config.CreateMap<CartItems, CartItemsDTO>();
                config.CreateMap<CartItem, CartItemDTO>()
                    .AfterMap((src, dest) => dest.Price = src.PriceText)
                    .AfterMap((src, dest) => dest.MailingList = src.MailingListName);
                config.CreateMap<CartItem, CartItemPreviewDTO>()
                    .AfterMap((src, dest) => dest.Price = src.PriceText)
                    .AfterMap((src, dest) => dest.MailingList = src.MailingListName);
                config.CreateMap<Models.PaymentMethod, PaymentMethodDTO>();
                config.CreateMap<PaymentMethods, PaymentMethodsDTO>();
                config.CreateMap<Total, TotalDTO>();
                config.CreateMap<Totals, TotalsDTO>();
                config.CreateMap<DeliveryOption, DeliveryServiceDTO>();
                config.CreateMap<DeliveryCarriers, DeliveryMethodsDTO>();
                config.CreateMap<DeliveryCarrier, DeliveryMethodDTO>();
                config.CreateMap<DeliveryAddresses, DeliveryAddressesDTO>();

                config.CreateMap<DeliveryAddress, DeliveryAddressDTO>()
                    .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country.Id))
                    .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State.Id));

                config.CreateMap<DeliveryAddressDTO, DeliveryAddress>()
                    .ForMember(dest => dest.Country, opt =>
                    {
                        opt.MapFrom(src => src);
                        opt.PreCondition(src => !string.IsNullOrWhiteSpace(src.Country));
                    })
                    .ForMember(dest => dest.State, opt =>
                    {
                        opt.MapFrom(src => src);
                        opt.PreCondition(src => !string.IsNullOrWhiteSpace(src.State));
                    });

                config.CreateMap<DeliveryAddressDTO, Country>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Country));

                config.CreateMap<DeliveryAddressDTO, State>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.State));

                config.CreateMap<CheckoutPage, CheckoutPageDTO>();
                config.CreateMap<NotificationEmail, NotificationEmailDto>();
                config.CreateMap<NotificationEmailTooltip, NotificationEmailTooltipDto>();
                config.CreateMap<CheckoutPageDeliveryTotals, CheckoutPageDeliveryTotalsDTO>();
                config.CreateMap<SubmitButton, SubmitButtonDTO>();
                config.CreateMap<SubmitRequestDto, SubmitOrderRequest>();
                config.CreateMap<SubmitOrderResult, SubmitOrderResponseDto>();
                config.CreateMap<SubmitOrderServiceResponseDto, SubmitOrderResult>();
                config.CreateMap<SubmitOrderErrorDto, SubmitOrderError>();
                config.CreateMap<BaseResponseDto<string>, SubmitOrderResult>();
                config.CreateMap<BaseErrorDto, SubmitOrderError>();
                config.CreateMap<PaymentMethodDto, Models.SubmitOrder.PaymentMethod>();
                config.CreateMap<DeliveryAddress, Dto.Settings.AddressDto>()
                    .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State.Id))
                    .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country.Id));
                config.CreateMap<Dto.Settings.AddressDto, Country>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Country));
                config.CreateMap<Dto.Settings.AddressDto, State>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.State));
                config.CreateMap<Dto.Settings.AddressDto, DeliveryAddress>()
                    .ForMember(dest => dest.State, opt =>
                    {
                        opt.MapFrom(src => src);
                        opt.PreCondition(src => !string.IsNullOrWhiteSpace(src.State));
                    })
                    .ForMember(dest => dest.Country, opt =>
                    {
                        opt.MapFrom(src => src);
                        opt.PreCondition(src => !string.IsNullOrWhiteSpace(src.Country));
                    });
                config.CreateMap<DeliveryAddress, IdDto>();
                config.CreateMap<PageButton, PageButtonDto>();
                config.CreateMap<AddressList, AddressListDto>();
                config.CreateMap<DialogButton, DialogButtonDto>();
                config.CreateMap<DialogType, DialogTypeDto>();
                config.CreateMap<DialogField, DialogFieldDto>();
                config.CreateMap<Models.Settings.AddressDialog, Dto.Settings.AddressDialogDto>();
                config.CreateMap<Models.Checkout.AddressDialog, Dto.Checkout.AddressDialogDto>();
                config.CreateMap<DefaultAddress, DefaultAddressDto>();
                config.CreateMap<SettingsAddresses, SettingsAddressesDto>();
                config.CreateMap<OrderedItem, OrderedItemDTO>();
                config.CreateMap<OrderedItems, OrderedItemsDTO>();
                config.CreateMap<OrderDetail, OrderDetailDTO>();
                config.CreateMap<CommonInfo, CommonInfoDTO>();
                config.CreateMap(typeof(TitleValuePair<>), typeof(TitleValuePairDto<>));
                config.CreateMap<ShippingInfo, ShippingInfoDTO>();
                config.CreateMap<PaymentInfo, PaymentInfoDTO>();
                config.CreateMap<PricingInfo, PricingInfoDTO>();
                config.CreateMap<Tracking, TrackingDTO>();
                config.CreateMap<PricingInfoItem, PricingInfoItemDTO>();
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
                config.CreateMap<Campaign, CampaignDTO>().ProjectUsing(s => new CampaignDTO { ID = s.ID, ProgramID = s.ProgramID, DistributorID = s.DistributorID });
                //config.CreateMap<CampaignDTO, Campaign>().ProjectUsing(s => new Campaign { ID = s.ID, ProgramID = s.ProgramID, DistributorID = s.DistributorID });
                config.CreateMap<Order, OrderRowDto>()
                    .AfterMap((s, d) =>
                    {
                        d.OrderNumber = s.Id;
                        d.OrderDate = s.CreateDate;
                        d.OrderStatus = s.Status;
                    });
                config.CreateMap<OrderBody, OrderBodyDto>();
                config.CreateMap<NewAddressButton, NewAddressButtonDTO>();
                config.CreateMap<DeliveryAddressesBounds, DeliveryAddressesBoundsDTO>();
                config.CreateMap<CheckoutPageDeliveryTotals, CheckoutPageDeliveryTotalsDTO>();
                config.CreateMap<UpdateAddressDto, MailingAddress>().ProjectUsing(a => new MailingAddress
                {
                    Id = a.Id,
                    Name = a.FullName,
                    Address1 = a.FirstAddressLine,
                    Address2 = a.SecondAddressLine,
                    City = a.City,
                    State = a.State,
                    Zip = a.PostalCode
                });
                config.CreateMap<MailingAddress, MailingAddressDto>().AfterMap((s, d) => d.FirstName = s.Name);
                config.CreateMap<CartItemsPreview, CartItemsPreviewDTO>();
                config.CreateMap<CartPrice, CartPriceDTO>();
                config.CreateMap<MailingListDataDTO, MailingList>();
                config.CreateMap<NewCartItemDto, NewCartItem>();
                config.CreateMap<AddToCartResult, AddToCartResultDto>();
                config.CreateMap<RequestResult, RequestResultDto>();
                config.CreateMap<ArtworkFtpSettings, ArtworkFtpResponseDto>();
                config.CreateMap<FtpCredentials, FtpCredentialsDto>();
                config.CreateMap<CartEmptyInfo, CartEmptyInfoDTO>();
                config.CreateMap<MailTemplate, MailTemplateDto>();
                config.CreateMap<KenticoSite, SiteDataResponseDto>();
                config.CreateMap<ProductsPage, GetProductsDto>();
                config.CreateMap<ProductCategoryLink, ProductCategoryDto>();
                config.CreateMap<ProductLink, ProductDto>();
                config.CreateMap<ProductTemplates, ProductTemplatesDTO>();
                config.CreateMap<ProductTemplate, ProductTemplateDTO>();
                config.CreateMap<ProductTemplatesHeader, ProductTemplatesHeaderDTO>()
                    .ForMember(dest => dest.Sorting, cfg => cfg.ResolveUsing(src => src.Sorting.ToString().ToLower()));
                config.CreateMap<LocalizationDto, string>().ProjectUsing(src => src.Language);
                config.CreateMap<ButtonLabels, ButtonLabelsDto>();
                config.CreateMap<AddressDTO, DeliveryAddress>()
                    .ForMember(dest => dest.Address1, opt => opt.MapFrom(src => src.AddressLine1))
                    .ForMember(dest => dest.Address2, opt => opt.MapFrom(src => src.AddressLine2))
                    .ForMember(dest => dest.State, opt => opt.Ignore())
                    .ForMember(dest => dest.Country, opt => opt.Ignore());
                config.CreateMap<DeliveryAddress, Dto.ViewOrder.Responses.AddressDto>()
                    .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State.StateCode))
                    .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country.Name));
                config.CreateMap<LogonUserRequestDTO, LoginRequest>();
                config.CreateMap<LoginResult, LogonUserResultDTO>();
                config.CreateMap<CheckTaCRequestDTO, LoginRequest>();
                config.CreateMap<AcceptTaCRequestDTO, LoginRequest>();
                config.CreateMap<CheckTaCResult, CheckTaCResultDTO>();
                config.CreateMap<BusinessUnit, BusinessUnitDto>();
                config.CreateMap<Brand, BrandDto>();
                config.CreateMap<OrderCampaginHead, OrderCampaginHeadDto>();
                config.CreateMap<OrderCampaginItem, OrderCampaginItemDto>();

                config.CreateMap<OrderHeadBlock, OrderHeadBlockDto>();
                config.CreateMap<OrderRow, RecentOrderRowDto>();
                config.CreateMap<OrderDialog, OrderDialogDto>();
                config.CreateMap<OrderDailogLabel, OrderDailogLabelDto>();
                config.CreateMap<OrderDialogTable, OrderDialogTableDto>();
                config.CreateMap<OrderTableCell, OrderTableCellDto>();
                config.CreateMap<OrderDialogTableCell, OrderDialogTableCellDto>();
                config.CreateMap<Distributor, DistributorDTO>();
            });
        }
    }
}