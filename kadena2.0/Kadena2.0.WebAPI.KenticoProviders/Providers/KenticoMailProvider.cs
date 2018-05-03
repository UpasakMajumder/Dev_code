using CMS.EmailEngine;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using Kadena.Models;
using Kadena.Models.Membership;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Text;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoMailProvider : IKenticoMailProvider
    {
        private readonly IKenticoResourceService resourceService;
        private readonly IKenticoLogger logger;
        private readonly IKenticoSiteProvider siteProvider;

        public KenticoMailProvider(IKenticoResourceService resourceService, IKenticoLogger logger, IKenticoSiteProvider siteProvider)
        {
            this.resourceService = resourceService ?? throw new ArgumentNullException(nameof(resourceService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.siteProvider = siteProvider ?? throw new ArgumentNullException(nameof(siteProvider));
        }

        public MailTemplate GetMailTemplate(int siteId, string templateName, string cultureCode = null)
        {
            var template = EmailTemplateProvider.GetEmailTemplate(templateName, siteId);

            var localizeToCulture = string.IsNullOrWhiteSpace(cultureCode) ?
                LocalizationContext.CurrentCulture.CultureCode
                : cultureCode;

            return template == null ? null : new MailTemplate()
            {
                From = template.TemplateFrom,
                Cc = template.TemplateCc,
                Bcc = template.TemplateBcc,
                ReplyTo = template.TemplateReplyTo,
                BodyHtml = Convert.ToBase64String(Encoding.UTF8.GetBytes(ResHelper.LocalizeString(template.TemplateText ?? string.Empty, localizeToCulture))),
                BodyPlain = ResHelper.LocalizeString(template.TemplatePlainText, localizeToCulture),
                Subject = ResHelper.LocalizeString(template.TemplateSubject, localizeToCulture)
            };
        }

        public EmailTemplateInfo GetEmailTemplate(string templateName, int siteId)
        {
            return EmailTemplateProvider.GetEmailTemplate(templateName, siteId);
        }

        public void SendRegistrationEmails(User user)
        {
            var ui = UserInfoProvider.GetUserInfo(user.UserId);
            if (ui != null)
            {
                var resolver = MembershipResolvers.GetRegistrationResolver(ui);
                var siteName = siteProvider.GetKenticoSite()?.Name;

                var template = EmailTemplateProvider.GetEmailTemplate("Membership.Registration", siteName);
                var subject = EmailHelper.GetSubject(template, resourceService.GetResourceString("RegistrationForm.RegistrationSubject"));
                var recipients = ui.Email;
                SendMessage(resolver, siteName, template, subject, recipients);

                template = EmailTemplateProvider.GetEmailTemplate("Registration.New", siteName);
                subject = resourceService.GetResourceString("RegistrationForm.EmailSubject");
                recipients = resourceService.GetSiteSettingsKey("CMSAdminEmailAddress");
                SendMessage(resolver, siteName, template, subject, recipients);
            }
        }

        private void SendMessage(CMS.MacroEngine.MacroResolver resolver, string siteName, EmailTemplateInfo template, string subject, string recipients)
        {
            if (template != null)
            {
                var fromAddress = resourceService.GetSiteSettingsKey("CMSNoreplyEmailAddress");
                var message = new EmailMessage
                {
                    EmailFormat = EmailFormatEnum.Default,
                    From = fromAddress,
                    Recipients = recipients,
                    Subject = subject
                };

                try
                {
                    EmailSender.SendEmailWithTemplateText(siteName, message, template, resolver, false);
                }
                catch (Exception ex)
                {
                    logger.LogException(this.GetType().Name, ex);
                }
            }
        }
    }
}
