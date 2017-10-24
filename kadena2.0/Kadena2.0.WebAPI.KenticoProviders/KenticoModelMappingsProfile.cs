using AutoMapper;
using CMS.Globalization;
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
                    Name = src.CountryDisplayName
                });
        }
    }
}
