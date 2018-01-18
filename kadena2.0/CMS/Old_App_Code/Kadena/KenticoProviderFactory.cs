using Kadena.Helpers;
using Kadena.WebAPI.KenticoProviders;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.Container.Default;
using Kadena2.MicroserviceClients.Contracts.Base;
using Kadena2.WebAPI.KenticoProviders.Contracts.KadenaSettings;
using Kadena2.WebAPI.KenticoProviders.Providers.KadenaSettings;

namespace Kadena.Old_App_Code.Kadena
{
    public static class ProviderFactory
    {
        public static IKenticoLogger KenticoLogger => new KenticoLogger();
        public static IKadenaSettings KadenaSettings => new KadenaSettings(KenticoResourceService);
        public static IKenticoResourceService KenticoResourceService => new KenticoResourceService();
        public static IKenticoSiteProvider KenticoSiteProvider => new KenticoSiteProvider(MapperBuilder.MapperInstance, KadenaSettings);
        public static IMicroProperties MicroProperties => new MicroProperties(KenticoResourceService, KenticoSiteProvider);
        public static ISuppliantDomainClient SuppliantDomain => new SuppliantDomain(KenticoSiteProvider);
        public static IKenticoLocalizationProvider KenticoLocalizationProvider => new KenticoLocalizationProvider(MapperBuilder.MapperInstance);
    }
}