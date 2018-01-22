using AutoMapper;
using DryIoc;
using Kadena.ScheduledTasks.DeleteExpiredMailingLists;
using Kadena.ScheduledTasks.Infrastructure;
using Kadena.ScheduledTasks.Infrastructure.Kentico;
using Kadena.ScheduledTasks.UpdateInventoryData;
using Kadena2.Container.Default;
using Kadena2.WebAPI.KenticoProviders;

namespace Kadena.ScheduledTasks
{
    public static class Services
    {
        private static IContainer container = null;

        static Services()
        {
            container = new Container();
            RegisterServices(container);
        }
    
        public static T Resolve<T>()
        {
            if (container == null)
            {
                var processName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
                ProviderFactory.KenticoLogger.LogError("Scheduled task container", $"[{processName}] container is null");
            }

            return container.Resolve<T>();
        }

        private static void RegisterServices(IContainer container)
        {
            container
                .RegisterKentico()
                .RegisterMicroservices()
                .RegisterKadenaSettings();

            // scheduled tasks infrastructure
            container.Register<Infrastructure.IConfigurationProvider, KenticoConfigurationProvider>();
            container.RegisterInstance(typeof(IMapper), CreateMapper());

            // scheduled tasks services
            container.Register<DeleteExpiredMailingListsService, DeleteExpiredMailingListsService>();
            container.Register<IUpdateInventoryDataService, UpdateInventoryDataService>();
        }

        private static IMapper CreateMapper()
        {
            return new MapperConfiguration(cfg => cfg.AddProfile<KenticoModelMappingsProfile>()).CreateMapper();
        }
    }
}