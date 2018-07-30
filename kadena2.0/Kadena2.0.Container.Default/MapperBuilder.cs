using AutoMapper;
using Kadena2.WebAPI.KenticoProviders;

namespace Kadena.Container.Default
{
    public static class MapperBuilder
    {
        private static readonly IMapper mapper;

        public static IMapper MapperInstance => mapper;

        static MapperBuilder()
        {

            mapper = new MapperConfiguration(cfg => 
            {
                cfg.AddProfiles(
                    typeof(KenticoModelMappingsProfile),
                    typeof(MapperDefaultProfile),
                    typeof(SystemDtoProfile)
                );
                cfg.AddProfiles("Kadena2.0.BusinessLogic");

            }).CreateMapper();
        }
     
    }
}
