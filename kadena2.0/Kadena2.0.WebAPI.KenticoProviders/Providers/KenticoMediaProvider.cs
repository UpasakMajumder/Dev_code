using Kadena.WebAPI.KenticoProviders.Contracts;
using CMS.MediaLibrary;
using CMS.SiteProvider;
using CMS.IO;
using System;
using CMS.Helpers;

namespace Kadena.WebAPI.KenticoProviders.Providers
{
    public class KenticoMediaProvider : IKenticoMediaProvider
    {
        public string GetMediaLibrariesLocation()
        {
            return Path.EnsureSlashes(MediaLibraryHelper.GetMediaRootFolderPath(SiteContext.CurrentSiteName, @"\"));
        }

        public string GetMediaLibraryPath(string mediaLibraryFolder)
        {
            return MediaLibraryInfoProvider.GetMediaLibraryFolderPath(SiteContext.CurrentSiteName, mediaLibraryFolder, @"\");
        }

        public string GetThumbnailPath(string mediaLibraryFolder, string mediaFilePath, int maxSideSize)
        {
            var mediaFile = MediaFileInfoProvider.GetMediaFileInfo(SiteContext.CurrentSiteName, mediaFilePath, mediaLibraryFolder);
            return MediaFileInfoProvider.EnsureThumbnailFile(mediaFile, SiteContext.CurrentSiteName, maxSideSize: maxSideSize);
        }

        public string GetThumbnailPath(Guid mediaFileId, string mediaFileName, int maxSideSize)
        {
            var mediaFileLink = MediaFileInfoProvider.GetMediaFileUrl(mediaFileId, mediaFileName);
            var thumbnailLink = URLHelper.AddParameterToUrl(mediaFileLink, "MaxSideSize", maxSideSize.ToString());
            return URLHelper.ResolveUrl(thumbnailLink);
        }

        public bool IsPermanentLink(string link)
        {
            return link.TrimStart('~', '/').StartsWith("getmedia");
        }
    }
}
