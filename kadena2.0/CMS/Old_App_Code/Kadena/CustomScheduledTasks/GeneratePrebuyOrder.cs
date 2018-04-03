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
using CMS.SiteProvider;
using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using Kadena.Container.Default;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.BusinessLogic.Contracts;
using Kadena.Old_App_Code.Kadena.InBoundForm;

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
                var shoppingCartInfo = DIContainer.Resolve<IShoppingCartProvider>();
                var addrerss = DIContainer.Resolve<IAddressBookService>();
                var userInfo = DIContainer.Resolve<IKenticoUserProvider>();
                var kenticoResourceService = DIContainer.Resolve<IKenticoResourceService>();
                var usersWithShoppingCartItems = shoppingCartInfo.GetUserIDsWithShoppingCart(openCampaignID,Convert.ToInt32(ProductsType.PreBuy));
                var orderTemplateSettingKey= kenticoResourceService.GetSettingsKey("KDA_OrderReservationEmailTemplate");
                var failedOrderTemplateSettingKey = kenticoResourceService.GetSettingsKey("KDA_FailedOrdersEmailTemplate");
                var failedOrdersUrl = kenticoResourceService.GetSettingsKey("KDA_FailedOrdersPageUrl");
                var unprocessedDistributorIDs = new List<Tuple<int, string>>();
                usersWithShoppingCartItems.ForEach(shoppingCartUser =>
                {
                    var salesPerson = userInfo.GetUserByUserId(shoppingCartUser);
                    var loggedInUserCartIDs = ShoppingCartHelper.GetCartsByUserID(shoppingCartUser, ProductType.PreBuy,openCampaignID);
                    loggedInUserCartIDs.ForEach(cart =>
                    {
                        var shippingCost = default(decimal);
                        Cart = ShoppingCartInfoProvider.GetShoppingCartInfo(cart);
                        ShippingOptionInfo shippingOption = ShippingOptionInfoProvider.GetShippingOptionInfo(Cart.ShoppingCartShippingOptionID);
                        if (shippingOption == null)
                        {
                            shippingOption = ShippingOptionInfoProvider.GetShippingOptionInfo(kenticoResourceService.GetSettingsKey(SiteContext.CurrentSiteID, "KDA_DefaultShipppingOption"), SiteContext.CurrentSiteName);
                            if (shippingOption == null)
                            {
                                Cart.ShoppingCartShippingOptionID = shippingOption.ShippingOptionID;
                                ShoppingCartInfoProvider.SetShoppingCartInfo(Cart);
                            }
                        }
                        OrderDTO ordersDTO = ShoppingCartHelper.CreateOrdersDTO(Cart, Cart.ShoppingCartUserID, OrderType.prebuy, shippingCost);
                        var response = ShoppingCartHelper.ProcessOrder(Cart, Cart.ShoppingCartUserID, OrderType.prebuy, ordersDTO, shippingCost);
                        if (response != null && response.Success)
                        {
                            ordersDTO.OrderID = response.Payload;
                            ProductEmailNotifications.SendMail(salesPerson, ordersDTO, orderTemplateSettingKey);
                            InBoundFormHelper.InsertIBFForm(ordersDTO);
                            ShoppingCartInfoProvider.DeleteShoppingCartInfo(Cart);
                            ShoppingCartHelper.UpdateRemainingBudget(ordersDTO, salesPerson.UserId);
                            DIContainer.Resolve<IIBTFService>().InsertIBTFAdjustmentRecord(ordersDTO);
                        }
                        else
                        {
                            unprocessedDistributorIDs.Add(new Tuple<int, string>(Cart.GetIntegerValue("ShoppingCartDistributorID", default(int)), response.ErrorMessages));
                            Cart.ShoppingCartCustomData["FailedReason"] = response.ErrorMessages;
                            ShoppingCartInfoProvider.SetShoppingCartInfo(Cart);
                        }
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
                    Dictionary<string, object> failedOrderData = new Dictionary<string, object>();
                    failedOrderData.Add("failedorderurl", URLHelper.AddHTTPToUrl($"{SiteContext.CurrentSite.DomainName}{failedOrdersUrl}?campid={openCampaignID}"));
                    failedOrderData.Add("failedordercount", listofFailedOrders.Count);
                    failedOrderData.Add("failedorders", listofFailedOrders);
                    ProductEmailNotifications.SendEmailNotification(failedOrderTemplateSettingKey, user.Email, listofFailedOrders, "failedOrders",failedOrderData);
                    UpdatetFailedOrders(openCampaignID, true);
                }
                return ResHelper.GetString("KDA.OrderSchedular.TaskSuccessfulMessage");
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("GeneratePrebuyOrderTask", "GenerateOrder", ex, SiteContext.CurrentSiteID, ex.Message);
                return ex.Message;
            }
        }
        private void UpdatetFailedOrders(int campaignID, bool isFailed)
        {
            var failedOrderStatusProvider = DIContainer.Resolve<IFailedOrderStatusProvider>();
            failedOrderStatusProvider.UpdatetFailedOrders(campaignID, isFailed);
        }
    }
}