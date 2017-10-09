using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Models;

namespace Kadena.WebAPI.Services
{
    public class MailTemplateService : IMailTemplateService
    {
        private readonly IKenticoMailProvider kenticoMail;

        public MailTemplateService(IKenticoMailProvider kenticoMail)
        {
            this.kenticoMail = kenticoMail;
        }

        public MailTemplate GetMailTemplate(int siteId, string templateName, string languageCode)
        {
            MailTemplate template = null;

            if (!string.IsNullOrEmpty(languageCode))
            {
                var templateLocalizedName = $"{templateName}.{languageCode}";
                template = kenticoMail.GetMailTemplate(siteId, templateLocalizedName);
            }

            if (template == null)
            {
                template = kenticoMail.GetMailTemplate(siteId, templateName);
            }

            return template;
        }
    }
}
