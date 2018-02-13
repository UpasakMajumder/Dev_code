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
        public string GenerateOrder(int openCampaignID, int campaignClosingUserID)
        {
            var shoppingCartInfo = DIContainer.Resolve<IShoppingCartProvider>();
            var addrerss = DIContainer.Resolve<IAddressBookService>();
            var userInfo = DIContainer.Resolve<IKenticoUserProvider>();
            var settingInfo = DIContainer.Resolve<IKenticoResourceService>();
            var shoppingCarts = DIContainer.Resolve<IShoppingCartService>();
            var usersWithShoppingCartItems = shoppingCartInfo.GetUserIDsWithShoppingCart(openCampaignID, 1);
            var orderTemplateSettingKey = settingInfo.GetSettingsKey("KDA_OrderReservationEmailTemplate");
            var failedOrderTemplateSettingKey = settingInfo.GetSettingsKey("KDA_FailedOrdersEmailTemplate");
            var failedOrdersUrl = settingInfo.GetSettingsKey("KDA_FailedOrdersPageUrl");
            var unprocessedDistributorIDs = new List<Tuple<int, string>>();
            usersWithShoppingCartItems.ForEach(shoppingCartUser =>
            {
                var salesPerson = userInfo.GetUserByUserId(shoppingCartUser);
                var salesPersonrCartIDs = shoppingCarts.GetLoggedInUserCartData(shoppingCartUser, 2, openCampaignID);
                salesPersonrCartIDs.ForEach(cart =>
                {
                    var shippingCost = default(decimal);
                    var Cart = shoppingCartInfo.GetShoppingCartByID(cart);
                });
            });
            var distributors = addrerss.GetAddressesByAddressIds(unprocessedDistributorIDs.Select(x => x.Item1).ToList()).Select(x =>
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
            var user = userInfo.GetUserByUserId(campaignClosingUserID);
            if (user?.Email != null && listofFailedOrders.Count > 0)
            {
                object[,] orderdata = { {"url", URLHelper.AddHTTPToUrl($"{SiteContext.CurrentSite.DomainName}{failedOrdersUrl}?campid={openCampaignID}") } ,
                                                             { "failedordercount",listofFailedOrders.Count}           };
                UpdatetFailedOrders(openCampaignID, true);
            }
            return ResHelper.GetString("KDA.OrderSchedular.TaskSuccessfulMessage");
        }

        private void UpdatetFailedOrders(int campaignID, bool IsFailed)
        {
            var failedOrderStatus = DIContainer.Resolve<IFailedOrderStatusProvider>();
            failedOrderStatus.UpdatetFailedOrders(campaignID, true);
        }
    }
}
