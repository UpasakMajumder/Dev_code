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
        private static readonly IPathProcessor _pathProcessor;

        private static string mTempPath;
        private static string mCachePath;
        private static string mCurrentDirectory;

        static PathHelper()
        {
            _pathProcessor = new PathProcessor();
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
            return _pathProcessor.GetValidPath(path, true);
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
            return _pathProcessor.GetPathFromObjectKey(objectKey, absolute, directory, lower);
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
            return _pathProcessor.GetObjectKeyFromPath(path, lower);
        }

        internal static string GetObjectKeyFromPathNonEnvironment(string path, bool lower = true)
        {
            return _pathProcessor.GetObjectKeyFromPathNonEnvironment(path, lower);
        }

        public static string EnsureFullKey(string key)
        {
            return _pathProcessor.EnsureFullKey(key);
        }

        /// <summary>Returns relative path from absolute one.</summary>
        /// <param name="absolute">Absolute path to process</param>
        internal static string GetRelativePath(string absolute)
        {
            if (absolute.StartsWith(_pathProcessor.CurrentDirectory, StringComparison.OrdinalIgnoreCase))
            {
                absolute = absolute.Substring(_pathProcessor.CurrentDirectory.Length);
            }
            return absolute.TrimStart('\\');
        }
    }
}
