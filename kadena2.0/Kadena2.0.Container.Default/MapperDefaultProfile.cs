using AutoMapper;
using Kadena.Container.Default.Converters;
using Kadena.Dto.AddToCart;
using Kadena.Dto.Approval.Responses;
using Kadena.Dto.Brands;
using Kadena.Dto.BusinessUnits;
using Kadena.Dto.Checkout;
using Kadena.Dto.Checkout.Responses;
using Kadena.Dto.Common;
using Kadena.Dto.CreditCard._3DSi.Requests;
using Kadena.Dto.CreditCard.Requests;
using Kadena.Dto.CreditCard.Responses;
using Kadena.Dto.CustomerData;
using Kadena.Dto.EstimateDeliveryPrice.MicroserviceRequests;
using Kadena.Dto.Logon.Requests;
using Kadena.Dto.Logon.Responses;
using Kadena.Dto.MailingList;
using Kadena.Dto.MailingList.MicroserviceResponses;
using Kadena.Dto.MailTemplate.Responses;
using Kadena.Dto.Order;
using Kadena.Dto.Order.Failed;
using Kadena.Dto.OrderManualUpdate.MicroserviceRequests;
using Kadena.Dto.OrderManualUpdate.Requests;
using Kadena.Dto.OrderManualUpdate.Responses;
using Kadena.Dto.OrderReport;
using Kadena.Dto.Product;
using Kadena.Dto.Product.Responses;
using Kadena.Dto.RecentOrders;
using Kadena.Dto.Search.Responses;
using Kadena.Dto.Settings;
using Kadena.Dto.Shipping;
using Kadena.Dto.Site.Responses;
using Kadena.Dto.SSO;
using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using Kadena.Dto.SubmitOrder.Requests;
using Kadena.Dto.SubmitOrder.Responses;
using Kadena.Dto.TemplatedProduct.Requests;
using Kadena.Dto.TemplatedProduct.Responses;
using Kadena.Dto.ViewOrder.Responses;
using Kadena.Models;
using Kadena.Models.AddToCart;
using Kadena.Models.Approval;
using Kadena.Models.Brand;
using Kadena.Models.BusinessUnit;
using Kadena.Models.Checkout;
using Kadena.Models.Common;
using Kadena.Models.CreditCard;
using Kadena.Models.CustomerData;
using Kadena.Models.Login;
using Kadena.Models.Membership;
using Kadena.Models.OrderDetail;
using Kadena.Models.Orders;
using Kadena.Models.Orders.Failed;
using Kadena.Models.Product;
using Kadena.Models.RecentOrders;
using Kadena.Models.Search;
using Kadena.Models.Settings;
using Kadena.Models.Shipping;
using Kadena.Models.ShoppingCarts;
using Kadena.Models.Site;
using Kadena.Models.SubmitOrder;
using Kadena.Models.TemplatedProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using Kadena.Dto.Routing;
using Kadena.Dto.Routing.MicroserviceRequests;
using Kadena.Models.Routing;
using Kadena.Models.Routing.Request;
using Kadena.Models.ErpSystem;
using Kadena.Dto.ErpSystem;
using Kadena.Models.CampaignData;

