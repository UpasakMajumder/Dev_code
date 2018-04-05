using AutoMapper;
using CMS.Globalization;
using CMS.Ecommerce;
using Kadena.Models;
using CMS.Membership;
using System;
using Kadena.Models.Site;
using CMS.SiteProvider;
using Kadena.WebAPI.KenticoProviders;
using CMS.DocumentEngine;
using Kadena.Models.Product;
using CMS.Helpers;
using CMS.CustomTables;
using Kadena.Models.CreditCard;
using Kadena.Models.Membership;
using Kadena.Models.Checkout;

namespace Kadena2.WebAPI.KenticoProviders
{
    public class KenticoModelMappingsProfile : Profile
    {
        public KenticoModelMappingsProfile()
        {
            CreateMap<OptionCategoryInfo, OptionCategory>()
                .ForMember(dest => dest.CodeName, opt => opt.MapFrom(src => src.CategoryName))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.CategoryDisplayName));

            CreateMap<SKUInfo, Sku>()
                .ForMember(dest => dest.NeedsShipping, opt => opt.MapFrom(src => src.SKUNeedsShipping))
                .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.SKUWeight));

            CreateMap<StateInfo, State>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.StateID));
            CreateMap<CountryInfo, Country>()
                .ProjectUsing(src => new Country
                {
                    Id = src.CountryID,
                    Name = src.CountryDisplayName,
                    Code = src.CountryTwoLetterCode
                });
            CreateMap<AddressInfo, DeliveryAddress>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AddressID))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.AddressCity))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.Address1, opt => opt.MapFrom(src => src.AddressLine1))
                .ForMember(dest => dest.Address2, opt => opt.MapFrom(src => src.AddressLine2))
                .ForMember(dest => dest.Zip, opt => opt.MapFrom(src => src.AddressZip))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.GetStringValue("Email", string.Empty)))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.GetStringValue("CompanyName", string.Empty)))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.AddressPhone));
            CreateMap<AddressInfo, State>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AddressStateID))
                .ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => src.AddressCountryID))
                .ForMember(dest => dest.StateCode, opt => opt.MapFrom(src => src.GetStateCode()));
            CreateMap<AddressInfo, Country>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AddressCountryID))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.GetCountryTwoLetterCode()));
            CreateMap<DeliveryAddress, AddressInfo>()
                .ForMember(dest => dest.AddressID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.AddressLine1, opt => opt.MapFrom(src => src.Address1))
                .ForMember(dest => dest.AddressLine2, opt => opt.MapFrom(src => src.Address2))
                .ForMember(dest => dest.AddressCity, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.AddressZip, opt => opt.MapFrom(src => src.Zip))
                .ForMember(dest => dest.AddressStateID, opt => opt.MapFrom(src => src.State.Id))
                .ForMember(dest => dest.AddressCountryID, opt => opt.MapFrom(src => src.Country.Id))
                .ForMember(dest => dest.AddressPhone, opt => opt.MapFrom(src => src.Phone))
                .AfterMap((src, dest) => dest.SetValue("Email", src.Email))
                .AfterMap((src, dest) => dest.SetValue("CompanyName", src.CompanyName));
            CreateMap<ShippingOptionInfo, DeliveryOption>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ShippingOptionID))
                .ForMember(dest => dest.CarrierId, opt => opt.MapFrom(src => src.ShippingOptionCarrierID))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.ShippingOptionDisplayName))
                .ForMember(dest => dest.Service, opt => opt.MapFrom(src => src.ShippingOptionCarrierServiceName))
                .ForMember(dest => dest.SAPName, opt => opt.MapFrom(src => src.GetStringValue("ShippingOptionSAPName", string.Empty)));
            CreateMap<UserInfo, User>()
                .ForMember(dest => dest.TermsConditionsAccepted, opt => opt.MapFrom(src => src.GetDateTimeValue("TermsConditionsAccepted", DateTime.MinValue)));
            CreateMap<UserSettingsInfo, UserSettings>()
                .ForMember(dest => dest.CallBackUrl, opt => opt.MapFrom(src => src.UserURLReferrer))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserSettingsUserID));
            CreateMap<SiteInfo, Site>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.SiteID))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.SiteName));
            CreateMap<PaymentOptionInfo, PaymentMethod>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PaymentOptionID))
                .ForMember(dest => dest.Disabled, opt => opt.MapFrom(src => !src.PaymentOptionEnabled))
                .ForMember(dest => dest.Icon, opt => opt.MapFrom(src => src.GetStringValue("IconResource", string.Empty)))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.PaymentOptionDisplayName))
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.PaymentOptionName))
                .ForMember(dest => dest.IsUnpayable, opt => opt.MapFrom(src => src.GetBooleanValue("IsUnpayable", false)))
                .AfterMap((src, dest) => dest.Checked = false);
            CreateMap<CustomerInfo, Customer>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CustomerID))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.CustomerFirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.CustomerLastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.CustomerEmail))
                .ForMember(dest => dest.CustomerNumber, opt => opt.MapFrom(src => src.CustomerGUID.ToString()))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.CustomerPhone))
                .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.CustomerUserID))
                .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.CustomerCompany))
                .ForMember(dest => dest.SiteId, opt => opt.MapFrom(src => src.CustomerSiteID))
                .ForMember(dest => dest.DefaultShippingAddressId, opt => opt.MapFrom(src => src.GetIntegerValue(KenticoAddressBookProvider.CustomerDefaultShippingAddresIDFieldName, 0)))
                .AfterMap((src, dest) => dest.PreferredLanguage = src.CustomerUser?.PreferredCultureCode ?? string.Empty);

            CreateMap<CarrierInfo, DeliveryCarrier>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CarrierID))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.CarrierDisplayName))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CarrierName))
                .AfterMap((src, dest) => dest.Opened = false);
            CreateMap<TreeNode, ProductCategoryLink>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.DocumentID))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.DocumentName))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.DocumentUrlPath))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => URLHelper.GetAbsoluteUrl(src.GetValue("ProductCategoryImage", string.Empty))))
                .ForMember(dest => dest.ProductBordersEnabled, opt => opt.MapFrom(src => src.GetBooleanValue("ProductCategoryBordersEnabled", false)))
                .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.NodeOrder))
                .AfterMap((src, dest) => dest.Border = new Border { Exists = src.GetBooleanValue("ProductCategoryBordersEnabled", false) });
            CreateMap<CustomTableItem, Submission>()
                .ForMember(dest => dest.SubmissionId, opt => opt.MapFrom(src => src.GetGuidValue("SubmissionId", Guid.Empty)))
                .ForMember(dest => dest.AlreadyVerified, opt => opt.MapFrom(src => src.GetBooleanValue("AlreadyVerified", false)))
                .ForMember(dest => dest.SiteId, opt => opt.MapFrom(src => src.GetIntegerValue("SiteId", 0)))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.GetIntegerValue("UserId", 0)))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.GetIntegerValue("CustomerId", 0)))
                .ForMember(dest => dest.Processed, opt => opt.MapFrom(src => src.GetBooleanValue("Processed", false)))
                .ForMember(dest => dest.Success, opt => opt.MapFrom(src => src.GetBooleanValue("Success", false)))
                .ForMember(dest => dest.OrderJson, opt => opt.MapFrom(src => src.GetStringValue("OrderJson", string.Empty)))
                .ForMember(dest => dest.Error, opt => opt.MapFrom(src => src.GetStringValue("Error", string.Empty)))
                .ForMember(dest => dest.SaveCardJson, opt => opt.MapFrom(src => src.GetStringValue("SaveCardJson", string.Empty)))
                .ForMember(dest => dest.RedirectUrl, opt => opt.MapFrom(src => src.GetStringValue("RedirectUrl", string.Empty)));
            CreateMap<AddressInfo, AddressData>();
            CreateMap<RoleInfo, Role>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RoleID))
                .ForMember(dest => dest.CodeName, opt => opt.MapFrom(src => src.RoleName))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.RoleDisplayName))
                .ReverseMap();

            CreateMap<ShoppingCartItemInfo, CartItemEntity>()
                .ForMember(dest => dest.ArtworkLocation, opt => opt.MapFrom(src => src.GetStringValue("ArtworkLocation", null)))
                .ForMember(dest => dest.ChiliTemplateID, opt => opt.MapFrom(src => src.GetGuidValue("ChiliTemplateID", Guid.Empty)))
                .ForMember(dest => dest.ChilliEditorTemplateID, opt => opt.MapFrom(src => src.GetGuidValue("ChilliEditorTemplateID", Guid.Empty)))
                .ForMember(dest => dest.MailingListGuid, opt => opt.MapFrom(src => src.GetGuidValue("MailingListGuid", Guid.Empty)))
                .ForMember(dest => dest.MailingListName, opt => opt.MapFrom(src => src.GetStringValue("MailingListName", null)))
                .ForMember(dest => dest.ProductChiliPdfGeneratorSettingsId, opt => opt.MapFrom(src => src.GetGuidValue("ProductChiliPdfGeneratorSettingsId", Guid.Empty)))
                .ForMember(dest => dest.ProductChiliWorkspaceId, opt => opt.MapFrom(src => src.GetGuidValue("ProductChiliWorkspaceId", Guid.Empty)))
                .ForMember(dest => dest.ProductPageID, opt => opt.MapFrom(src => src.GetIntegerValue("ProductPageID", 0)))
                .ForMember(dest => dest.ProductProductionTime, opt => opt.MapFrom(src => src.GetValue("ProductProductionTime", string.Empty)))
                .ForMember(dest => dest.ProductShipTime, opt => opt.MapFrom(src => src.GetValue("ProductShipTime", string.Empty)))
                .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.GetValue("ProductType", string.Empty)))
                .ForMember(dest => dest.CartItemPrice, opt => opt.MapFrom(src => (decimal)src.GetDoubleValue("CartItemPrice", 0.0d)))
                .ForMember(dest => dest.SKUUnits, opt => opt.MapFrom(src => src.GetIntegerValue("SKUUnits", 0)));

            CreateMap<CartItemEntity, ShoppingCartItemInfo>()
                .AfterMap((src, dest) => dest.SetValue("ArtworkLocation", src.ArtworkLocation))
                .AfterMap((src, dest) => dest.SetValue("ChiliTemplateID", src.ChiliTemplateID))
                .AfterMap((src, dest) => dest.SetValue("ChilliEditorTemplateID", src.ChilliEditorTemplateID))
                .AfterMap((src, dest) => dest.SetValue("MailingListGuid", src.MailingListGuid))
                .AfterMap((src, dest) => dest.SetValue("MailingListName", src.MailingListName))
                .AfterMap((src, dest) => dest.SetValue("ProductChiliPdfGeneratorSettingsId", src.ProductChiliPdfGeneratorSettingsId))
                .AfterMap((src, dest) => dest.SetValue("ProductChiliWorkspaceId", src.ProductChiliWorkspaceId))
                .AfterMap((src, dest) => dest.SetValue("ProductPageID", src.ProductPageID))
                .AfterMap((src, dest) => dest.SetValue("ProductProductionTime", src.ProductProductionTime))
                .AfterMap((src, dest) => dest.SetValue("ProductShipTime", src.ProductShipTime))
                .AfterMap((src, dest) => dest.SetValue("ProductType", src.ProductType))
                .AfterMap((src, dest) => dest.SetValue("CartItemPrice", src.CartItemPrice))
                .AfterMap((src, dest) => dest.SetValue("SKUUnits", src.SKUUnits));
            CreateMap<Customer, CustomerInfo>()
                .ForMember(dest => dest.CustomerID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CustomerFirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.CustomerLastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.CustomerEmail, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.CustomerGUID, opt => opt.MapFrom(src => new Guid(src.CustomerNumber)))
                .ForMember(dest => dest.CustomerPhone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.CustomerUserID, opt => opt.MapFrom(src => src.UserID))
                .ForMember(dest => dest.CustomerCompany, opt => opt.MapFrom(src => src.Company))
                .ForMember(dest => dest.CustomerSiteID, opt => opt.MapFrom(src => src.SiteId));
            CreateMap<SKUInfo, Sku>()
                .ForMember(dest => dest.SkuId, opt => opt.MapFrom(src => src.SKUID))
                .ForMember(dest => dest.NeedsShipping, opt => opt.MapFrom(src => src.SKUNeedsShipping))
                .ForMember(dest => dest.SellOnlyIfAvailable, opt => opt.MapFrom(src => src.SKUSellOnlyAvailable))
                .ForMember(dest => dest.AvailableItems, opt => opt.MapFrom(src => src.SKUAvailableItems))
                .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.SKUWeight));

            CreateMap<User, UserInfo>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
            CreateMap<UserSettings, UserSettingsInfo>()
                .ForMember(dest => dest.UserURLReferrer, opt => opt.MapFrom(src => src.CallBackUrl))
                .ForMember(dest => dest.UserSettingsUserID, opt => opt.MapFrom(src => src.UserId));

        }
    }
}
