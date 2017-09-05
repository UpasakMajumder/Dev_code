using CMS.EmailEngine;
using Kadena.Models;
using Kadena.WebAPI.KenticoProviders.Contracts;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoMailProvider : IKenticoMailProvider
    {
        public MailTemplate GetMailTemplate(string templateId)
        {
            var template = EmailTemplateProvider.GetEmailTemplates().WhereEquals("EmailTemplateName", templateId).FirstObject;

            return template == null ? null : new MailTemplate()
            {
                From = template.TemplateFrom,
                BodyHtml = template.TemplateText,
                BodyPlain = template.TemplatePlainText,
                Subject = template.TemplateSubject
            };
        }
    }
}
