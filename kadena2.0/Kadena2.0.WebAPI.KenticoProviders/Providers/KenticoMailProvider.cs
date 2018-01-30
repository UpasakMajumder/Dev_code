using CMS.EmailEngine;
using CMS.Helpers;
using CMS.Localization;
using Kadena.Models;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Text;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoMailProvider : IKenticoMailProvider
    {
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
    }
}
