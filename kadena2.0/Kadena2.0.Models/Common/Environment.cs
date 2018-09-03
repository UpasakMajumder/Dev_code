namespace Kadena.Models.Common
{
    public class Environment
    {
        public int Id { get; set; }
        public string AmazonS3Folder { get; set; }
        public string AmazonS3ExcludedPaths { get; set; }
    }
}
