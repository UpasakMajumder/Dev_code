using AutoMapper;
using CMS.Globalization;
using CMS.Ecommerce;
using Kadena.Models;
using CMS.Membership;
using System;

namespace Kadena2.WebAPI.KenticoProviders
{
    public class KenticoModelMappingsProfile : Profile
    {
        public KenticoModelMappingsProfile()
        {
            CreateMap<StateInfo, State>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.StateID));
            CreateMap<CountryInfo, Country>()
                .ProjectUsing(src => new Country
                {
                    Id = src.CountryID,
                    Name = src.CountryDisplayName,
                    Code = src.CountryTwoLetterCode
                });
            CreateMap<IAddress, DeliveryAddress>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AddressID))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.AddressCity))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.Address1, opt => opt.MapFrom(src => src.AddressLine1))
                .ForMember(dest => dest.Address2, opt => opt.MapFrom(src => src.AddressLine2))
                .ForMember(dest => dest.Zip, opt => opt.MapFrom(src => src.AddressZip));
            CreateMap<IAddress, State>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AddressStateID))
                .ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => src.AddressCountryID))
                .ForMember(dest => dest.StateCode, opt => opt.MapFrom(src => src.GetStateCode()));
            CreateMap<IAddress, Country>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AddressCountryID))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.GetCountryTwoLetterCode()));
            CreateMap<DeliveryAddress, AddressInfo>()
                .ForMember(dest => dest.AddressID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.AddressLine1, opt => opt.MapFrom(src => src.Address1))
                .ForMember(dest => dest.AddressLine2, opt => opt.MapFrom(src => src.Address2))
                .ForMember(dest => dest.AddressCity, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.AddressZip, opt => opt.MapFrom(src => src.Zip))
                .ForMember(dest => dest.AddressStateID, opt => opt.MapFrom(src => src.State.Id))
                .ForMember(dest => dest.AddressCountryID, opt => opt.MapFrom(src => src.Country.Id));
            CreateMap<ShippingOptionInfo, DeliveryOption>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ShippingOptionID))
                .ForMember(dest => dest.CarrierId, opt => opt.MapFrom(src => src.ShippingOptionCarrierID))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.ShippingOptionDisplayName))
                .ForMember(dest => dest.Service, opt => opt.MapFrom(src => src.ShippingOptionCarrierServiceName))
                .ForMember(dest => dest.SAPName, opt => opt.MapFrom(src => src.GetStringValue("ShippingOptionSAPName", string.Empty)));
            CreateMap<UserInfo, User>()
                .ForMember(dest => dest.TermsConditionsAccepted, opt => opt.MapFrom(src => src.GetDateTimeValue("TermsConditionsAccepted", DateTime.MinValue)));
        }
    }
}
