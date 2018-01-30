using CMS.EmailEngine;
using Kadena.Models;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoMailProvider
    {
        MailTemplate GetMailTemplate(int siteId, string templateId, string cultureCode = null);

        EmailTemplateInfo GetEmailTemplate(string templateName,int siteId);
    }
}