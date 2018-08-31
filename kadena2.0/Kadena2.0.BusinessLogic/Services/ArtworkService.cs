using Kadena.AmazonFileSystemProvider;
using Kadena.BusinessLogic.Contracts;
using Kadena.Helpers;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;

namespace Kadena.BusinessLogic.Services
{
    public class ArtworkService : IArtworkService
    {
        private readonly IKenticoProductsProvider productsProvider;
        private readonly IS3PathService pathService;
        private readonly IKenticoSiteProvider siteProvider;

        public ArtworkService(IKenticoProductsProvider productsProvider, IS3PathService pathService, IKenticoSiteProvider siteProvider)
        {
            this.productsProvider = productsProvider ?? throw new ArgumentNullException(nameof(productsProvider));
            this.pathService = pathService ?? throw new ArgumentNullException(nameof(pathService));
            this.siteProvider = siteProvider ?? throw new ArgumentNullException(nameof(siteProvider));
        }

        public string GetLocation(int documentId)
        {
            var attachmentUri = productsProvider.GetProductArtworkUri(documentId);
            if (attachmentUri == null)
            {
                return null;
            }
            var s3FileUri = new Uri(siteProvider.GetAbsoluteUrl(Helpers.Routes.File.Get), UriKind.Absolute);
            if (s3FileUri.IsBaseOf(attachmentUri))
            {
                return pathService.GetObjectKeyFromPath(attachmentUri.GetParameter("path"), true);
            }
            else
            {
                return attachmentUri.OriginalString;
            }
        }
    }
}
