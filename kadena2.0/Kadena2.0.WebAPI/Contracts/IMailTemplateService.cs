using Kadena.Models;

namespace Kadena.WebAPI.Contracts
{
    public interface IMailTemplateService
    {
        MailTemplate GetMailTemplate(string templateName);
    }
}