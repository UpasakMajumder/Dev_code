using AutoMapper;
using DryIoc;
using Kadena.Helpers;
using Kadena.ScheduledTasks.DeleteExpiredMailingLists;
using Kadena.ScheduledTasks.Infrastructure;
using Kadena.ScheduledTasks.Infrastructure.Kentico;
using Kadena.ScheduledTasks.UpdateInventoryData;
using Kadena.WebAPI.KenticoProviders;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Clients;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.MicroserviceClients.Contracts.Base;
using Kadena2.WebAPI.KenticoProviders;

namespace Kadena.ScheduledTasks
{
    public static class Services
    {
        private static IContainer container;
        private static object initializationLock = new object();

        public static T Resolve<T>()
        {
            if (container == null)
            {
                lock (initializationLock)
                {
                    if (container == null)
                    {
                        var newContainer = new Container();
                        ConfigureMapper();
                        RegisterServices(newContainer);

                        container = newContainer;
                    }
                }
            }

            return container.Resolve<T>();
        }

        private static void RegisterServices(IContainer container)
        {
            // infrastructure
            container.Register<Infrastructure.IConfigurationProvider, KenticoConfigurationProvider>();
            container.Register<IKenticoProviderService, KenticoProviderService>();
            container.Register<IKenticoResourceService, KenticoResourceService>();
            container.Register<IKenticoLogger, KenticoLogger>();
            container.RegisterInstance(typeof(IMapper), Mapper.Instance);

            // microservices
            container.Register<IMailingListClient, MailingListClient>();
            container.Register<IInventoryUpdateClient, InventoryUpdateClient>();

            // task services
            container.Register<DeleteExpiredMailingListsService, DeleteExpiredMailingListsService>();
            container.Register<IUpdateInventoryDataService, UpdateInventoryDataService>();

            // helpers
            container.Register<IMicroProperties, MicroProperties>();
        }

        private static void ConfigureMapper()
        {
            Mapper.Initialize(cfg => cfg.AddProfile<KenticoModelMappingsProfile>());
        }
    }
}