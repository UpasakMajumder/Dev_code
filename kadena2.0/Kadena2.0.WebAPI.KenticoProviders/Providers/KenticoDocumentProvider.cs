using AutoMapper;
using CMS.DocumentEngine;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Collections.Generic;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoDocumentProvider : IKenticoDocumentProvider
    {
        //private readonly IKenticoResourceService resources;
        private readonly IKenticoLogger logger;
        //private readonly IMapper _mapper;

        public KenticoDocumentProvider(IKenticoResourceService resources, IKenticoLogger logger, IMapper mapper)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

          //  this.resources = resources;
            this.logger = logger;
  //          _mapper = mapper;
        }

        /// <summary>
        /// Gets document URL with respect to current culture
        /// </summary>
        public string GetDocumentUrl(string aliasPath)
        {
            return GetDocumentUrl(aliasPath, LocalizationContext.CurrentCulture.CultureCode);
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
    }
}