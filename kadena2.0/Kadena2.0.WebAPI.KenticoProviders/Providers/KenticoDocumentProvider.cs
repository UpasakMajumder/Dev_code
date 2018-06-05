using AutoMapper;
using CMS.DocumentEngine;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.WebAPI.KenticoProviders.Contracts.KadenaSettings;
using System;
using System.Collections.Generic;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoDocumentProvider : IKenticoDocumentProvider
    {
        private readonly IKenticoLogger logger;
        private readonly IKadenaSettings kadenaSettings;

        public KenticoDocumentProvider(IKenticoLogger logger, IKadenaSettings kadenaSettings)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.kadenaSettings = kadenaSettings ?? throw new ArgumentNullException(nameof(kadenaSettings));
        }

        /// <summary>
        /// Gets document URL with respect to current culture
        /// </summary>
        public string GetDocumentUrl(string aliasPath, bool absoluteUrl = false)
        {
            return GetDocumentUrl(aliasPath, LocalizationContext.CurrentCulture.CultureCode, absoluteUrl);
        }

        public string GetDocumentAbsoluteUrl(string aliasPath)
        {
            return GetDocumentUrl(aliasPath, LocalizationContext.CurrentCulture.CultureCode, true);
        }

        public string GetDocumentUrl(string aliasPath, string cultureCode, bool absoluteUrl = false)
        {
            var document = DocumentHelper.GetDocument(
                new NodeSelectionParameters
                {
                    AliasPath = aliasPath,
                    SiteName = SiteContext.CurrentSiteName,
                    CultureCode = cultureCode,
                    CombineWithDefaultCulture = false
                },
                new TreeProvider(MembershipContext.AuthenticatedUser)
            );
            if (document == null)
            {
                logger.LogInfo("GetDocumentUrl", "INFORMATION", $"Document not found for alias '{aliasPath}' and culture '{cultureCode}'");
                return "/";
            }

            return absoluteUrl ? document.AbsoluteURL : document.DocumentUrlPath;
        }

        public string GetDocumentUrl(Guid documentGUID)
        {
            TreeNode document = new TreeProvider().SelectSingleNode(documentGUID, LocalizationContext.CurrentCulture.CultureCode, SiteContext.CurrentSiteName);
            if (document == null)
            {
                logger.LogInfo("GetDocumentUrl", "INFORMATION", $"Document not found for document guid '{documentGUID.ToString()}' and culture '{LocalizationContext.CurrentCulture.CultureCode}'");
                return "/";
            }
            return document.DocumentUrlPath;
        }

        public string GetDocumentUrl(int documentId)
        {
            var doc = DocumentHelper.GetDocument(documentId, new TreeProvider(MembershipContext.AuthenticatedUser));
            return doc?.AbsoluteURL ?? "#";
        }

        public List<string> GetBreadcrumbs(int documentId)
        {
            var breadcrubs = new List<string>();
            var doc = DocumentHelper.GetDocument(documentId, new TreeProvider(MembershipContext.AuthenticatedUser));

            while (doc != null && doc.Parent != null)
            {
                breadcrubs.Add(doc.DocumentName);
                doc = doc.Parent;
            };

            breadcrubs.Reverse();
            return breadcrubs;
        }

        public DateTime GetTaCValidFrom()
        {
            var tacNodeAliasPath = kadenaSettings.TermsAndConditionsPage;
            var tacDocument = DocumentHelper.GetDocuments("KDA.TermsAndConditions")
                .OnCurrentSite()
                .Path(tacNodeAliasPath)
                .Culture(LocalizationContext.CurrentUICulture.CultureCode)
                .FirstObject;

            if (tacDocument == null)
            {
                throw new Exception("Unable to find Terms and condition page");
            }

            return tacDocument.GetDateTimeValue("ValidFrom", DateTime.MinValue);
        }
    }
}