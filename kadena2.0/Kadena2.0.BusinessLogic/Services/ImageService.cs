using System;
using Kadena.AmazonFileSystemProvider;
using Kadena.BusinessLogic.Contracts;
using Kadena.Helpers;
using Kadena.Models.SiteSettings;
using Kadena.WebAPI.KenticoProviders.Contracts;

namespace Kadena.BusinessLogic.Services
{
    public class ImageService : IImageService
    {
        private readonly IKenticoMediaProvider _mediaProvider;
        private readonly IKenticoSiteProvider _siteProvider;
        private readonly IKenticoResourceService _resourceService;

        public ImageService(IKenticoMediaProvider mediaProvider, IKenticoSiteProvider siteProvider, IKenticoResourceService resourceService)
        {
            _mediaProvider = mediaProvider ?? throw new ArgumentNullException(nameof(mediaProvider));
            _siteProvider = siteProvider ?? throw new ArgumentNullException(nameof(siteProvider));
            _resourceService = resourceService ?? throw new ArgumentNullException(nameof(resourceService));
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
            var originalFileRelativeLink = originalImageUri.LocalPath.TrimStart('/').ToLower();

            var thumbnailMaxSideSize = _resourceService.GetSiteSettingsKey<int>(Settings.KDA_ThumbnailMaxSideSize);

            if (_mediaProvider.IsPermanentLink(originalFileRelativeLink))
            {
                var linkParts = originalFileRelativeLink.Split('/');
                if (linkParts.Length == 3)
                {
                    var mediaFileId = new Guid(linkParts[1]);
                    return _mediaProvider.GetThumbnailPath(mediaFileId, linkParts[2], thumbnailMaxSideSize);
                }
            }

            var s3FileUri = new Uri(hostUri, Helpers.Routes.File.Get);
            if (s3FileUri.IsBaseOf(originalImageUri))
            {
                originalFileRelativeLink = originalImageUri.GetParameter("path").TrimStart('/').ToLower();
            }

            var mediaLibrariesLocation = _mediaProvider.GetMediaLibrariesLocation().TrimStart('/').ToLower();
            if (originalFileRelativeLink.IndexOf(mediaLibrariesLocation, 0) != 0)
            {
                return originalImageLink;
            }

            var fileLibraryLink = originalFileRelativeLink.Remove(0, mediaLibrariesLocation.Length).TrimStart('/');
            var libraryFolderName = fileLibraryLink.Split('/')[0];
            var fileLibraryRelativeLink = fileLibraryLink.Remove(fileLibraryLink.IndexOf(libraryFolderName, 0), libraryFolderName.Length).TrimStart('/');

            var thumbnailLibraryLink = _mediaProvider.GetThumbnailPath(libraryFolderName, fileLibraryRelativeLink, thumbnailMaxSideSize)?.TrimStart('/');

            if (string.IsNullOrWhiteSpace(thumbnailLibraryLink))
            {
                return originalImageLink;
            }

            var mediaLibraryLink = _mediaProvider.GetMediaLibraryPath(libraryFolderName);
            var originalFileFolderUri = new Uri(hostUri, $"{mediaLibraryLink}/");
            var thumbnailUri = new Uri(originalFileFolderUri, thumbnailLibraryLink);

            if (s3FileUri.IsBaseOf(originalImageUri))
            {
                return s3FileUri
                    .AddParameter("path", PathHelper.GetObjectKeyFromPathNonEnvironment(thumbnailUri.LocalPath))
                    .PathAndQuery;
            }
            return thumbnailUri.LocalPath;
        }
    }
}
