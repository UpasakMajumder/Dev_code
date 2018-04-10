using Kadena.WebAPI.KenticoProviders.Contracts;
using CMS.MediaLibrary;
using CMS.SiteProvider;

namespace Kadena.WebAPI.KenticoProviders.Providers
{
    public class KenticoMediaProvider : IKenticoMediaProvider
    {
        public string GetThumbnailPath(string mediaLibraryFolder, string mediaFilePath, int maxSideSize)
        {
            var mediaFile = MediaFileInfoProvider.GetMediaFileInfo(SiteContext.CurrentSiteName, mediaFilePath, mediaLibraryFolder);
            return MediaFileInfoProvider.EnsureThumbnailFile(mediaFile, SiteContext.CurrentSiteName, maxSideSize: maxSideSize);
        }
    }
}
