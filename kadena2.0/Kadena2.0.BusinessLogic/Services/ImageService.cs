using System;
using System.IO;
using Kadena.BusinessLogic.Contracts;
using Kadena.Helpers;
using Kadena.WebAPI.KenticoProviders.Contracts;

namespace Kadena.BusinessLogic.Services
{
    public class ImageService : IImageService
    {
        private readonly IKenticoMediaProvider _mediaProvider;
        private readonly IKenticoSiteProvider _siteProvider;
        private readonly IKenticoResourceService _resources;

        public ImageService(IKenticoMediaProvider mediaProvider, IKenticoSiteProvider siteProvider, IKenticoResourceService resources)
        {
            _mediaProvider = mediaProvider ?? throw new ArgumentNullException(nameof(mediaProvider));
            _siteProvider = siteProvider ?? throw new ArgumentNullException(nameof(siteProvider));
            _resources = resources ?? throw new ArgumentNullException(nameof(resources));
        }

        public string GetThumbnailLink(string originalImageLink)
        {
            if (string.IsNullOrWhiteSpace(originalImageLink))
            {
                return string.Empty;
            }

            var hostUri = new Uri(_siteProvider.GetFullUrl());
            var originalImageUri = new Uri(originalImageLink, UriKind.RelativeOrAbsolute);
            if (originalImageUri.IsAbsoluteUri && !hostUri.IsBaseOf(originalImageUri))
            {
                return originalImageLink;
            }

            originalImageUri = new Uri(hostUri, originalImageLink.TrimStart('~'));
            var s3FileUri = new Uri(hostUri, Helpers.Routes.File.Get);

            var originalFileRelativeLink = originalImageUri.LocalPath.TrimStart('/').ToLower();
            if (s3FileUri.IsBaseOf(originalImageUri))
            {
                originalFileRelativeLink = originalImageUri.GetParameter("path").TrimStart('/').ToLower();
            }

            var mediaLibrariesLocation = _resources.GetMediaLibrariesLocation().TrimStart('/').ToLower();
            if (originalFileRelativeLink.IndexOf(mediaLibrariesLocation, 0) != 0)
            {
                return originalImageLink;
            }

            var fileLibraryLink = originalFileRelativeLink.Remove(0, mediaLibrariesLocation.Length).TrimStart('/');
            var libraryFolderName = fileLibraryLink.Split('/')[0];
            var fileLibraryRelativeLink = fileLibraryLink.Remove(fileLibraryLink.IndexOf(libraryFolderName, 0), libraryFolderName.Length).TrimStart('/');

            var thumbnailRelativePath = _mediaProvider.GetThumbnailPath(libraryFolderName, fileLibraryRelativeLink, 200).TrimStart('/');

            var originalFileFolder = Path.GetDirectoryName(originalFileRelativeLink);
            var originalFileFolderUri = new Uri(hostUri, $"{originalFileFolder}/");
            var thumbnailUri = new Uri(originalFileFolderUri, thumbnailRelativePath);

            if (s3FileUri.IsBaseOf(originalImageUri))
            {
                return s3FileUri
                    .AddParameter("path", thumbnailUri.LocalPath.TrimStart('/'))
                    .PathAndQuery;
            }
            return thumbnailUri.LocalPath;
        }
    }
}
