using CMS.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.AmazonFileSystemProvider
{
    /// <summary>Directory info for Amazon storage provider</summary>
    public class DirectoryInfo : CMS.IO.DirectoryInfo
    {
        private readonly string currentPath;
        private readonly System.IO.DirectoryInfo systemDirectory;

        /// <summary>Constructor.</summary>
        /// <param name="path">Path to directory</param>
        public DirectoryInfo(string path)
        {
            this.currentPath = PathHelper.GetValidPath(path);
            this.currentPath = this.currentPath.TrimEnd('\\');
            if (Directory.ExistsInFileSystem(path))
            {
                this.systemDirectory = new System.IO.DirectoryInfo(this.currentPath);
            }
            this.InitCMSValues();
        }

        /// <summary>Creation time.</summary>
        public override DateTime CreationTime { get; set; } = DateTimeHelper.ZERO_TIME;

        /// <summary>Whether directory exists.</summary>
        public override bool Exists { get; set; }

        /// <summary>Full name of directory (whole path).</summary>
        public override string FullName { get; set; }

        /// <summary>Last write time to directory.</summary>
        public override DateTime LastWriteTime { get; set; } = DateTimeHelper.ZERO_TIME;

        /// <summary>Name of directory (without path).</summary>
        public override string Name { get; set; }

        /// <summary>Parent directory</summary>
        public override CMS.IO.DirectoryInfo Parent
        {
            get
            {
                if (this.systemDirectory != null)
                {
                    System.IO.DirectoryInfo parent = this.systemDirectory.Parent;
                    DirectoryInfo directoryInfo = new DirectoryInfo(parent.FullName)
                    {
                        CreationTime = parent.CreationTime,
                        Exists = parent.Exists,
                        FullName = parent.FullName,
                        LastWriteTime = parent.LastWriteTime,
                        Name = parent.Name
                    };
                    return directoryInfo;
                }
                if (string.IsNullOrEmpty(this.currentPath))
                {
                    return this;
                }
                string directoryName = CMS.IO.Path.GetDirectoryName(this.currentPath);
                DirectoryInfo directoryInfo1 = new DirectoryInfo(directoryName)
                {
                    Exists = true,
                    FullName = directoryName,
                    Name = CMS.IO.Path.GetFileName(directoryName)
                };
                return directoryInfo1;
            }
        }

        /// <summary>Creates subdirectory.</summary>
        /// <param name="subdir">Subdirectory to create.</param>
        protected override CMS.IO.DirectoryInfo CreateSubdirectoryInternal(string subdir)
        {
            return CMS.IO.Directory.CreateDirectory($"{this.FullName}\\{subdir}");
        }

        /// <summary>Deletes directory</summary>
        protected override void DeleteInternal()
        {
            CMS.IO.Directory.Delete(this.currentPath, true);
        }

        /// <summary>
        /// Returns an enumerable collection of file information that matches a specified search pattern and search subdirectory option.
        /// </summary>
        /// <param name="searchPattern">Search pattern.</param>
        /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or all subdirectories.</param>
        /// <returns>An enumerable collection of files that matches <paramref name="searchPattern" /> and <paramref name="searchOption" />.</returns>
        protected override IEnumerable<CMS.IO.DirectoryInfo> EnumerateDirectoriesInternal(string searchPattern, CMS.IO.SearchOption searchOption)
        {
            return CMS.IO.Directory.EnumerateDirectories(this.FullName, searchPattern, searchOption)
                .Select(d => new DirectoryInfo(d));
        }

        /// <summary>
        /// Returns an array of directories in the current DirectoryInfo matching the given search criteria and using a value
        /// to determine whether to search subdirectories.
        /// </summary>
        /// <param name="searchPattern">Search pattern.</param>
        /// <param name="searchOption">Specifies whether to search the current directory, or the current directory and all subdirectories.</param>
        protected override CMS.IO.DirectoryInfo[] GetDirectoriesInternal(string searchPattern, CMS.IO.SearchOption searchOption)
        {
            return this.EnumerateDirectoriesInternal(searchPattern, searchOption).ToArray();
        }

        /// <summary>
        /// Returns an enumerable collection of directory information that matches a specified search pattern and search subdirectory option.
        /// </summary>
        /// <param name="searchPattern">Search pattern.</param>
        /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or all subdirectories.</param>
        /// <returns>An enumerable collection of directories that matches <paramref name="searchPattern" /> and <paramref name="searchOption" />.</returns>
        protected override IEnumerable<CMS.IO.FileInfo> EnumerateFilesInternal(string searchPattern, CMS.IO.SearchOption searchOption)
        {
            IEnumerable<CMS.IO.FileInfo> first = CMS.IO.Directory
                .EnumerateFiles(this.FullName, searchPattern)
                .Select(f => new FileInfo(f));
            if (searchOption == CMS.IO.SearchOption.AllDirectories)
            {
                IEnumerable<string> source = CMS.IO.Directory.EnumerateDirectories(this.FullName);
                first = first.Concat(source.SelectMany(d => new DirectoryInfo(d).EnumerateFiles(searchPattern, searchOption)));
            }
            return first;
        }

        /// <summary>Returns files of the current directory.</summary>
        /// <param name="searchPattern">Search pattern.</param>
        /// <param name="searchOption">Search option.</param>
        protected override CMS.IO.FileInfo[] GetFilesInternal(string searchPattern, CMS.IO.SearchOption searchOption)
        {
            return this.EnumerateFilesInternal(searchPattern, searchOption).ToArray();
        }

        /// <summary>Initializes CMS values by System.IO values</summary>
        private void InitCMSValues()
        {
            if (this.systemDirectory != null)
            {
                this.InitCMSValuesFromSystemDirectory();
            }
            else
            {
                this.InitCMSValuesFromS3ObjectInfo();
            }
        }

        /// <summary>Initializes CMS values by System.IO values</summary>
        private void InitCMSValuesFromSystemDirectory()
        {
            if (this.systemDirectory != null)
            {
                this.CreationTime = this.systemDirectory.CreationTime;
                this.Exists = this.systemDirectory.Exists;
                this.FullName = this.systemDirectory.FullName;
                this.LastWriteTime = this.systemDirectory.LastWriteTime;
                this.Name = this.systemDirectory.Name;
            }
        }

        /// <summary>Initializes CMS values by S3 object.</summary>
        private void InitCMSValuesFromS3ObjectInfo()
        {
            if (!string.IsNullOrEmpty(this.currentPath))
            {
                this.Exists = Directory.ExistsInS3Storage(this.currentPath);
                this.FullName = this.currentPath;
                this.Name = CMS.IO.Path.GetFileName(this.FullName);
            }
        }
    }
}
