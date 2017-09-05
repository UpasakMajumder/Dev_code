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

        public MailTemplate GetMailTemplate(string templateId)
        {
            return kenticoMail.GetMailTemplate(templateId);
        }
    }
}
