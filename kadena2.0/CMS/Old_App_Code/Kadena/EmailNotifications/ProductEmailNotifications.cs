using CMS.DocumentEngine;
using CMS.EmailEngine;
using CMS.EventLog;
using CMS.MacroEngine;
using CMS.Membership;
using CMS.SiteProvider;
using Kadena.Dto.General;
using Kadena.Dto.Order;
using Kadena2.Container.Default;
using Kadena2.MicroserviceClients.Contracts;
using System;
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

        public static void SendEmailNotification(OrderDto orderDetails, string templateName, UserInfo customer)
        {
            var email = EmailTemplateProvider.GetEmailTemplate(templateName, SiteContext.CurrentSite.SiteName);
            EmailMessage msg = new EmailMessage();
            if (email != null)
            {
                MacroResolver resolver = MacroResolver.GetInstance();
                resolver.SetAnonymousSourceData(orderDetails);
                msg.From = resolver.ResolveMacros(email.TemplateFrom);
                msg.Recipients = customer.Email;
                msg.EmailFormat = EmailFormatEnum.Default;
                msg.ReplyTo = resolver.ResolveMacros(email.TemplateReplyTo);
                msg.Subject = resolver.ResolveMacros(email.TemplateSubject);
                msg.Body = resolver.ResolveMacros(email.TemplateText);
                EmailSender.SendEmail(SiteContext.CurrentSite.SiteName, msg, true);
            }
        }

        public static bool GetCampaignOrders(int campaignID)
        {
            try
            {
                var orderType = "prebuy";
                var templateName = "";
                var client = DIContainer.Resolve<IOrderViewClient>();
                BaseResponseDto<OrderListDto> response = client.GetOrders(SiteContext.CurrentSiteName, 1, 1000, campaignID, orderType).Result;
                if (response.Success && response.Payload.TotalCount != 0)
                {
                    var responseData = response.Payload.Orders.ToList();
                    var customerOrderData = responseData.GroupBy(x => x.CustomerId).ToList();
                    customerOrderData.ForEach(x =>
                    {
                        var cutomerData = UserInfoProvider.GetUserInfo(x.Key);
                        if (cutomerData != null)
                        {
                            x.ToList().ForEach(o =>
                        {
                            SendEmailNotification(o, templateName, cutomerData);
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