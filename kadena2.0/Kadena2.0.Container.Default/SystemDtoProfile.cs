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
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src[opt.DestinationMember.Name]))
                .ForAllOtherMembers(opt => opt.ResolveUsing((src) =>
                  {
                      if (src.TryGetValue(nameof(opt.DestinationMember.Name), out IEnumerable<string> values))
                      {
                          return values.FirstOrDefault();
                      }
                      return null;
                  }));
            CreateMap<Dictionary<string, IEnumerable<string>>, CustomerDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src[opt.DestinationMember.Name].FirstOrDefault()))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src[opt.DestinationMember.Name].FirstOrDefault()))
                .ForAllOtherMembers(opt => opt.ResolveUsing((src) =>
                {
                    if (src.TryGetValue(nameof(opt.DestinationMember.Name), out IEnumerable<string> values))
                    {
                        return values.FirstOrDefault();
                    }
                    return null;
                }));
            CreateMap<Dictionary<string, IEnumerable<string>>, AddressDto>()
                .ForAllMembers(opt => opt.ResolveUsing((src) =>
                {
                    if (src.TryGetValue(nameof(opt.DestinationMember.Name), out IEnumerable<string> values))
                    {
                        return values.FirstOrDefault();
                    }
                    return null;
                }));
        }
    }
}
