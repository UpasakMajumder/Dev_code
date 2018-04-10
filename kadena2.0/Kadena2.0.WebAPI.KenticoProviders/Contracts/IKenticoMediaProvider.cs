namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoMediaProvider
    {
        string GetThumbnailPath(string mediaLibraryFolder, string mediaFilePath, int maxSideSize);
    }
}
