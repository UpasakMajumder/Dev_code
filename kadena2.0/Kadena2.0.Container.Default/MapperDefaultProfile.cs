using AutoMapper;
using Kadena.Dto.Brands;
using Kadena.Dto.BusinessUnits;
using Kadena.Dto.Checkout;
using Kadena.Dto.Checkout.Responses;
using Kadena.Dto.Common;
using Kadena.Dto.CreditCard._3DSi.Requests;
using Kadena.Dto.CreditCard.Requests;
using Kadena.Dto.CreditCard.Responses;
using Kadena.Dto.CustomerData;
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
using Kadena.Models.Common;
using Kadena.Models.CreditCard;
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
using System.Linq;

namespace Kadena2.Container.Default
{
    public class MapperDefaultProfile : Profile
    {
        public MapperDefaultProfile()
        {
            CreateMap<ChiliProcess, ChiliProcessDto>();
            CreateMap<ItemOption, ItemOptionDto>();

            CreateMap<Price, PriceDto>()
                .ForMember(dest => dest.PricePrefix, opt => opt.MapFrom(src => src.Prefix))
                .ForMember(dest => dest.PriceValue, opt => opt.MapFrom(src => src.Value));
            CreateMap<CartItem, SKUDTO>()
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.SKUName))
                    .ForMember(dest => dest.KenticoSKUID, opt => opt.MapFrom(src => src.SKUID));

            CreateMap<CartItem, OrderItemDTO>()
                .ForMember(dest => dest.SKU, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.UnitCount, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Attributes, opt => opt.MapFrom(src => src.Options.ToDictionary(i => i.Name, i => i.Value)));

            CreateMap<CustomerData, CustomerDataDTO>();
            CreateMap<CustomerAddress, CustomerAddressDTO>();
            CreateMap<CartItems, CartItemsDTO>();
            CreateMap<CartItem, CartItemDTO>()
                .AfterMap((src, dest) => dest.Price = src.PriceText)
                .AfterMap((src, dest) => dest.MailingList = src.MailingListName);
            CreateMap<CartItem, CartItemPreviewDTO>()
                .AfterMap((src, dest) => dest.Price = src.PriceText)
                .AfterMap((src, dest) => dest.MailingList = src.MailingListName);
            CreateMap<Kadena.Models.PaymentMethod, PaymentMethodDTO>();
            CreateMap<PaymentMethods, PaymentMethodsDTO>();
            CreateMap<Total, TotalDTO>();
            CreateMap<Totals, TotalsDTO>();
            CreateMap<DeliveryOption, DeliveryServiceDTO>();
            CreateMap<DeliveryCarriers, DeliveryMethodsDTO>();
            CreateMap<DeliveryCarrier, DeliveryMethodDTO>();
            CreateMap<DeliveryAddresses, DeliveryAddressesDTO>();

