using CMS.Base;
using CMS.IO;
using System;

namespace Kadena.AmazonFileSystemProvider
{
    public class PathProcessor : IPathProcessor
    {
        public PathProcessor()
        {
            var provider = StorageHelper.GetStorageProvider("~/");
            if (string.IsNullOrWhiteSpace(provider.CustomRootUrl))
            {
                EnvironmentFolder = string.Empty;
            }
            else
            {
                EnvironmentFolder = $"{provider.CustomRootUrl.Trim('/')}/";
            }
            DefaultSpecialFolder = $"{EnvironmentFolder}media/";
            CurrentDirectory = Directory.CurrentDirectory;
        }

        public string CurrentDirectory { get; private set; }

        protected string EnvironmentFolder { get; private set; }

        protected string DefaultSpecialFolder { get; private set; }

        public string EnsureFullKey(string key)
        {
            if (key.StartsWith(DefaultSpecialFolder))
            {
                return key;
            }
            return $"{DefaultSpecialFolder}{key}";
        }

        public string GetObjectKeyFromPath(string path, bool lower)
        {
            return $"{DefaultSpecialFolder}{GetObjectKeyFromPathNonEnvironment(path, lower)}";
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
            string nonEnvPath = objectKey;
            if (nonEnvPath.StartsWith(DefaultSpecialFolder))
            {
                nonEnvPath = nonEnvPath.Substring(DefaultSpecialFolder.Length);
            }
            string result = GetValidPath(nonEnvPath, lower);
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