namespace Kadena.Container.Default
{
    public class MapperDefaultProfile : Profile
    {
        public MapperDefaultProfile()
        {
            CreateMap<RoutingDto, Routing>();
            CreateMap<DeleteRouting, DeleteRoutingRequestDto>();
            CreateMap<SetRouting, SetRoutingRequestDto>();
            CreateMap<ShoppingCart, OrderManualUpdateRequestDto>()
                .ForMember(dest => dest.TotalShipping, opt => opt.MapFrom(src => src.ShippingPrice))
                .ForMember(dest => dest.OrderId, opt => opt.Ignore());
            CreateMap<CartItemEntity, ItemUpdateDto>()
               .ForMember(dest => dest.LineNumber, opt => opt.ResolveUsing((src, dest, val, ctx) =>
               {
                   if (ctx.Items.TryGetValue(src.SKUID.ToString(), out object value))
                   {
                       return value;
                   }
                   return 0;
               }
               ));

            CreateMap<OrderReportViewItem, TableRow>()
                .ForMember(dest => dest.Items, opt => opt.ResolveUsing(src => new object[] {
                    src.Site,
                    src.Number,
                    src.OrderingDate,
                    src.User,
                    src.Name,
                    src.SKU ?? string.Empty,
                    src.Quantity,
                    src.Price,
                    src.Status,
                    src.ShippingDate,
                    src.TrackingInfos
                }));
            CreateMap<ErpSystem, ErpSystemDto>();
            CreateMap<ErpSystem[], ErpSystemDto[]>();
            CreateMap<RecentOrderDto, OrderReportViewItem>()
                .ForMember(dest => dest.Site, opt => opt.MapFrom(src => src.SiteName))
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Url, opt => opt.Ignore())
                .ForMember(dest => dest.OrderingDate, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.SKU, opt => opt.Ignore())
                .ForMember(dest => dest.Quantity, opt => opt.Ignore())
                .ForMember(dest => dest.Price, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.ShippingDate, opt => opt.Ignore())
                .ForMember(dest => dest.TrackingInfos, opt => opt.Ignore());
            CreateMap<Dto.Order.OrderItemDto, OrderReportViewItem>()
                .ForMember(dest => dest.Site, opt => opt.Ignore())
                .ForMember(dest => dest.Number, opt => opt.Ignore())
                .ForMember(dest => dest.Url, opt => opt.Ignore())
                .ForMember(dest => dest.OrderingDate, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.SKU, opt => opt.MapFrom(src => src.SKUNumber))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.UnitPrice))
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.ShippingDate, opt => opt.Ignore());

            CreateMap<Dto.ViewOrder.MicroserviceResponses.TrackingInfoDto, TrackingInfo>();
            CreateMap<TrackingInfo, TrackingInfoDto>();

            CreateMap<OrderedItem, OrderedItem>();
            CreateMap<Dto.ViewOrder.MicroserviceResponses.OrderItemDTO, OrderedItem>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.SkuId))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.Image, opt => opt.Ignore())
                .ForMember(dest => dest.DownloadPdfURL, opt => opt.Ignore())
                .ForMember(dest => dest.TemplatePrefix, opt => opt.Ignore())
                .ForMember(dest => dest.Template, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.MailingListPrefix, opt => opt.Ignore())
                .ForMember(dest => dest.MailingList, opt => opt.MapFrom(src => src.MailingList == Guid.Empty.ToString() ? string.Empty : src.MailingList))
                .ForMember(dest => dest.ShippingDatePrefix, opt => opt.Ignore())
                .ForMember(dest => dest.ShippingDate, opt => opt.UseValue(string.Empty))
                .ForMember(dest => dest.TrackingPrefix, opt => opt.Ignore())
                .ForMember(dest => dest.Tracking, opt => opt.MapFrom(src => src.TrackingInfoList))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => string.Format("$ {0:#,0.00}", src.TotalPrice)))
                .ForMember(dest => dest.QuantityPrefix, opt => opt.Ignore())
                .ForMember(dest => dest.QuantityShippedPrefix, opt => opt.Ignore())
                .ForMember(dest => dest.ProductStatusPrefix, opt => opt.Ignore())
                .ForMember(dest => dest.ProductStatus, opt => opt.Ignore())
                .ForMember(dest => dest.Preview, opt => opt.Ignore())
                .ForMember(dest => dest.EmailProof, opt => opt.Ignore())
                .ForMember(dest => dest.Removed, opt => opt.Ignore())
                .ForMember(dest => dest.RemoveLabel, opt => opt.Ignore())
                .ForMember(dest => dest.Options, opt => opt.UseValue(Enumerable.Empty<ItemOption>()));

            CreateMap<ApprovalResult, ApprovalResultDto>();

            CreateMap<RegistrationDto, Registration>();

            CreateMap<ChiliProcess, ChiliProcessDto>();
            CreateMap<ItemOption, ItemOptionDto>();

            CreateMap<Price, PriceDto>()
                .ForMember(dest => dest.PricePrefix, opt => opt.MapFrom(src => src.Prefix))
                .ForMember(dest => dest.PriceValue, opt => opt.MapFrom(src => src.Value));

            CreateMap<OrderItemSku, SKUDTO>();

            CreateMap<Models.SubmitOrder.MailingList, MailingListDTO>();

            CreateMap<OrderCartItem, OrderItemDTO>()
                .ForMember(dest => dest.UnitCount, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Attributes, opt => opt.MapFrom(src => src.Options.ToDictionary(i => i.Name, i => i.Value)))
                .ForMember(dest => dest.DesignFileKey, opt => opt.MapFrom(src => src.Artwork))
                .ForMember(dest => dest.UnitOfMeasure, opt => opt.MapFrom(src => src.UnitOfMeasureErpCode))
                .ForMember(dest => dest.Type, opt => opt.Ignore());

            CreateMap<CustomerData, CustomerDataDTO>();
            CreateMap<Approver, ApproverDto>();
            CreateMap<CustomerAddress, CustomerAddressDTO>();
            CreateMap<CartItems, CartItemsDTO>();
            CreateMap<CheckoutCartItem, CartItemDTO>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.PriceText))
                .ForMember(dest => dest.MailingList, opt => opt.MapFrom(src => src.MailingListName))
                .ForMember(dest => dest.UnitOfMeasure, opt => opt.MapFrom(src => src.UnitOfMeasureName));
            CreateMap<CheckoutCartItem, CartItemPreviewDTO>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.PriceText))
                .ForMember(dest => dest.MailingList, opt => opt.MapFrom(src => src.MailingListName))
                .ForMember(dest => dest.UnitOfMeasure, opt => opt.MapFrom(src => src.UnitOfMeasureName));
            CreateMap<Models.PaymentMethod, PaymentMethodDTO>();
            CreateMap<PaymentMethods, PaymentMethodsDTO>();
            CreateMap<Total, TotalDTO>();
            CreateMap<Totals, TotalsDTO>();
            CreateMap<DeliveryOption, DeliveryServiceDTO>();
            CreateMap<DeliveryOption, DeliveryOptionDto>()
                .ForMember(dest => dest.ShippingOptionId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CarrierCode, opt => opt.MapFrom(src => src.SAPName))
                .ForMember(dest => dest.Provider, opt => opt.MapFrom(src => src.CarrierCode))
                .ForMember(dest => dest.ShippingService, opt => opt.MapFrom(src => src.Service.Replace("#", "")))
                .ForMember(dest => dest.ShippingServiceDisplayName, opt => opt.MapFrom(src => src.Title));
            CreateMap<DeliveryCarriers, DeliveryMethodsDTO>();
            CreateMap<DeliveryCarrier, DeliveryMethodDTO>();
            CreateMap<DeliveryAddresses, DeliveryAddressesDTO>();

            CreateMap<DeliveryAddress, DeliveryAddressDTO>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.AddressPersonalName))
                .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.CompanyName))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country.Id))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State.Id));

            CreateMap<CheckoutPage, CheckoutPageDTO>();
            CreateMap<NotificationEmail, NotificationEmailDto>();
            CreateMap<NotificationEmailTooltip, NotificationEmailTooltipDto>();
            CreateMap<CheckoutPageDeliveryTotals, CheckoutPageDeliveryTotalsDTO>();
            CreateMap<DeliveryDate, DeliveryDateDTO>();
            CreateMap<DeliveryDateMessages, DeliveryDateMessagesDTO>();
            CreateMap<SubmitButton, SubmitButtonDTO>();
            CreateMap<SubmitRequestDto, SubmitOrderRequest>();
            CreateMap<string, DateTime?>().ConvertUsing<StringToNullableDateTimeConverter>();
            CreateMap<SubmitOrderResult, SubmitOrderResponseDto>();
            CreateMap<PaymentMethodDto, Models.SubmitOrder.PaymentMethod>();
            CreateMap<DeliveryAddress, Dto.Settings.AddressDto>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.AddressPersonalName))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State.Id))
                .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.CompanyName))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country.Id));
            CreateMap<Dto.Settings.AddressDto, Country>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Country))
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.Code, opt => opt.Ignore());
            CreateMap<Dto.Settings.AddressDto, State>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.State))
                .ForMember(dest => dest.StateDisplayName, opt => opt.Ignore())
                .ForMember(dest => dest.StateName, opt => opt.Ignore())
                .ForMember(dest => dest.StateCode, opt => opt.Ignore())
                .ForMember(dest => dest.CountryId, opt => opt.Ignore());
            CreateMap<Dto.Settings.AddressDto, DeliveryAddress>()
                .ForMember(dest => dest.AddressPersonalName, opt => opt.MapFrom(src => src.CustomerName))
                .ForMember(dest => dest.Checked, opt => opt.Ignore())
                .ForMember(dest => dest.CustomerId, opt => opt.Ignore())
                .ForMember(dest => dest.AddressName, opt => opt.Ignore())
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company))
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
            CreateMap<DeliveryAddress, IdDto>();
            CreateMap<PageButton, PageButtonDto>();
            CreateMap<AddressList, AddressListDto>();
            CreateMap<DialogButton, DialogButtonDto>();
            CreateMap<DialogType, DialogTypeDto>();
            CreateMap<DialogField, DialogFieldDto>();
            CreateMap<Models.Settings.AddressDialog, Dto.Settings.AddressDialogDto>();
            CreateMap<Models.Checkout.AddressDialog, Dto.Checkout.AddressDialogDto>();
            CreateMap<DefaultAddress, DefaultAddressDto>();
            CreateMap<SettingsAddresses, SettingsAddressesDto>();
            CreateMap<OrderedItem, OrderedItemDTO>();
            CreateMap<OrderedItems, OrderedItemsDTO>();
            CreateMap<OrderedItemsSection, OrderedItemsSectionDTO>();
            CreateMap<OrderedItemsGroup, OrderedItemsGroupDTO>();
            CreateMap<OrderedItemsGroupTracking, OrderedItemsGroupTrackingDTO>();
            CreateMap<OrderedItemsGroupShippingDate, OrderedItemsGroupShippingDateDTO>();
            CreateMap<OrderDetail, OrderDetailDTO>();
            CreateMap<CommonInfo, CommonInfoDTO>();
            CreateMap<OrderStatusInfo, OrderStatusInfoDTO>();
            CreateMap<Link, LinkDto>();
            CreateMap<OrderInfo, OrderInfoDTO>();
            CreateMap<OrderActions, OrderActionsDTO>();
            CreateMap<DialogButton<Dialog>, DialogButtonDTO<DialogDTO>>();
            CreateMap<DialogButton<EditOrderDialog>, DialogButtonDTO<EditOrderDialogDTO>>();
            CreateMap<EditOrderDialog, EditOrderDialogDTO>();
            CreateMap<EditOrderDialogButtons, EditOrderDialogButtonsDTO>();
            CreateMap<Dialog, DialogDTO>();
            CreateMap(typeof(TitleValuePair<>), typeof(TitleValuePairDto<>));
            CreateMap<ShippingInfo, ShippingInfoDTO>();
            CreateMap<PaymentInfo, PaymentInfoDTO>();
            CreateMap<PricingInfo, PricingInfoDTO>();
            CreateMap<PricingInfoItem, PricingInfoItemDTO>();
            CreateMap<SearchResultPage, SearchResultPageResponseDTO>();
            CreateMap<ResultItemPage, PageDTO>();
            CreateMap<ResultItemProduct, ProductDTO>();
            CreateMap<UseTemplateBtn, UseTemplateBtnDTO>();
            CreateMap<Stock, StockDTO>();
            CreateMap<AutocompleteResponse, AutocompleteResponseDTO>();
            CreateMap<AutocompleteProducts, AutocompleteProductsDTO>();
            CreateMap<AutocompletePages, AutocomletePagesDTO>();
            CreateMap<AutocompleteProduct, AutocompleteProductDTO>();
            CreateMap<AutocompletePage, AutocompletePageDTO>();
            CreateMap<ResultItemPage, AutocompletePage>();
            CreateMap<Pagination, PaginationDto>();
            CreateMap<OrderHead, OrderHeadDto>();
            CreateMap<Dto.Order.OrderItemDto, CheckoutCartItem>()
                .ProjectUsing(s => new CheckoutCartItem { SKUName = s.Name, Quantity = s.Quantity });
            CreateMap<RecentOrderDto, Order>()
                .ForMember(dest => dest.ViewBtn, opt => opt.Ignore())
                .ForMember(dest => dest.ClientName, opt => opt.Ignore());
            CreateMap<OrderListDto, OrderList>();
            CreateMap<CheckoutCartItem, Dto.RecentOrders.OrderItemDto>()
                .ProjectUsing(s => new Dto.RecentOrders.OrderItemDto { Name = s.SKUName, Quantity = s.Quantity.ToString() });
            CreateMap<Button, ButtonDto>();
            CreateMap<Campaign, CampaignDTO>();
            CreateMap<CampaignDTO, Campaign>();
            CreateMap<Order, OrderRowDto>()
                .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.CreateDate))
                .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.DeliveryDate, opt => opt.MapFrom(src => src.ShippingDate.GetValueOrDefault()))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.ClientName));
            CreateMap<OrderBody, OrderBodyDto>();

            CreateMap<TableView, TableViewDto>();
            CreateMap<TableRow, TableRowDto>()
                .AfterMap((src, dest, ctx) => dest.Items[10] = ctx.Mapper.Map<TrackingFieldDto>(dest.Items[10]));
            CreateMap<Pagination, PaginationDto>();
            CreateMap<IEnumerable<TrackingInfo>, TrackingFieldDto>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src));
            CreateMap<TitledMessage, TitledMessageDto>();

            CreateMap<NewAddressButton, NewAddressButtonDTO>();
            CreateMap<DeliveryAddressesBounds, DeliveryAddressesBoundsDTO>();
            CreateMap<CheckoutPageDeliveryTotals, CheckoutPageDeliveryTotalsDTO>();
            CreateMap<UpdateAddressDto, MailingAddress>().ProjectUsing(a => new MailingAddress
            {
                Id = a.Id,
                Name = a.FullName,
                Address1 = a.FirstAddressLine,
                Address2 = a.SecondAddressLine,
                City = a.City,
                State = a.State,
                Zip = a.PostalCode
            });
            CreateMap<MailingAddress, MailingAddressDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ContainerId, opt => opt.Ignore())
                .ForMember(dest => dest.Title, opt => opt.Ignore())
                .ForMember(dest => dest.LastName, opt => opt.Ignore())
                .ForMember(dest => dest.ErrorMessage, opt => opt.Ignore());
            CreateMap<CartItemsPreview, CartItemsPreviewDTO>();
            CreateMap<CartPrice, CartPriceDTO>();
            CreateMap<MailingListDataDTO, Models.MailingList>();
            CreateMap<NewCartItemDto, NewCartItem>();
            CreateMap<AddToCartResult, AddToCartResultDto>();
            CreateMap<RequestResult, RequestResultDto>();
            CreateMap<ArtworkFtpSettings, ArtworkFtpResponseDto>();
            CreateMap<FtpCredentials, FtpCredentialsDto>();
            CreateMap<CartEmptyInfo, CartEmptyInfoDTO>();
            CreateMap<MailTemplate, MailTemplateDto>();
            CreateMap<KenticoSite, KenticoSiteWithDeliveryOptions>()
                .ForMember(dest => dest.DeliveryOptions, opt => opt.Ignore());
            CreateMap<KenticoSiteWithDeliveryOptions, SiteDataResponseDto>();
            CreateMap<ProductsPage, GetProductsDto>();
            CreateMap<ProductCategoryLink, ProductCategoryDto>();
            CreateMap<ProductLink, ProductDto>();
            CreateMap<Border, BorderDto>();
            CreateMap<ProductTemplates, ProductTemplatesDTO>();
            CreateMap<ProductTemplate, ProductTemplateDTO>();
            CreateMap<ProductTemplatesHeader, ProductTemplatesHeaderDTO>()
                .ForMember(dest => dest.Sorting, cfg => cfg.ResolveUsing(src => src.Sorting.ToString().ToLower()));
            CreateMap<LocalizationDto, string>().ProjectUsing(src => src.Language);
            CreateMap<ButtonLabels, ButtonLabelsDto>();
            CreateMap<AddressDTO, State>()
                .ForMember(dest => dest.StateCode, opt => opt.MapFrom(src => src.State))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.KenticoStateID.GetValueOrDefault()))
                .ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => src.KenticoCountryID))
                .ForMember(dest => dest.StateName, opt => opt.MapFrom(src => src.StateDisplayName));
            CreateMap<AddressDTO, Country>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.isoCountryCode))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.KenticoCountryID))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Country));
            CreateMap<AddressDTO, DeliveryAddress>()
                .ForMember(dest => dest.Address1, opt => opt.MapFrom(src => src.AddressLine1))
                .ForMember(dest => dest.Address2, opt => opt.MapFrom(src => src.AddressLine2))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.AddressCompanyName))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.KenticoAddressID.GetValueOrDefault()))
                .ForMember(dest => dest.Checked, opt => opt.Ignore())
                .ForMember(dest => dest.AddressName, opt => opt.Ignore())
                .ForMember(dest => dest.CustomerId, opt => opt.Ignore());
            CreateMap<DeliveryAddress, AddressDTO>()
                .ForMember(dest => dest.AddressLine1, opt => opt.MapFrom(src => src.Address1))
                .ForMember(dest => dest.AddressLine2, opt => opt.MapFrom(src => src.Address2))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.KenticoCountryID, opt => opt.MapFrom(src => src.Country.Id))
                .ForMember(dest => dest.isoCountryCode, opt => opt.MapFrom(src => src.Country.Code))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country.Name))
                .ForMember(dest => dest.AddressCompanyName, opt => opt.MapFrom(src => src.CompanyName))
                .ForMember(dest => dest.KenticoAddressID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.StateDisplayName, opt => opt.ResolveUsing(src => src.State?.StateDisplayName))
                .ForMember(dest => dest.KenticoStateID, opt => opt.ResolveUsing(src => src.State?.Id))
                .ForMember(dest => dest.State
                    , opt => opt.ResolveUsing(src => !string.IsNullOrEmpty(src.State?.StateCode) ? src.State.StateCode : src.Country?.Name));
            CreateMap<DeliveryAddress, Dto.ViewOrder.Responses.AddressDto>()
                .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.CompanyName))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.AddressPersonalName))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State.StateDisplayName))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country.Name));
            CreateMap<LogonUserRequestDTO, LoginRequest>();
            CreateMap<LoginResult, LogonUserResultDTO>();
            CreateMap<CheckTaCResult, CheckTaCResultDTO>();
            CreateMap<BusinessUnit, BusinessUnitDto>();
            CreateMap<Brand, BrandDto>();
            CreateMap<OrderCampaginHead, OrderCampaginHeadDto>();
            CreateMap<OrderCampaginItem, OrderCampaginItemDto>();

            CreateMap<OrderHeadBlock, OrderHeadBlockDto>();
            CreateMap<OrderRow, RecentOrderRowDto>();
            CreateMap<OrderDialog, OrderDialogDto>();
            CreateMap<OrderDailogLabel, OrderDailogLabelDto>();
            CreateMap<OrderDialogTable, OrderDialogTableDto>();
            CreateMap<OrderTableCell, OrderTableCellDto>();
            CreateMap<OrderDialogTableCell, OrderDialogTableCellDto>();

            CreateMap<FailedOrder, FailedOrderDto>();

            CreateMap<DistributorDTO, Distributor>();
            CreateMap<string, CreditCardPaymentDoneDto>()
                .ForMember(dest => dest.RedirectionURL, opt => opt.MapFrom(src => src));
            CreateMap<SaveTokenDataRequestDto, SaveTokenData>();
            CreateMap<CartItems, ChangeItemQuantityResponseDto>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src));
            CreateMap<DeliveryAddresses, ChangeDeliveryAddressResponseDto>()
                .ForMember(dest => dest.DeliveryAddresses, opt => opt.MapFrom(src => src));
            CreateMap<SaveCreditCardRequestDto, SaveCardData>();
            CreateMap<StoredCard, StoredCardDto>();
            CreateMap<DistributorCart, DistributorCartDto>();
            CreateMap<DistributorCartItem, DistributorCartItemDto>();
            CreateMap<DistributorCartDto, DistributorCart>();
            CreateMap<DistributorCartItemDto, DistributorCartItem>();

            CreateMap<UserDto, User>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.TermsConditionsAccepted, opt => opt.Ignore())
                .ForMember(dest => dest.IsExternal, opt => opt.Ignore());
            CreateMap<CustomerDto, Customer>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CustomerNumber, opt => opt.Ignore())
                .ForMember(dest => dest.UserID, opt => opt.Ignore())
                .ForMember(dest => dest.Company, opt => opt.Ignore())
                .ForMember(dest => dest.SiteId, opt => opt.Ignore())
                .ForMember(dest => dest.PreferredLanguage, opt => opt.Ignore())
                .ForMember(dest => dest.ApproverUserId, opt => opt.Ignore())
                .ForMember(dest => dest.DefaultShippingAddressId, opt => opt.Ignore());
            CreateMap<Dto.SSO.AddressDto, DeliveryAddress>()
                .ForMember(dest => dest.Country, opt => opt.ResolveUsing(src => new Country { Code = src.Country }))
                .ForMember(dest => dest.State, opt => opt.ResolveUsing(src => new State { StateCode = src.State }))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Checked, opt => opt.Ignore())
                .ForMember(dest => dest.AddressPersonalName, opt => opt.Ignore())
                .ForMember(dest => dest.AddressName, opt => opt.Ignore())
                .ForMember(dest => dest.CustomerId, opt => opt.Ignore())
                .ForMember(dest => dest.CompanyName, opt => opt.Ignore());
            CreateMap<EmailProofRequestDto, EmailProofRequest>();
            CreateMap<ProductAvailability, ProductAvailabilityDto>();
            CreateMap<Dto.EstimateDeliveryPrice.MicroserviceRequests.AddressDto, AddressDTO>()
                .ForMember(dest => dest.AddressLine1, opt => opt.MapFrom(src => src.StreetLines != null && src.StreetLines.Count > 0 ? src.StreetLines[0] : string.Empty))
                .ForMember(dest => dest.AddressLine2, opt => opt.MapFrom(src => src.StreetLines != null && src.StreetLines.Count > 1 ? src.StreetLines[1] : string.Empty))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State))
                .ForMember(dest => dest.Zip, opt => opt.MapFrom(src => src.Postal))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
                .ForAllOtherMembers(m => m.Ignore());
            CreateMap<Weight, WeightDto>()
                .ReverseMap();
            CreateMap<OrderItemUpdateDto, OrderItemUpdate>();
            CreateMap<OrderUpdateDto, OrderUpdate>();
            CreateMap<AddressDTO, Dto.EstimateDeliveryPrice.MicroserviceRequests.AddressDto>()
                .ForMember(dest => dest.StreetLines, opt => opt.MapFrom(src => new[] { src.AddressLine1, src.AddressLine2 }.Where(s => !string.IsNullOrEmpty(s)).ToList()))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
                .ForMember(dest => dest.Postal, opt => opt.MapFrom(src => src.Zip))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State));
            CreateMap<OrderUpdateResult, OrderUpdateResultDto>();
            CreateMap<ItemUpdateResult, ItemUpdateResultDto>();

            CreateMap<Product, CartItemEntity>()
                .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.ProductType))
                .ForMember(dest => dest.ProductPageID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProductProductionTime, opt => opt.MapFrom(src => src.ProductionTime))
                .ForMember(dest => dest.ProductShipTime, opt => opt.MapFrom(src => src.ShipTime))
                .ForMember(dest => dest.ChiliTemplateID, opt => opt.MapFrom(src => src.ProductMasterTemplateID))
                .ForMember(dest => dest.ProductChiliPdfGeneratorSettingsId, opt => opt.MapFrom(src => src.TemplateHiResSettingId))
                .ForMember(dest => dest.ProductChiliWorkspaceId, opt => opt.MapFrom(src => src.ProductChiliWorkgroupID))
                .ForMember(dest => dest.SendPriceToErp, opt => opt.MapFrom(src => src.SendPriceToERP))
                .ForMember(dest => dest.UnitOfMeasure, opt => opt.MapFrom(src => src.UnitOfMeasure))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<Dto.ViewOrder.MicroserviceResponses.GetOrderByOrderIdResponseDTO, ShoppingCart>()
                .ForMember(dest => dest.CampaignId, opt => opt.ResolveUsing(src => src.campaign?.ID ?? 0))
                .ForMember(dest => dest.ProgramId, opt => opt.ResolveUsing(src => src.campaign?.ProgramID ?? 0))
                .ForMember(dest => dest.DistributorId, opt => opt.ResolveUsing(src => src.campaign?.DistributorID ?? 0))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Customer.KenticoUserID))
                .ForMember(dest => dest.CustomerId, opt => opt.Ignore())
                .ForMember(dest => dest.TotalTax, opt => opt.MapFrom(src => src.PaymentInfo.Tax))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.PaymentInfo.Summary))
                .ForMember(dest => dest.ShippingPrice, opt => opt.MapFrom(src => src.PaymentInfo.Shipping))
                .ForMember(dest => dest.PricedItemsTax, opt => opt.Ignore())
                .ForMember(dest => dest.TotalItemsWeight, opt => opt.Ignore())
                .ForMember(dest => dest.Type, opt => opt.ResolveUsing(src =>
                {
                    if (OrderType.generalInventory.Equals(src.Type))
                    {
                        return CampaignProductType.GeneralInventory;
                    }
                    if (OrderType.prebuy.Equals(src.Type))
                    {
                        return CampaignProductType.PreBuy;
                    }
                    return CampaignProductType.Standard;
                }))
                .ForMember(dest => dest.ShippingOptionId, opt => opt.MapFrom(src => src.ShippingInfo.ShippingOptionId))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.ShippingInfo.AddressTo));

            CreateMap<Dto.ViewOrder.MicroserviceResponses.OrderItemDTO, CartItemEntity>()
                .ForMember(dest => dest.CartItemID, opt => opt.Ignore())
                .ForMember(dest => dest.CartItemGUID, opt => opt.Ignore())
                .ForMember(dest => dest.ShoppingCartID, opt => opt.Ignore())
                .ForMember(dest => dest.UnitPrice, opt => opt.Ignore())
                .ForMember(dest => dest.CartItemPrice, opt => opt.Ignore())
                .ForMember(dest => dest.CartItemText, opt => opt.Ignore())
                .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.ChilliEditorTemplateID, opt => opt.MapFrom(src => src.TemplateId))
                .ForMember(dest => dest.ChiliTemplateID, opt => opt.Ignore())
                .ForMember(dest => dest.ArtworkLocation, opt => opt.MapFrom(src => src.FileKey))
                .ForMember(dest => dest.ProductPageID, opt => opt.MapFrom(src => src.DocumentId))
                .ForMember(dest => dest.MailingListName, opt => opt.MapFrom(src => src.MailingList))
                .ForMember(dest => dest.MailingListGuid, opt => opt.Ignore())
                .ForMember(dest => dest.ProductChiliWorkspaceId, opt => opt.Ignore())
                .ForMember(dest => dest.ProductChiliPdfGeneratorSettingsId, opt => opt.Ignore())
                .ForMember(dest => dest.ProductShipTime, opt => opt.Ignore())
                .ForMember(dest => dest.ProductProductionTime, opt => opt.Ignore())
                .ForMember(dest => dest.SendPriceToErp, opt => opt.Ignore());
        }
    }
}
