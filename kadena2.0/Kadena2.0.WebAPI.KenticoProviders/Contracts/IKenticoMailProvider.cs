using System.Collections.Generic;
using CMS.EmailEngine;
using CMS.MacroEngine;
using Kadena.Models;
using Kadena.Models.Membership;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoMailProvider
    {
        MailTemplate GetMailTemplate(int siteId, string templateId, string cultureCode = null);

        EmailTemplateInfo GetEmailTemplate(string templateName,int siteId);

        void SendRegistrationEmails(User user);
        void SendKenticoEmail(string[] recipients, IDictionary<string, object> templateData, string emailTemplaceCodeName);
        void SendKenticoEmail(MacroResolver resolver, string siteName, EmailTemplateInfo template, string subject, string recipients);
    }
}