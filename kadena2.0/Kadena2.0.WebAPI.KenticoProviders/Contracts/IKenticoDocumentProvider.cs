using System;
using System.Collections.Generic;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoDocumentProvider
    {
        string GetDocumentUrl(int documentId);

        List<string> GetBreadcrumbs(int documentId);

        string GetDocumentUrl(string aliasPath, bool absoluteUrl = false);

        string GetDocumentAbsoluteUrl(string aliasPath);

        string GetDocumentUrl(string aliasPath, string cultureCode, bool absoluteUrl = false);

        string GetDocumentUrl(Guid documentGUID);

        DateTime GetTaCValidFrom();
    }
}
