namespace Kadena.AmazonFileSystemProvider
{
    /// <summary>
    /// Defines which type of objects is returned by S3ObjectInfoProvider.GetObjectList
    /// </summary>
    public enum ObjectTypeEnum
    {
        Files,
        Directories,
        FilesAndDirectories,
    }
}