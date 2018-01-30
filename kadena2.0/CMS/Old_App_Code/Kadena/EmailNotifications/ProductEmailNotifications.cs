using CMS.DocumentEngine;
using CMS.EmailEngine;
using CMS.EventLog;
using CMS.MacroEngine;
using CMS.Membership;
using CMS.SiteProvider;
using Kadena.Dto.General;
using Kadena.Dto.Order;
using Kadena.Models;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.Container.Default;
using Kadena2.MicroserviceClients.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public static void CampaignEmail(string campaignName, string recipientEmail, string templateName)
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
        ///    Send emails to all sales persons
        /// </summary>
        /// <typeparam name="T">object type</typeparam>
        /// <param name="orderDetails"></param>
        /// <param name="templateName">email template name</param>
        /// <param name="customer">user details</param>
        public static void SendEmailNotification<T>(T orderDetails, string templateName, User customer)
        {
            try
            {
                var email = DIContainer.Resolve<IKenticoMailProvider>().GetEmailTemplate(templateName, SiteContext.CurrentSiteID);
                EmailMessage msg = new EmailMessage();
                if (email != null)
                {
                    MacroResolver resolver = MacroResolver.GetInstance();
                    resolver.SetAnonymousSourceData(orderDetails);
                    resolver.SetAnonymousSourceData(customer);
                    msg.From = resolver.ResolveMacros(email.TemplateFrom);
                    msg.Recipients = customer.Email;
                    msg.EmailFormat = EmailFormatEnum.Default;
                    msg.ReplyTo = resolver.ResolveMacros(email.TemplateReplyTo);
                    msg.Subject = resolver.ResolveMacros(email.TemplateSubject);
                    msg.Body = resolver.ResolveMacros(email.TemplateText);
                    EmailSender.SendEmail(SiteContext.CurrentSite.SiteName, msg, true);
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("ProductEmailNotifications", "SendEmailNotification", ex, SiteContext.CurrentSite.SiteID, ex.Message);
            }
        }
        /// <summary>
        /// Sending the emails based on datasource
        /// </summary>
        /// <param name="campaignName"></param>
        /// <param name="reciepientEmail"></param>
        /// <param name="templateName"></param>
        public static void SendEmail<T>(string templateName, string recipientEmail, IEnumerable<T> emailDataSource)
        {
            try
            {
                var email = DIContainer.Resolve<IKenticoMailProvider>().GetEmailTemplate(templateName, SiteContext.CurrentSiteID);
                EmailMessage msg = new EmailMessage();
                if (email != null)
                {
                    MacroResolver resolver = MacroResolver.GetInstance();
                    resolver.SetNamedSourceData("data", emailDataSource);
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
                var orderType = Constants.OrderType.prebuy;
                var client = DIContainer.Resolve<IOrderViewClient>();
                BaseResponseDto<OrderListDto> response = client.GetOrders(SiteContext.CurrentSiteName, 1, 1000, campaignID, orderType).Result;
                if (response.Success && response.Payload.TotalCount != 0)
                {
                    var responseData = response.Payload.Orders.ToList();
                    var customerOrderData = responseData.GroupBy(x => x.CustomerId).ToList();
                    customerOrderData.ForEach(x =>
                    {
                        var customerData = DIContainer.Resolve<IKenticoUserProvider>().GetUserByUserId(x.Key);
                        if (customerData != null)
                        {
                            x.ToList().ForEach(o =>
                        {
                            SendEmailNotification(o, templateName, customerData);
                        });
                        }
                    });
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("ProductEmailNotifications", "GetCampaignOrders", ex, SiteContext.CurrentSite.SiteID, ex.Message);
            }
            return false;
        }
    }
}