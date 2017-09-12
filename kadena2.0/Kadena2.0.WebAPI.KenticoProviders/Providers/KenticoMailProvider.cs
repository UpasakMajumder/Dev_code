using CMS.EmailEngine;
using Kadena.Models;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Linq;
using System.Text;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoMailProvider : IKenticoMailProvider
    {
        public MailTemplate GetMailTemplate(int siteId, string templateName)
        {
            var templates = EmailTemplateProvider.GetEmailTemplates().WhereEquals("EmailTemplateName", templateName).ToArray();
            var template = templates.Where(t => t.TemplateSiteID == siteId || t.TemplateSiteID == 0).FirstOrDefault();

            return template == null ? null : new MailTemplate()
            {
                From = template.TemplateFrom,
                BodyHtml = Convert.ToBase64String( Encoding.UTF8.GetBytes(template.TemplateText ?? string.Empty)),
                BodyPlain = template.TemplatePlainText,
                Subject = template.TemplateSubject
            };
        }
    }
}
