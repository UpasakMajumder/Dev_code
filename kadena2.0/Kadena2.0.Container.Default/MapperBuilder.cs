using AutoMapper;
using Kadena2.WebAPI.KenticoProviders;

namespace Kadena2.Container.Default
{
    public static class MapperBuilder
    {
        private static readonly IMapper mapper;

        public static IMapper MapperInstance => mapper;

        static MapperBuilder()
        {

            mapper = new MapperConfiguration(cfg => cfg.AddProfiles(typeof(KenticoModelMappingsProfile),
                                                                    typeof(MapperDefaultProfile)
                                             )).CreateMapper();
        }
     
    }
}
