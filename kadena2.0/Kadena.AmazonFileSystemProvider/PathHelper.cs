using CMS.Base;
using CMS.IO;
using System;

namespace Kadena.AmazonFileSystemProvider
{
    /// <summary>
    /// Contains helper methods for conversion between NTFS and Amazon S3 storage.
    /// </summary>
    public static class PathHelper
    {
        private static string _environmentFolder;
        private static string _defaultSpecialFolder;

        private static string mTempPath;
        private static string mCachePath;
        private static string mCurrentDirectory;

        static PathHelper()
        {
            var provider = StorageHelper.GetStorageProvider("~/");
            if (string.IsNullOrWhiteSpace(provider.CustomRootUrl))
            {
                _environmentFolder = string.Empty;
            }
            else
            {
                _environmentFolder = $"{provider.CustomRootUrl.Trim('/')}/";
            }
            _defaultSpecialFolder = $"{_environmentFolder}media/";
        }

        /// <summary>Gets or sets path to local storage for temp.</summary>
        internal static string TempPath
        {
            get
            {
                return mTempPath ?? (mTempPath = GetPathToTempDirectory(SettingsHelper.AppSettings["CMSAmazonTempPath"], "AmazonTemp"));
            }
            set
            {
                mTempPath = value;
            }
        }

        /// <summary>Gets or sets path to local storage for cache.</summary>
        internal static string CachePath
        {
            get
            {
                return mCachePath ?? (mCachePath = GetPathToTempDirectory(SettingsHelper.AppSettings["CMSAmazonCachePath"], "AmazonCache"));
            }
            set
            {
                mCachePath = value;
            }
        }

        /// <summary>Gets or sets current directory.</summary>
        internal static string CurrentDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(mCurrentDirectory))
                {
                    mCurrentDirectory = Directory.CurrentDirectory;
                }
                return mCurrentDirectory;
            }
            set
            {
                mCurrentDirectory = value;
            }
        }

        /// <summary>
        /// Returns absolute path to temp. Also ensures that the directory exists.
        /// </summary>
        /// <param name="tempAbsolutePath">Absolute path to temp.</param>
        /// <param name="relativeDirectoryPath">Path to directory relative to default temp root.</param>
        /// <remarks>Relative directory path is used only when absolute path is not specified.</remarks>
        private static string GetPathToTempDirectory(string tempAbsolutePath, string relativeDirectoryPath)
        {
            if (string.IsNullOrEmpty(tempAbsolutePath))
            {
                tempAbsolutePath = $"{SystemContext.WebApplicationPhysicalPath}\\App_Data\\{relativeDirectoryPath}";
            }
            if (!System.IO.Directory.Exists(tempAbsolutePath))
            {
                System.IO.Directory.CreateDirectory(tempAbsolutePath);
            }
            return tempAbsolutePath.TrimEnd('\\');
        }

        /// <summary>
        /// Converts path to valid one (replaces slash to back slash) and lower the case in the path.
        /// </summary>
        /// <param name="path">Path</param>
        internal static string GetValidPath(string path)
        {
            return GetValidPath(path, true);
        }

        /// <summary>
        /// Converts path to valid one (replaces slash to back slash) and optionally lower the case in the path.
        /// </summary>
        /// <param name="path">Path</param>
        /// <param name="lower">Specifies whether path should be lowered inside method.</param>
        internal static string GetValidPath(string path, bool lower)
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

        /// <summary>Returns path from given object key.</summary>
        /// <param name="objectKey">Object key.</param>
        /// <param name="absolute">Indicates whether returned path is absolute</param>
        internal static string GetPathFromObjectKey(string objectKey, bool absolute)
        {
            return GetPathFromObjectKey(objectKey, absolute, false, true);
        }

        /// <summary>Returns path from given object key.</summary>
        /// <param name="objectKey">Object key.</param>
        /// <param name="absolute">Indicates whether returned path is absolute</param>
        /// <param name="directory">Specifies whether object is directory.</param>
        /// <param name="lower">Specifies whether path should be lowered inside method.</param>
        internal static string GetPathFromObjectKey(string objectKey, bool absolute, bool directory, bool lower)
        {
            if (objectKey == null)
            {
                return null;
            }
            string nonEnvPath = objectKey;
            if (objectKey.StartsWith(_defaultSpecialFolder))
            {
                nonEnvPath = objectKey.Substring(_defaultSpecialFolder.Length);
            }
            string str1 = GetValidPath(nonEnvPath, lower);
            string str2 = lower ? CurrentDirectory.ToLowerInvariant() : CurrentDirectory;
            if (absolute)
            {
                str1 = $"{str2}\\{str1}";
            }
            if (directory)
            {
                str1 += "\\";
            }
            return str1;
        }

        /// <summary>Returns object key from given path.</summary>
        /// <param name="path">Path.</param>
        public static string GetObjectKeyFromPath(string path)
        {
            return GetObjectKeyFromPath(path, true);
        }

        /// <summary>Returns object key from given path.</summary>
        /// <param name="path">Path.</param>
        /// <param name="lower">Specifies whether path should be lowered inside method.</param>
        internal static string GetObjectKeyFromPath(string path, bool lower)
        {
            return $"{_defaultSpecialFolder}{GetObjectKeyFromPathNonEnvironment(path, lower)}";
        }

        public static string GetObjectKeyFromPathNonEnvironment(string path, bool lower = true)
        {
            if (path == null)
            {
                return null;
            }
            bool isDirectory = path.EndsWith("\\", StringComparison.Ordinal) || path.EndsWith("/", StringComparison.Ordinal);
            path = GetValidPath(path, lower);
            string str = lower ? CurrentDirectory.ToLowerInvariant() : CurrentDirectory;
            if (path.StartsWith(str, StringComparison.Ordinal))
            {
                path = path.Substring(str.Length);
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

        public static string EnsureFullKey(string key)
        {
            if (key.StartsWith(_defaultSpecialFolder))
            {
                return key;
            }
            return $"{_defaultSpecialFolder}{key}";
        }

        /// <summary>Returns relative path from absolute one.</summary>
        /// <param name="absolute">Absolute path to process</param>
        internal static string GetRelativePath(string absolute)
        {
            if (absolute.StartsWith(CurrentDirectory, StringComparison.OrdinalIgnoreCase))
            {
                absolute = absolute.Substring(CurrentDirectory.Length);
            }
            return absolute.TrimStart('\\');
        }
    }
}
