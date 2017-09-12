using CMS.EmailEngine;
using Kadena.Models;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Text;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoMailProvider : IKenticoMailProvider
    {
        public MailTemplate GetMailTemplate(string templateName)
        {
            var template = EmailTemplateProvider.GetEmailTemplates().WhereEquals("EmailTemplateName", templateName).FirstObject;

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
