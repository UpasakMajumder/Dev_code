using AutoMapper;
using DryIoc;
using Kadena.ScheduledTasks.Infrastructure;
using Kadena.ScheduledTasks.Infrastructure.Kentico;
using Kadena.ScheduledTasks.UpdateInventoryData;
using Kadena.WebAPI.KenticoProviders;
using Kadena.Container.Default;
using Kadena2.WebAPI.KenticoProviders;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Models.Site;

namespace Kadena.ScheduledTasks
{
    public static class Services
    {
        private static IContainer container = null;

        static Services()
        {
            container = new DryIoc.Container();
            RegisterServices(container);
        }

        public static T Resolve<T>()
        {
            if (container == null)
            {
                var processName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
                new KenticoLogger().LogError("Scheduled task container", $"[{processName}] container is null");
            }

            return container.Resolve<T>();
        }

        public static void UpdateInstance<T>(T instance)
        {
            container.Unregister(instance.GetType());
            container.RegisterInstance(instance);
        }

        public static void Register<T1, T2>(Setup setup = null) where T2 : T1
        {
            container.Register<T1, T2>(setup: setup);
        }

        private static void RegisterServices(IContainer container)
        {
            container
                .RegisterBLL()
                .RegisterKentico()
                .RegisterMicroservices()
                .RegisterKadenaSettings();

            // scheduled tasks infrastructure
            container.Register<Infrastructure.IConfigurationProvider, KenticoConfigurationProvider>();
            container.RegisterInstance(typeof(IMapper), CreateMapper());


            // scheduled tasks services
            container.Register<IUpdateInventoryDataService, UpdateInventoryDataService>();

            container.Register<IKenticoSiteProvider, SpecificSiteProvider>(setup: Setup.Decorator);
            container.Register<IKenticoResourceService, SpecificResourceService>(setup: Setup.Decorator);
            container.Register<KenticoSite>();
        }

        private static IMapper CreateMapper()
        {
            return new MapperConfiguration(cfg => cfg.AddProfile<KenticoModelMappingsProfile>()).CreateMapper();
        }
    }
}