using CMS.Base;
using CMS.IO;
using System;

namespace Kadena.AmazonFileSystemProvider
{
    public class S3PathService : IS3PathService
    {
        public string CurrentDirectory { get; } = Directory.CurrentDirectory;

        public string GetObjectKeyFromPath(string path, bool lower)
        {
            return GetObjectKeyFromPathNonEnvironment(path, lower);
        }

        public string GetObjectKeyFromPathNonEnvironment(string path, bool lower = true)
        {
            if (path == null)
            {
                return null;
            }
            bool isDirectory = path.EndsWith("\\", StringComparison.Ordinal) || path.EndsWith("/", StringComparison.Ordinal);
            path = GetValidPath(path, lower);
            string currentDirectory = lower ? CurrentDirectory.ToLowerInvariant() : CurrentDirectory;
            if (path.StartsWith(currentDirectory, StringComparison.Ordinal))
            {
                path = path.Substring(currentDirectory.Length);
            }
            if (path.StartsWith("~\\", StringComparison.Ordinal))
            {
                path = path.Substring(2);
            }
            if (isDirectory)
            {
                path += "/";
            }
            path = Path.EnsureSlashes(path, false);
            return $"{path.TrimStart('/')}";
        }

        public string GetPathFromObjectKey(string objectKey, bool absolute, bool directory, bool lower)
        {
            if (objectKey == null)
            {
                return null;
            }
            string result = GetValidPath(objectKey, lower);
            if (absolute)
            {
                string currentDirectory = lower ? CurrentDirectory.ToLowerInvariant() : CurrentDirectory;
                result = $"{currentDirectory}\\{result}";
            }
            if (directory)
            {
                result += "\\";
            }
            return result;
        }

        public string GetValidPath(string path, bool lower)
        {
            if (path == null)
            {
                return null;
            }
            path = Path.EnsureBackslashes(path, true);
            if (lower)
            {
                path = path.ToLowerCSafe();
            }
            return path;
        }
    }
}
