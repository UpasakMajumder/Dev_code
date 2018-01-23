using CMS.DocumentEngine;
using CMS.EmailEngine;
using CMS.EventLog;
using CMS.MacroEngine;
using CMS.Membership;
using CMS.SiteProvider;
using System;
using System.Collections.Generic;

namespace Kadena.Old_App_Code.Kadena.EmailNotifications
{
    public class ProductEmailNotifications
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
        /// Sending the emails based on datasource
        /// </summary>
        /// <param name="campaignName"></param>
        /// <param name="reciepientEmail"></param>
        /// <param name="templateName"></param>
        public static void SendEmail<T>(string templateName, string recipientEmail, IEnumerable<T> emailDataSource)
        {
            try
            {
                var email = EmailTemplateProvider.GetEmailTemplate(templateName, SiteContext.CurrentSite.SiteName);
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
    }
}