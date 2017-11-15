using Kadena.Models;

namespace Kadena.WebAPI.Contracts
{
    public interface IMailTemplateService
    {
        MailTemplate GetMailTemplate(int siteid ,string templateName, string languageCode);
    }
}