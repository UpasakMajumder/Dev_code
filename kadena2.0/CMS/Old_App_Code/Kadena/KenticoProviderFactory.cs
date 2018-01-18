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
        public static IKenticoLogger KenticoLogger => ContainerBuilder.Resolve<IKenticoLogger>();
        public static IKadenaSettings KadenaSettings => ContainerBuilder.Resolve<IKadenaSettings>();
        public static IKenticoResourceService KenticoResourceService => ContainerBuilder.Resolve<IKenticoResourceService>();
        public static IKenticoSiteProvider KenticoSiteProvider => ContainerBuilder.Resolve<IKenticoSiteProvider>();
        public static IMicroProperties MicroProperties => ContainerBuilder.Resolve<IMicroProperties>();
        public static ISuppliantDomainClient SuppliantDomain => ContainerBuilder.Resolve<ISuppliantDomainClient>();
        public static IKenticoLocalizationProvider KenticoLocalizationProvider => ContainerBuilder.Resolve<IKenticoLocalizationProvider>();
    }
}