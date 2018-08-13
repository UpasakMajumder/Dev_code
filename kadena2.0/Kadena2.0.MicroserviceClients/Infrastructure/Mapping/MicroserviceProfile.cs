using AutoMapper;
using Kadena.Dto.Membership;
using Kadena.Models.Membership;

namespace Kadena2.MicroserviceClients.Infrastructure.Mapping
{
    public class MicroserviceProfile : Profile
    {
        public MicroserviceProfile()
        {
            CreateMap<User, CreateUserDto>()
                .ForMember(dest => dest.Password, opt => opt.Ignore());
        }
    }
}
