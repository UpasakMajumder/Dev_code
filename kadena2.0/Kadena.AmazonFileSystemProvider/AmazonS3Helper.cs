using System.Text.RegularExpressions;

namespace Kadena.AmazonFileSystemProvider
{
    static class AmazonS3Helper
    {
        const string kenticoFolder = "dev/media/";

        private static string GetKey(string objectPath)
        {
            return $"{kenticoFolder}{objectPath.TrimStart('/')}";
        }

        public static string EnsureKey(string objectPath)
        {
            var avoidSymbols = @"[\x00-\x20%`~^\[\]\\#\{\}<>" + "'\"]";
            var key = GetKey(objectPath);
            if (!Regex.IsMatch(key, avoidSymbols))
            {
                return key;
            }
            return null;
        }
    }
}
