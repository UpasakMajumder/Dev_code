using System.Collections.Generic;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoDocumentProvider
    {
        string GetDocumentUrl(int documentId);

        List<string> GetBreadcrumbs(int documentId);

        string GetDocumentUrl(string aliasPath);

        string GetDocumentAbsoluteUrl(string aliasPath);

        string GetDocumentUrl(string aliasPath, string cultureCode, bool absoluteUrl = false);
    }
}
