﻿using System;
using System.Collections.Generic;
using System.Text;
using CMS.EmailEngine;
using CMS.Helpers;
using CMS.Localization;
using CMS.MacroEngine;
using CMS.Membership;
using Kadena.Models;
using Kadena.Models.Membership;
using Kadena.Models.Product;
using Kadena.Models.SiteSettings;
using Kadena.WebAPI.KenticoProviders.Contracts;

namespace Kadena.WebAPI.KenticoProviders.Providers
{
    public class KenticoMailProvider : IKenticoMailProvider
    {
        private readonly IKenticoResourceService _resourceService;
        private readonly IKenticoLogger _logger;
        private readonly IKenticoSiteProvider _siteProvider;

        public KenticoMailProvider(IKenticoResourceService resourceService, IKenticoLogger logger, IKenticoSiteProvider siteProvider)
        {
            _resourceService = resourceService ?? throw new ArgumentNullException(nameof(resourceService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _siteProvider = siteProvider ?? throw new ArgumentNullException(nameof(siteProvider));
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
                var siteName = _siteProvider.GetKenticoSite()?.Name;

                var template = EmailTemplateProvider.GetEmailTemplate("Membership.Registration", siteName);
                var subject = EmailHelper.GetSubject(template, _resourceService.GetResourceString("RegistrationForm.RegistrationSubject"));
                var recipients = ui.Email;
                SendMessage(resolver, siteName, template, subject, recipients);

                template = EmailTemplateProvider.GetEmailTemplate("Registration.New", siteName);
                subject = _resourceService.GetResourceString("RegistrationForm.EmailSubject");
                recipients = _resourceService.GetSiteSettingsKey("CMSAdminEmailAddress");
                SendMessage(resolver, siteName, template, subject, recipients);
            }
        }

        public void SendNewProductNotification(IEnumerable<Customer> customers, Sku sku, ProductLinkWithDescription product, Price price)
        {
            try
            {
                var currentSite = _siteProvider.GetKenticoSite();

                var emailTemplateCodeName =
                    _resourceService.GetSiteSettingsKey(Settings.KDA_NewProductEmailNotificationTemplate);

                var emailTemplate = EmailTemplateProvider.GetEmailTemplate(emailTemplateCodeName, currentSite.Id);

                var macroData = new Dictionary<string, object>
                {
                    { "productName", product.Title },
                    { "description", product.Description },
                    { "price", price.Value },
                    { "thumbnail", product.ImageUrl },
                };

                var resolver = MacroResolver.GetInstance();
                resolver.SetNamedSourceData(macroData);

                var email = new EmailMessage
                {
                    From = resolver.ResolveMacros(emailTemplate.TemplateFrom),
                    EmailFormat = EmailFormatEnum.Default,
                    ReplyTo = resolver.ResolveMacros(emailTemplate.TemplateReplyTo),
                    Subject = resolver.ResolveMacros(emailTemplate.TemplateSubject),
                    Body = resolver.ResolveMacros(emailTemplate.TemplateText)
                };

                foreach (var customer in customers)
                {
                    email.Recipients = customer.Email;
                    EmailSender.SendEmail(currentSite.Name, email, true);
                }
            }
            catch (Exception e)
            {
                _logger.LogException(GetType().Name, e);
            }
        }

        private void SendMessage(CMS.MacroEngine.MacroResolver resolver, string siteName, EmailTemplateInfo template, string subject, string recipients)
        {
            if (template != null)
            {
                var fromAddress = _resourceService.GetSiteSettingsKey("CMSNoreplyEmailAddress");
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
                    _logger.LogException(GetType().Name, ex);
                }
            }
        }
    }
}