            CreateMap<DeliveryAddress, DeliveryAddressDTO>()
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country.Id))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State.Id));

            CreateMap<DeliveryAddressDTO, DeliveryAddress>()
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

            CreateMap<DeliveryAddressDTO, Country>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Country));

            CreateMap<DeliveryAddressDTO, State>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.State));

            CreateMap<CheckoutPage, CheckoutPageDTO>();
            CreateMap<NotificationEmail, NotificationEmailDto>();
            CreateMap<NotificationEmailTooltip, NotificationEmailTooltipDto>();
            CreateMap<CheckoutPageDeliveryTotals, CheckoutPageDeliveryTotalsDTO>();
            CreateMap<SubmitButton, SubmitButtonDTO>();
            CreateMap<SubmitRequestDto, SubmitOrderRequest>();
            CreateMap<SubmitOrderResult, SubmitOrderResponseDto>();
            CreateMap<PaymentMethodDto, Kadena.Models.SubmitOrder.PaymentMethod>();
            CreateMap<DeliveryAddress, Kadena.Dto.Settings.AddressDto>()
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State.Id))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country.Id));
            CreateMap<Kadena.Dto.Settings.AddressDto, Country>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Country));
            CreateMap<Kadena.Dto.Settings.AddressDto, State>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.State));
            CreateMap<Kadena.Dto.Settings.AddressDto, DeliveryAddress>()
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
            CreateMap<Kadena.Models.Settings.AddressDialog, Kadena.Dto.Settings.AddressDialogDto>();
            CreateMap<Kadena.Models.Checkout.AddressDialog, Kadena.Dto.Checkout.AddressDialogDto>();
            CreateMap<DefaultAddress, DefaultAddressDto>();
            CreateMap<SettingsAddresses, SettingsAddressesDto>();
            CreateMap<OrderedItem, OrderedItemDTO>();
            CreateMap<OrderedItems, OrderedItemsDTO>();
            CreateMap<OrderDetail, OrderDetailDTO>();
            CreateMap<CommonInfo, CommonInfoDTO>();
            CreateMap(typeof(TitleValuePair<>), typeof(TitleValuePairDto<>));
            CreateMap<ShippingInfo, ShippingInfoDTO>();
            CreateMap<PaymentInfo, PaymentInfoDTO>();
            CreateMap<PricingInfo, PricingInfoDTO>();
            CreateMap<Tracking, TrackingDTO>();
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
            CreateMap<Kadena.Dto.Order.OrderItemDto, CartItem>()
                .ProjectUsing(s => new CartItem { SKUName = s.Name, Quantity = s.Quantity });
            CreateMap<RecentOrderDto, Order>();
            CreateMap<OrderListDto, OrderList>();
            CreateMap<CartItem, Kadena.Dto.RecentOrders.OrderItemDto>()
                .ProjectUsing(s => new Kadena.Dto.RecentOrders.OrderItemDto { Name = s.SKUName, Quantity = s.Quantity.ToString() });
            CreateMap<Button, ButtonDto>();
            CreateMap<Campaign, CampaignDTO>();
            CreateMap<CampaignDTO, Campaign>();
            CreateMap<Order, OrderRowDto>()
                .AfterMap((s, d) =>
                {
                    d.OrderNumber = s.Id;
                    d.OrderDate = s.CreateDate;
                    d.OrderStatus = s.Status;
                });
            CreateMap<OrderBody, OrderBodyDto>();
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
            CreateMap<MailingAddress, MailingAddressDto>().AfterMap((s, d) => d.FirstName = s.Name);
            CreateMap<CartItemsPreview, CartItemsPreviewDTO>();
            CreateMap<CartPrice, CartPriceDTO>();
            CreateMap<MailingListDataDTO, MailingList>();
            CreateMap<NewCartItemDto, NewCartItem>();
            CreateMap<AddToCartResult, AddToCartResultDto>();
            CreateMap<RequestResult, RequestResultDto>();
            CreateMap<ArtworkFtpSettings, ArtworkFtpResponseDto>();
            CreateMap<FtpCredentials, FtpCredentialsDto>();
            CreateMap<CartEmptyInfo, CartEmptyInfoDTO>();
            CreateMap<MailTemplate, MailTemplateDto>();
            CreateMap<KenticoSite, SiteDataResponseDto>();
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
                    .ForMember(dest => dest.StateCode, opt => opt.MapFrom(src => src.State));
            CreateMap<AddressDTO, DeliveryAddress>()
                .ForMember(dest => dest.Address1, opt => opt.MapFrom(src => src.AddressLine1))
                .ForMember(dest => dest.Address2, opt => opt.MapFrom(src => src.AddressLine2))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.Country, opt => opt.Ignore());
            CreateMap<DeliveryAddress, Kadena.Dto.ViewOrder.Responses.AddressDto>()
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State.StateDisplayName))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country.Name));
            CreateMap<LogonUserRequestDTO, LoginRequest>();
            CreateMap<LoginResult, LogonUserResultDTO>();
            CreateMap<CheckTaCRequestDTO, LoginRequest>();
            CreateMap<AcceptTaCRequestDTO, LoginRequest>();
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
            CreateMap<DistributorDTO,Distributor>();
            CreateMap<string, CreditCardPaymentDoneDto>()
                .ForMember(dest => dest.RedirectionURL, opt => opt.MapFrom(src => src));
            CreateMap<SaveTokenDataRequestDto, SaveTokenData>();
            CreateMap<CartItems, ChangeItemQuantityResponseDto>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src));
            CreateMap<DeliveryAddresses, ChangeDeliveryAddressResponseDto>()
                .ForMember(dest => dest.DeliveryAddresses, opt => opt.MapFrom(src => src));
   		    CreateMap<SaveCreditCardRequestDto, SaveCardData>();
            CreateMap<StoredCard, StoredCardDto>();
        }
    }
}
