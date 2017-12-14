using System;
using System.Collections.Generic;

using CMS.IO;

namespace Kadena.AmazonFileSystemProvider
{
    /// <summary>
    /// Sample of DirectoryInfo class object of CMS.IO provider.
    /// </summary>
    class DirectoryInfo : CMS.IO.DirectoryInfo
    {
        private string _path;


        #region "Constructors"

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="path">Path to directory</param>
        public DirectoryInfo(string path)
        {
            this.FullName = path;
        }

        #endregion


        #region "Public properties"

        /// <summary>
        /// Full name of directory (whole path).
        /// </summary>
        public override string FullName
        {
            get
            {
                return Path.EnsureSlashes(Path.EnsureEndBackslash(Path.Combine(_path, Name)));
            }
            set
            {
                var trimmedPath = value?.TrimEnd(System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar);
                if (string.IsNullOrWhiteSpace(trimmedPath))
                {
                    _path = string.Empty;
                    Name = string.Empty;
                }
                else
                {
                    _path = Path.GetDirectoryName(trimmedPath);
                    Name = Path.GetFileName(trimmedPath);
                }
            }
        }


        /// <summary>
        /// Last write time to directory.
        /// </summary>
        public override DateTime LastWriteTime
        {
            get;
            set;
        }


        /// <summary>
        /// Name of directory (without path).
        /// </summary>
        public override string Name
        {
            get;
            set;
        }


        /// <summary>
        /// Creation time.
        /// </summary>
        public override DateTime CreationTime
        {
            get;
            set;
        }


        /// <summary>
        /// Whether directory exists.
        /// </summary>
        public override bool Exists
        {
            get;
            set;
        }


        /// <summary>
        /// Parent directory.
        /// </summary>
        public override CMS.IO.DirectoryInfo Parent
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion


        #region "Methods"

        /// <summary>
        /// Creates subdirectory.
        /// </summary>
        /// <param name="subdir">Subdirectory to create.</param>
        protected override CMS.IO.DirectoryInfo CreateSubdirectoryInternal(string subdir)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Deletes directory
        /// </summary>
        protected override void DeleteInternal()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Returns an enumerable collection of directory information that matches a specified search pattern and search subdirectory option.
        /// </summary>
        /// <param name="searchPattern">Search pattern.</param>
        /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or all subdirectories.</param>
        /// <returns>An enumerable collection of directories that matches <paramref name="searchPattern"/> and <paramref name="searchOption"/>.</returns>
        protected override IEnumerable<CMS.IO.DirectoryInfo> EnumerateDirectoriesInternal(string searchPattern, SearchOption searchOption)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Returns an array of directories in the current DirectoryInfo matching the given search criteria and using a value
        /// to determine whether to search subdirectories.
        /// </summary>
        /// <param name="searchPattern">Search pattern.</param>
        /// <param name="searchOption">Specifies whether to search the current directory, or the current directory and all subdirectories.</param>
        protected override CMS.IO.DirectoryInfo[] GetDirectoriesInternal(string searchPattern, SearchOption searchOption)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Returns an enumerable collection of file information that matches a specified search pattern and search subdirectory option.
        /// </summary>
        /// <param name="searchPattern">Search pattern.</param>
        /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or all subdirectories.</param>
        /// <returns>An enumerable collection of files that matches <paramref name="searchPattern"/> and <paramref name="searchOption"/>.</returns>
        protected override IEnumerable<CMS.IO.FileInfo> EnumerateFilesInternal(string searchPattern, SearchOption searchOption)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Returns files of the current directory.
        /// </summary>
        /// <param name="searchPattern">Search pattern.</param>
        /// <param name="searchOption">Whether return files from top directory or also from any subdirectories.</param>
        protected override CMS.IO.FileInfo[] GetFilesInternal(string searchPattern, CMS.IO.SearchOption searchOption)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
