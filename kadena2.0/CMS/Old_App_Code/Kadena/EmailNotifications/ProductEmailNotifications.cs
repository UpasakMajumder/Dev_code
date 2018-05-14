using CMS.DocumentEngine;
using CMS.EmailEngine;
using CMS.EventLog;
using CMS.MacroEngine;
using CMS.Membership;
using CMS.SiteProvider;
using Kadena.Dto.General;
using Kadena.Dto.Order;
using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Container.Default;
using Kadena2.MicroserviceClients.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using CMS.Ecommerce;
using Kadena.Models.Membership;

namespace Kadena.Old_App_Code.Kadena.EmailNotifications
{
    public static class ProductEmailNotifications
    {
        /// <summary>
        /// Sending the emails to users for campaign open/close
        /// </summary>
        /// <param name="campaignName"></param>
        /// <param name="reciepientEmail"></param>
        /// <param name="templateName"></param>
        public static void CampaignEmail(string campaignName, string recipientEmail, string templateName, string programName = "", string campaignURL = "")
        {
            try
            {
                TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                var email = EmailTemplateProvider.GetEmailTemplate(templateName, SiteContext.CurrentSite.SiteName);
                EmailMessage msg = new EmailMessage();
                if (email != null)
                {
                    MacroResolver resolver = MacroResolver.GetInstance();
                    resolver.SetNamedSourceData("CampaignName", campaignName);
                    resolver.SetNamedSourceData("programName", programName);
                    resolver.SetNamedSourceData("CampaignNameURL", campaignURL);
                    msg.From = resolver.ResolveMacros(email.TemplateFrom);
                    msg.Recipients = recipientEmail;
                    msg.EmailFormat = EmailFormatEnum.Default;
                    msg.ReplyTo = resolver.ResolveMacros(email.TemplateReplyTo);
                    msg.Subject = resolver.ResolveMacros(email.TemplateSubject);
                    msg.Body = resolver.ResolveMacros(email.TemplateText);
                    EmailSender.SendEmail(SiteContext.CurrentSite.SiteName, msg, true);
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("ProductEmailNotifications", "CampaignEmail", ex, SiteContext.CurrentSite.SiteID, ex.Message);
            }
        }
        /// <summary>
        /// Sending the emails based on datasource
        /// </summary>
        /// <param name="campaignName"></param>
        /// <param name="reciepientEmail"></param>
        /// <param name="templateName"></param>
        public static void SendEmailNotification<T>(string templateName, string recipientEmail, IEnumerable<T> emailDataSource, string dataSourceName, Dictionary<string, object> macroData = null)
        {
            try
            {
                var email = DIContainer.Resolve<IKenticoMailProvider>().GetEmailTemplate(templateName, SiteContext.CurrentSiteID);
                var itemTable = ConvertToDataTable(emailDataSource.ToList());
                EmailMessage msg = new EmailMessage();
                if (email != null)
                {
                    MacroResolver resolver = MacroResolver.GetInstance();
                    resolver.SetNamedSourceData(dataSourceName, itemTable.Rows);
                    resolver.SetNamedSourceData(macroData);
                    msg.From = resolver.ResolveMacros(email.TemplateFrom);
                    msg.Recipients = recipientEmail;
                    msg.EmailFormat = EmailFormatEnum.Default;
                    msg.ReplyTo = resolver.ResolveMacros(email.TemplateReplyTo);
                    msg.Subject = resolver.ResolveMacros(email.TemplateSubject);
                    msg.Body = resolver.ResolveMacros(email.TemplateText);
                    EmailSender.SendEmail(SiteContext.CurrentSite.SiteName, msg, true);
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("ProductEmailNotifications", "SendEmail", ex, SiteContext.CurrentSite.SiteID, ex.Message);
            }
        }

        /// <summary>
        ///   Gets all the prebuy orders under particular campaign
        /// </summary>
        /// <param name="campaignID">campaign id</param>
        /// <returns>true/false</returns>
        public static bool GetCampaignOrders(int campaignID, string templateName)
        {
            try
            {
                var orderViewClient = DIContainer.Resolve<IOrderViewClient>();
                var response = orderViewClient.GetOrders(SiteContext.CurrentSiteName, 1, 1000, campaignID, Constants.OrderType.prebuy).Result;
                if (response.Success && response.Payload.TotalCount != 0)
                {
                    var customerProvider = DIContainer.Resolve<IKenticoCustomerProvider>();
                    var userProvider = DIContainer.Resolve<IKenticoUserProvider>();

                    var customerOrderData = response.Payload.Orders.GroupBy(x => x.CustomerId,
                        (key, value) => new
                        {
                            CustomerId = key,
                            User = userProvider.GetUserByUserId(customerProvider.GetCustomer(key)?.UserID ?? 0),
                            Orders = value.Select(ord => new
                            {
                                ord.TotalPrice,
                                ord.ShippingDate,
                                CampaignId = ord.Campaign.ID,
                                Items = ord.Items.Select(i => new
                                {
                                    i.SKUNumber,
                                    i.Name,
                                    i.Quantity,
                                    Price = i.UnitPrice,
                                    PosNumber = GetPosNum(i.SKUNumber)
                                })
                            }).ToList()
                        })
                        .ToList();
                    customerOrderData.ForEach(x =>
                    {
                        if (x.User != null)
                        {
                            x.Orders.ForEach(o =>
                            {
                                var orderDetails = new Dictionary<string, object>
                                {
                                    { "name", x.User.FirstName },
                                    { "totalprice", o.TotalPrice },
                                    { "shippingdate", o.ShippingDate },
                                    { "campaignid", o.CampaignId }
                                };
                                SendEmailNotification(templateName, x.User.Email, o.Items, "orderitems", orderDetails);
                            });
                        }
                    });
                    return true;
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("ProductEmailNotifications", "GetCampaignOrders", ex, SiteContext.CurrentSite.SiteID, ex.Message);
            }
            return false;
        }
        public static void SendMail(User user, OrderDTO ordersDTO, string orderTemplateSettingKey)
        {
            if (user?.Email != null)
            {
                var cartItems = ordersDTO.Items.Select(item =>
                {
                    return new
                    {
                        SKUNumber = item.SKU.SKUNumber,
                        Name = item.SKU.Name,
                        Quantity = item.UnitCount,
                        Price = item.TotalPrice,
                        PosNumber = GetPosNum(item.SKU.KenticoSKUID)
                    };
                }).ToList();
                Dictionary<string, object> orderDetails = new Dictionary<string, object>();
                orderDetails.Add("name", user.FirstName);
                orderDetails.Add("totalprice", ordersDTO.Totals.Price);
                orderDetails.Add("totalshipping", ordersDTO.Totals.Shipping);
                orderDetails.Add("campaignid", ordersDTO.Totals.Shipping);
                SendEmailNotification(orderTemplateSettingKey, user.Email, cartItems, "orderitems", orderDetails);
            }
        }
        public static DataTable ConvertToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public static string GetPosNum(int SkuId)
        {
            var skuData = SKUInfoProvider.GetSKUs()
                .WhereEquals("SKUID", SkuId)
                .Columns("SKUProductCustomerReferenceNumber")
                .FirstOrDefault();
            return skuData != null ? skuData.GetValue("SKUProductCustomerReferenceNumber", string.Empty) : string.Empty;
        }
        public static string GetPosNum(string skuNumber)
        {
            var skuData = SKUInfoProvider.GetSKUs()
                .WhereEquals(nameof(SKUInfo.SKUNumber), skuNumber)
                .Columns("SKUProductCustomerReferenceNumber")
                .FirstOrDefault();
            return skuData != null ? skuData.GetValue("SKUProductCustomerReferenceNumber", string.Empty) : string.Empty;
        }

    }
}