using AutoMapper;
using Kadena.Dto.SSO;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.Container.Default
{
    public class SystemDtoProfile : Profile
    {
        public SystemDtoProfile()
        {
            CreateMap<Dictionary<string, IEnumerable<string>>, UserDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src[opt.DestinationMember.Name].FirstOrDefault()))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src[opt.DestinationMember.Name].FirstOrDefault()))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src[opt.DestinationMember.Name].FirstOrDefault()))
                .ForMember(dest => dest.Roles, opt => opt.ResolveUsing((src) =>
                {
                    IEnumerable<string> values;
                    if (src.TryGetValue(opt.DestinationMember.Name, out values))
                    {
                        return values;
                    }
                    return null;
                }))
                .ForAllOtherMembers(opt => opt.ResolveUsing((src) => GetFirstStringByKey(opt.DestinationMember.Name, src)));
            CreateMap<Dictionary<string, IEnumerable<string>>, CustomerDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src[opt.DestinationMember.Name].FirstOrDefault()))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src[opt.DestinationMember.Name].FirstOrDefault()))
                .ForAllOtherMembers(opt => opt.ResolveUsing((src) => GetFirstStringByKey(opt.DestinationMember.Name, src)));
            CreateMap<Dictionary<string, IEnumerable<string>>, AddressDto>()
                .ForAllMembers(opt => opt.ResolveUsing((src) => GetFirstStringByKey(opt.DestinationMember.Name, src)));
        }

        private string GetFirstStringByKey(string key, Dictionary<string, IEnumerable<string>> source)
        {
            IEnumerable<string> values;
            if (source.TryGetValue(key, out values))
            {
                return values.FirstOrDefault();
            }
            return null;
        }
    }
}
