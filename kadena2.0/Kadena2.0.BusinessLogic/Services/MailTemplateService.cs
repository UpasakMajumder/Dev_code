using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Models;

namespace Kadena.BusinessLogic.Services
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
            return kenticoMail.GetMailTemplate(siteId, templateName, languageCode);
        }
    }
}
