using Kadena.Models;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IMailTemplateService
    {
        MailTemplate GetMailTemplate(int siteid ,string templateName, string languageCode);
    }
}