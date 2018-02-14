using CMS.SiteProvider;
using Kadena.ScheduledTasks.Infrastructure;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.ScheduledTasks.GenerateOrders
{
    class OrderCreationService : IOrderCreationService
    {
        private IShoppingCartProvider shoppingCartProvider;
        private IKenticoAddressBookProvider kenticoAddressBookService;
        private IKenticoUserProvider KenticoUserProvider;
        private IKenticoResourceService kenticoresourceService;
        private IFailedOrderStatusProvider failedOrderStatusProvider;
        public OrderCreationService(IShoppingCartProvider shoppingCartProvider,
            IKenticoAddressBookProvider kenticoAddressBookService, IKenticoUserProvider KenticoUserProvider, 
            IKenticoResourceService kenticoresourceService, IFailedOrderStatusProvider failedOrderStatusProvider)
        {
            if (shoppingCartProvider == null)
            {
                throw new ArgumentNullException(nameof(shoppingCartProvider));
            }
            if (kenticoAddressBookService == null)
            {
                throw new ArgumentNullException(nameof(kenticoAddressBookService));
            }
            if (KenticoUserProvider == null)
            {
                throw new ArgumentNullException(nameof(KenticoUserProvider));
            }
            if (kenticoresourceService == null)
            {
                throw new ArgumentNullException(nameof(kenticoresourceService));
            }
            if (failedOrderStatusProvider == null)
            {
                throw new ArgumentNullException(nameof(failedOrderStatusProvider));
            }
            this.shoppingCartProvider = shoppingCartProvider;
            this.kenticoAddressBookService = kenticoAddressBookService;
            this.KenticoUserProvider = KenticoUserProvider;
            this.kenticoresourceService = kenticoresourceService;
            this.failedOrderStatusProvider = failedOrderStatusProvider;
        }
        public string GenerateOrder(int openCampaignID, int campaignClosingUserID)
        {
            var usersWithShoppingCartItems = shoppingCartProvider.GetUserIDsWithShoppingCart(openCampaignID, 1);
            var orderTemplateSettingKey = kenticoresourceService.GetSettingsKey("KDA_OrderReservationEmailTemplate");
            var failedOrderTemplateSettingKey = kenticoresourceService.GetSettingsKey("KDA_FailedOrdersEmailTemplate");
            var failedOrdersUrl = kenticoresourceService.GetSettingsKey("KDA_FailedOrdersPageUrl");
            var unprocessedDistributorIDs = new List<Tuple<int, string>>();
            usersWithShoppingCartItems.ForEach(shoppingCartUser =>
            {
                var salesPerson = KenticoUserProvider.GetUserByUserId(shoppingCartUser);
                var salesPersonrCartIDs = shoppingCartProvider.GetShoppingCartIDByInventoryType(shoppingCartUser, 2, openCampaignID);
            });
            var distributors = kenticoAddressBookService.GetAddressesByAddressIds(unprocessedDistributorIDs.Select(x => x.Item1).ToList()).Select(x =>
            {
                return new { AddressID = x?.Id, AddressPersonalName = x?.AddressPersonalName };
            }).ToList();
            var listofFailedOrders = unprocessedDistributorIDs.Select(x =>
            {
                var distributor = distributors.Where(y => y.AddressID == x.Item1).FirstOrDefault();
                return new
                {
                    Name = distributor.AddressPersonalName,
                    Reason = x.Item2
                };
            }).ToList();
            var user = KenticoUserProvider.GetUserByUserId(campaignClosingUserID);
            if (user?.Email != null && listofFailedOrders.Count > 0)
            {
                object[,] orderdata = { {"url", $"{SiteContext.CurrentSite.DomainName}{failedOrdersUrl}?campid={openCampaignID}" } ,
                                                             { "failedordercount",listofFailedOrders.Count}           };
                UpdatetFailedOrders(openCampaignID, true);
            }
            if (listofFailedOrders.Count == 0)
            {
                UpdatetFailedOrders(openCampaignID, false);
            }
            return kenticoresourceService.GetResourceString("KDA.OrderSchedular.TaskSuccessfulMessage");
        }

        private void UpdatetFailedOrders(int campaignID, bool isFailed)
        {
            failedOrderStatusProvider.UpdatetFailedOrders(campaignID, isFailed);
        }
    }
}
