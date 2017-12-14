using System;
using System.Text.RegularExpressions;

namespace Kadena.AmazonFileSystemProvider
{
    static class AmazonS3Helper
    {
        const string kenticoFolder = "dev/media/";

        private static string GetKey(string objectPath)
        {
            if (objectPath == null)
            {
                throw new ArgumentNullException(nameof(objectPath), "Cannot get key for null value.");
            }
            return $"{kenticoFolder}{objectPath.TrimStart('/')}";
        }

        public static string EnsureKey(string objectPath)
        {
            var key = GetKey(objectPath);
            var avoidSymbols = @"[\x00-\x20%`~^\[\]\\#\{\}<>" + "'\"]";
            if (!Regex.IsMatch(key, avoidSymbols))
            {
                return key;
            }
            throw new InvalidOperationException("Key is not valid to use in Amazon S3.");
        }

        public static string GetBucketName()
        {
            return "kadena-objects";
        }
    }
}
