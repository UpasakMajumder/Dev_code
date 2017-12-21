using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Models;
using System;

namespace Kadena.BusinessLogic.Services
{
    public class MailTemplateService : IMailTemplateService
    {
        private readonly IKenticoMailProvider kenticoMail;

        public MailTemplateService(IKenticoMailProvider kenticoMail)
        {
            if (kenticoMail == null)
            {
                throw new ArgumentNullException(nameof(kenticoMail));
            }

            this.kenticoMail = kenticoMail;
        }

        public MailTemplate GetMailTemplate(int siteId, string templateName, string languageCode)
        {
            return kenticoMail.GetMailTemplate(siteId, templateName, languageCode);
        }
    }
}
