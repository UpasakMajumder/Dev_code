namespace Kadena.AmazonFileSystemProvider
{
    public interface IS3PathService
    {
        string CurrentDirectory { get; }
        string GetPathFromObjectKey(string objectKey, bool absolute, bool directory, bool lower);
        string GetObjectKeyFromPath(string path, bool lower);
        string GetValidPath(string path, bool lower);
    }
}
