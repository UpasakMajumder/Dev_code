using AutoMapper;
using CMS.Globalization;
using CMS.Ecommerce;
using Kadena.Models;

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
                .ForMember(dest => dest.Street1, opt => opt.MapFrom(src => src.AddressLine1))
                .ForMember(dest => dest.Street2, opt => opt.MapFrom(src => src.AddressLine2))
                .ForMember(dest => dest.Zip, opt => opt.MapFrom(src => src.AddressZip));
            CreateMap<IAddress, State>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AddressStateID))
                .ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => src.AddressCountryID))
                .ForMember(dest => dest.StateCode, opt => opt.MapFrom(src => src.GetStateCode()));
            CreateMap<IAddress, Country>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AddressCountryID))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.GetCountryTwoLetterCode()));
        }
    }
}
