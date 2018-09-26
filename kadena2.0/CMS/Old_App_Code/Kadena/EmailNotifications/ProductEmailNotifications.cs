using CMS.DocumentEngine;
using CMS.EmailEngine;
using CMS.EventLog;
using CMS.MacroEngine;
using CMS.Membership;
using CMS.SiteProvider;
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
using CMS.Helpers;
using Kadena.Models.CampaignData;

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
                var response = orderViewClient.GetOrders(SiteContext.CurrentSiteName, 1, 1000, campaignID, OrderType.prebuy).Result;
                if (response.Success && response.Payload.TotalCount != 0)
                {
                    var customerProvider = DIContainer.Resolve<IKenticoCustomerProvider>();
                    var userProvider = DIContainer.Resolve<IKenticoUserProvider>();

                    var customerOrderData = response.Payload.Orders.GroupBy(x => x.ClientId,
                        (key, value) => new
                        {
                            CustomerId = key,
                            User = userProvider.GetUserByUserId(customerProvider.GetCustomer(key)?.UserID ?? 0),
                            Orders = value.Select(ord =>
                            {
                                var skus = SKUInfoProvider.GetSKUs()
                                    .WhereIn(nameof(SKUInfo.SKUNumber), ord.Items.Select(i => i.SKUNumber).ToList())
                                    .Columns("SKUProductCustomerReferenceNumber", nameof(SKUInfo.SKUEnabled), nameof(SKUInfo.SKUNumber));
                                return new
                                {
                                    ord.Id,
                                    ord.TotalCost,
                                    ord.ShippingDate,
                                    CampaignId = ord.Campaign.ID,
                                    Items = ord.Items.GroupJoin(skus, i => i.SKUNumber, s => s.SKUNumber, (i, sks) => new
                                    {
                                        i.SKUNumber,
                                        i.Name,
										i.SKUProductCustomerReferenceNumber,
                                        i.Quantity,
                                        Price = i.UnitPrice,
                                        PosNumber = GetPosNum(sks.DefaultIfEmpty().FirstOrDefault()),
                                        Status = GetStatus(sks.DefaultIfEmpty().FirstOrDefault())
                                    })
                                };
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
                                    { "totalprice", o.TotalCost },
                                    { "shippingdate", o.ShippingDate },
                                    { "campaignid", o.CampaignId },
                                    { "orderNumber", o.Id }
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
                var skus = SKUInfoProvider.GetSKUs()
                    .WhereIn(nameof(SKUInfo.SKUID), ordersDTO.Items.Select(i => i.SKU.KenticoSKUID).ToList())
                    .Columns("SKUProductCustomerReferenceNumber", nameof(SKUInfo.SKUEnabled), nameof(SKUInfo.SKUID));
                var cartItems = ordersDTO.Items.GroupJoin(skus, i => i.SKU.KenticoSKUID, s => s.SKUID, (item, sks) =>
                    {
                        return new
                        {
                            SKUNumber = item.SKU.SKUNumber,
                            Name = item.SKU.Name,
                            Quantity = item.UnitCount,
                            Price = item.TotalPrice,
                            PosNumber = GetPosNum(sks.DefaultIfEmpty().FirstOrDefault()),
                            Status = GetStatus(sks.DefaultIfEmpty().FirstOrDefault()),
                        };
                    }).ToList();
                var orderDetails = new Dictionary<string, object>
                {
                    { "name", user.FirstName },
                    { "totalprice", ordersDTO.Totals.Price },
                    { "totalshipping", ordersDTO.Totals.Shipping },
                    { "campaignid", ordersDTO.Totals.Shipping },
                    { "orderNumber", ordersDTO.OrderID }
                };
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

        private static string GetPosNum(SKUInfo sku)
        {
            return sku != null ?
                sku.GetValue("SKUProductCustomerReferenceNumber", string.Empty)
                : string.Empty;
        }

        private static string GetStatus(SKUInfo sku)
        {
            return (sku?.SKUEnabled ?? false) ?
                ResHelper.GetString("KDA.Common.Status.Active")
                : ResHelper.GetString("KDA.Common.Status.Inactive");
        }
    }
}