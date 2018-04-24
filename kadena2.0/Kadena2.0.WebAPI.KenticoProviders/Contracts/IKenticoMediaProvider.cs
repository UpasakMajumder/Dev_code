using System;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoMediaProvider
    {
        string GetThumbnailPath(string mediaLibraryFolder, string mediaFilePath, int maxSideSize);

        string GetThumbnailPath(Guid mediaFileId, string mediaFileName, int maxSideSize);

        string GetMediaLibraryPath(string mediaLibraryFolder);

        string GetMediaLibrariesLocation();
    }
}
