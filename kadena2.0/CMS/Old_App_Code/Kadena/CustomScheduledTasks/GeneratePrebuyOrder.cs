using CMS.Ecommerce;
using CMS.EventLog;
using CMS.Scheduler;
using Kadena.Old_App_Code.Kadena.Constants;
using Kadena.Old_App_Code.Kadena.EmailNotifications;
using Kadena.Old_App_Code.Kadena.Enums;
using Kadena.Old_App_Code.Kadena.Shoppingcart;
using System;
using System.Collections.Generic;
using System.Linq;
using CMS.Helpers;
using CMS.Membership;
using CMS.SiteProvider;

namespace Kadena.Old_App_Code.Kadena.CustomScheduledTasks
{
    public class GeneratePrebuyOrder : ITask
    {
        private ShoppingCartInfo Cart { get; set; }
        /// <summary>
        /// Executes the task for generationg ordeers.
        /// </summary>
        /// <param name="ti">Info object representing the scheduled task</param>
        public string Execute(TaskInfo taskInfo)
        {
            try
            {
                var taskData = taskInfo.TaskData.Split('|');
                var openCampaign = ValidationHelper.GetInteger(taskData[0], 0);
                var campaignClosingUserID = ValidationHelper.GetInteger(taskData[1], 0);
                var orderResponse = GenerateOrder(openCampaign, campaignClosingUserID);
                return orderResponse;
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("GeneratePrebuyOrderTask", "Execute", ex, SiteContext.CurrentSiteID, ex.Message);
                return ex.Message;
            }
        }
        /// <summary>
        /// This method  will do order processing
        /// </summary>
        /// <param name="openCampaignID"></param>
        /// <param name="campaignClosingUserID"></param>
        /// <returns></returns>
        private string GenerateOrder(int openCampaignID, int campaignClosingUserID)
        {
            try
            {
                var usersWithShoppingCartItems = ShoppingCartInfoProvider.GetShoppingCarts().WhereEquals("ShoppingCartCampaignID", openCampaignID)
                                                                   .WhereEquals("ShoppingCartInventoryType", ProductType.PreBuy).ToList().Select(x => x.ShoppingCartUserID).Distinct().ToList();
                var unprocessedDistributorIDs = new List<Tuple<int, string>>();
                usersWithShoppingCartItems.ForEach(shoppingCartUser =>
                {
                    var loggedInUserCartIDs = ShoppingCartHelper.GetCartsByUserID(shoppingCartUser, ProductType.PreBuy);
                    loggedInUserCartIDs.ForEach(cart =>
                    {
                        Cart = ShoppingCartInfoProvider.GetShoppingCartInfo(cart);
                        var response = ShoppingCartHelper.ProcessOrder(Cart, Cart.ShoppingCartUserID, OrderType.prebuy);
                        if (response != null && response.Success)
                        {
                            ShoppingCartInfoProvider.DeleteShoppingCartInfo(Cart);
                        }
                        else
                        {
                            unprocessedDistributorIDs.Add(new Tuple<int, string>(Cart.GetIntegerValue("ShoppingCartDistributorID", default(int)), response.ErrorMessages));
                        }
                    });
                });
                var distributors = AddressInfoProvider.GetAddresses().WhereIn("AddressID", unprocessedDistributorIDs.Select(x => x.Item1).ToList())
                       .Columns("AddressPersonalName,AddressID").ToList().Select(x =>
                       {
                           return new { AddressID = x?.AddressID, AddressPersonalName = x?.AddressPersonalName };
                       }).ToList();
                var list = unprocessedDistributorIDs.Select(x =>
                  {
                      var distributor = distributors.Where(y => y.AddressID == x.Item1).FirstOrDefault();
                      return new
                      {
                          AddressPersonalName = distributor.AddressPersonalName,
                          Reason = x.Item2
                      };
                  }).ToList();
                var user = UserInfoProvider.GetUsers().WhereEquals("UserID", campaignClosingUserID).Column("Email").FirstOrDefault();
                if (user?.Email != null)
                    ProductEmailNotifications.SendEmail(EmailTemplate.PrebuyOrderStatusTemplate, user?.Email, list);
                return ResHelper.GetString("KDA.OrderSchedular.TaskSuccessfulMessage");
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("GeneratePrebuyOrderTask", "GenerateOrder", ex, SiteContext.CurrentSiteID, ex.Message);
                return ex.Message;
            }
        }
    }
}