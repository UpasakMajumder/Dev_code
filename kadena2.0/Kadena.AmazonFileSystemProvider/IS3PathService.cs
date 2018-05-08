namespace Kadena.AmazonFileSystemProvider
{
    public interface IS3PathService
    {
        string CurrentDirectory { get; }
        string GetPathFromObjectKey(string objectKey, bool absolute, bool directory, bool lower);
        string GetObjectKeyFromPath(string path, bool lower);
        string GetObjectKeyFromPathNonEnvironment(string path, bool lower = true);
        string EnsureFullKey(string key);
        string GetValidPath(string path, bool lower);
    }
}
