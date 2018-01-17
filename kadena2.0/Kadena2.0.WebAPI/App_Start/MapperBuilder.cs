using AutoMapper;
using Kadena.WebAPI.App_Start;
using Kadena2.WebAPI.KenticoProviders;

namespace Kadena.WebAPI
{
    public static class MapperBuilder
    {
        public static void InitializeAll() // todo consider separating aka builder after discussion 
        {
            Mapper.Initialize(config =>
            {
                config.AddProfile<KenticoModelMappingsProfile>();
                config.AddProfile<MapperDefaultProfile>();
            });
        }
    }
}