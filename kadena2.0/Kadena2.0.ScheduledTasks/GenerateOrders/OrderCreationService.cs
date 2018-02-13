using CMS.Helpers;
using CMS.SiteProvider;
using Kadena.BusinessLogic.Contracts;
using Kadena.ScheduledTasks.Infrastructure;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.Container.Default;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.ScheduledTasks.GenerateOrders
{
    class OrderCreationService : IOrderCreationService
    {
        private IShoppingCartProvider shoppingCartProvider;
        private IAddressBookService kenticoAddressBookService;
        private IKenticoUserProvider KenticoUserProvider;
        private IKenticoResourceService kenticoresourceService;
        private IShoppingCartService shoppingCartService;
        private IFailedOrderStatusProvider failedOrderStatusProvider;
        public OrderCreationService(IShoppingCartProvider shoppingCartProvider, 
            IAddressBookService kenticoAddressBookService, IKenticoUserProvider KenticoUserProvider, 
            IKenticoResourceService kenticoresourceService,
            IShoppingCartService shoppingCartService, IFailedOrderStatusProvider failedOrderStatusProvider)
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
            if (shoppingCartService == null)
            {
                throw new ArgumentNullException(nameof(shoppingCartService));
            }
            if (failedOrderStatusProvider == null)
            {
                throw new ArgumentNullException(nameof(failedOrderStatusProvider));
            }
            this.shoppingCartProvider = shoppingCartProvider;
            this.kenticoAddressBookService = kenticoAddressBookService;
            this.KenticoUserProvider = KenticoUserProvider;
            this.kenticoresourceService = kenticoresourceService;
            this.shoppingCartService = shoppingCartService;
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
                var salesPersonrCartIDs = shoppingCartService.GetLoggedInUserCartData(shoppingCartUser, 2, openCampaignID);
                salesPersonrCartIDs.ForEach(cart =>
                {
                    var shippingCost = default(decimal);
                    var Cart = shoppingCartProvider.GetShoppingCartByID(cart);
                });
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
                object[,] orderdata = { {"url", URLHelper.AddHTTPToUrl($"{SiteContext.CurrentSite.DomainName}{failedOrdersUrl}?campid={openCampaignID}") } ,
                                                             { "failedordercount",listofFailedOrders.Count}           };
                UpdatetFailedOrders(openCampaignID, true);
            }
            if (listofFailedOrders.Count == 0)
            {
                UpdatetFailedOrders(openCampaignID, false);
            }
            return ResHelper.GetString("KDA.OrderSchedular.TaskSuccessfulMessage");
        }

        private void UpdatetFailedOrders(int campaignID, bool isFailed)
        {
            failedOrderStatusProvider.UpdatetFailedOrders(campaignID, isFailed);
        }
    }
}
