using Amazon.S3;
using Amazon.S3.Model;
using CMS.Base;
using CMS.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;

namespace Kadena.AmazonFileSystemProvider
{
    /// <summary>Implementation of Directory class for Amazon S3.</summary>
    public class Directory : AbstractDirectory
    {
        private static string mCurrentDirectory;

        /// <summary>
        /// Returns current directory. Value remains the same so it can be cached.
        /// </summary>
        internal static string CurrentDirectory
        {
            get
            {
                if (Directory.mCurrentDirectory == null)
                    Directory.mCurrentDirectory = new Directory().GetCurrentDirectory().ToLowerInvariant();
                return Directory.mCurrentDirectory;
            }
        }

        /// <summary>Returns S3Object provider.</summary>
        private IS3ObjectInfoProvider Provider
        {
            get
            {
                return S3ObjectFactory.Provider;
            }
        }

        /// <summary>
        /// Determines whether the given path refers to an existing directory on disk.
        /// </summary>
        /// <param name="path">Path to test.</param>
        public override bool Exists(string path)
        {
            if (!Directory.ExistsInFileSystem(path))
                return Directory.ExistsInS3Storage(path);
            return true;
        }

        /// <summary>
        /// Creates all directories and subdirectories as specified by path.
        /// </summary>
        /// <param name="path">Path to create.</param>
        public override CMS.IO.DirectoryInfo CreateDirectory(string path)
        {
            path = PathHelper.GetValidPath(path);
            if (this.Exists(path))
                return (CMS.IO.DirectoryInfo)new DirectoryInfo(path);
            IS3ObjectInfo info = S3ObjectFactory.GetInfo(path);
            info.Key = info.Key + "/";
            this.Provider.CreateEmptyObject(info);
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            directoryInfo.CreationTime = DateTime.Now;
            directoryInfo.Exists = true;
            directoryInfo.FullName = path;
            directoryInfo.LastWriteTime = directoryInfo.CreationTime;
            directoryInfo.Name = System.IO.Path.GetFileName(path);
            return (CMS.IO.DirectoryInfo)directoryInfo;
        }

        /// <summary>
        /// Returns an enumerable collection of file names that match a search pattern in a specified path.
        /// </summary>
        /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
        /// <param name="searchPattern">Search pattern.</param>
        /// <returns>An enumerable collection of the full names (including paths) for the files in the directory specified by <paramref name="path" /> and that match the specified search pattern.</returns>
        public override IEnumerable<string> EnumerateFiles(string path, string searchPattern)
        {
            FileDebug.LogFileOperation(path, nameof(EnumerateFiles), -1, (string)null, (string)null, "Amazon");
            return this.EnumerateFilesCore(path, searchPattern);
        }

        /// <summary>
        /// Returns an enumerable collection of file names that match a search pattern in a specified path.
        /// </summary>
        private IEnumerable<string> EnumerateFilesCore(string path, string searchPattern)
        {
            IEnumerable<string> first = (IEnumerable<string>)null;
            if (Directory.ExistsInFileSystem(path))
                first = System.IO.Directory.EnumerateFiles(path, searchPattern).Select<string, string>((Func<string, string>)(f => f.ToLowerInvariant()));
            path = PathHelper.GetValidPath(path);
            Func<string, bool> searchCondition = this.GetSearchCondition(searchPattern);
            IEnumerable<string> second = this.Provider.GetObjectsList(path, ObjectTypeEnum.Files, false, true, true).Where<string>((Func<string, bool>)(f => searchCondition(CMS.IO.Path.GetFileName(f)))).Select<string, string>((Func<string, string>)(f => f.ToLowerInvariant()));
            if (first != null)
                return first.Union<string>(second, (IEqualityComparer<string>)StringComparer.Ordinal);
            return second;
        }

        /// <summary>
        /// Returns the names of files (including their paths) that match the specified search pattern in the specified directory.
        /// </summary>
        /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
        /// <param name="searchPattern">Search pattern.</param>
        /// <returns>An array of the full names (including paths) for the files in the specified directory that match the specified search pattern, or an empty array if no files are found.</returns>
        public override string[] GetFiles(string path, string searchPattern)
        {
            string[] array = this.EnumerateFilesCore(path, searchPattern).ToArray<string>();
            FileDebug.LogFileOperation(path, nameof(GetFiles), -1, array.Length.ToString(), (string)null, "Amazon");
            return array;
        }

        /// <summary>
        /// Returns an enumerable collection of directory names that match a search pattern in a specified path,
        /// and optionally searches subdirectories.
        /// </summary>
        /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
        /// <param name="searchPattern">Search pattern.</param>
        /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or should include all subdirectories.</param>
        /// <returns>An enumerable collection of the full names (including paths) for the directories in the directory specified by <paramref name="path" /> and that match the specified search pattern and option.</returns>
        public override IEnumerable<string> EnumerateDirectories(string path, string searchPattern, CMS.IO.SearchOption searchOption)
        {
            FileDebug.LogFileOperation(path, nameof(EnumerateDirectories), -1, (string)null, (string)null, "Amazon");
            return this.EnumerateDirectoriesCore(path, searchPattern, searchOption);
        }

        /// <summary>
        /// Returns an enumerable collection of directory names that match a search pattern in a specified path,
        /// and optionally searches subdirectories.
        /// </summary>
        private IEnumerable<string> EnumerateDirectoriesCore(string path, string searchPattern, CMS.IO.SearchOption searchOption)
        {
            IEnumerable<string> first = (IEnumerable<string>)null;
            if (Directory.ExistsInFileSystem(path))
                first = System.IO.Directory.EnumerateDirectories(path, searchPattern, (System.IO.SearchOption)searchOption).Select<string, string>((Func<string, string>)(d => d.ToLowerInvariant()));
            path = PathHelper.GetValidPath(path);
            Func<string, bool> searchCondition = this.GetSearchCondition(searchPattern);
            IEnumerable<string> second = this.Provider.GetObjectsList(path, ObjectTypeEnum.Directories, searchOption == CMS.IO.SearchOption.AllDirectories, true, true).Select<string, string>((Func<string, string>)(d => d.TrimEnd('\\'))).Where<string>((Func<string, bool>)(d => searchCondition(CMS.IO.Path.GetFileName(d)))).Select<string, string>((Func<string, string>)(d => d.ToLowerInvariant()));
            if (first != null)
                return first.Union<string>(second, (IEqualityComparer<string>)StringComparer.Ordinal);
            return second;
        }

        /// <summary>
        /// Gets the names of the subdirectories (including their paths) that match the specified search pattern in the current directory,
        /// and optionally searches subdirectories.
        /// </summary>
        /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
        /// <param name="searchPattern">Search pattern.</param>
        /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include all subdirectories or only the current directory.</param>
        /// <returns>An array of the full names (including paths) of the subdirectories that match the specified criteria, or an empty array if no directories are found.</returns>
        public override string[] GetDirectories(string path, string searchPattern, CMS.IO.SearchOption searchOption)
        {
            string[] array = this.EnumerateDirectoriesCore(path, searchPattern, searchOption).ToArray<string>();
            FileDebug.LogFileOperation(path, nameof(GetDirectories), -1, array.Length.ToString(), (string)null, "Amazon");
            return array;
        }

        /// <summary>Gets the current working directory of the application.</summary>
        public override string GetCurrentDirectory()
        {
            return SystemContext.WebApplicationPhysicalPath;
        }

        /// <summary>
        /// Deletes an empty directory and, if indicated, any subdirectories and files in the directory.
        /// </summary>
        /// <param name="path">Path to directory</param>
        /// <param name="recursive">Deletes all sub directories in given path.</param>
        public override void Delete(string path, bool recursive)
        {
            if (Directory.ExistsInS3Storage(path))
            {
                if (recursive)
                {
                    this.DeleteInternal(path, true);
                }
                else
                {
                    if (this.Provider.GetObjectsList(path, ObjectTypeEnum.FilesAndDirectories, false, true, true).Count != 0)
                        throw new InvalidOperationException("Directory is not empty.");
                    this.Provider.DeleteObject(S3ObjectFactory.GetInfo(path));
                }
                if (!path.StartsWith(Directory.CurrentDirectory, StringComparison.OrdinalIgnoreCase))
                    return;
                path = path.Substring(Directory.CurrentDirectory.Length);
                try
                {
                    System.IO.Directory.Delete(System.IO.Path.Combine(PathHelper.TempPath, path));
                    System.IO.Directory.Delete(System.IO.Path.Combine(PathHelper.CachePath, path));
                }
                catch (IOException ex)
                {
                }
            }
            else
            {
                if (Directory.ExistsInFileSystem(path))
                    throw new InvalidOperationException("Cannot delete path '" + path + "' because it's not in Amazon S3 storage and it exists only in local file system.\r\n                    This exception typically occurs when file system is mapped to Amazon S3 storage after the file or directory\r\n                    '" + path + "' was created in the local file system. To fix this issue remove specified file or directory.");
                throw new DirectoryNotFoundException("Path '" + path + "' does not exist.");
            }
        }

        /// <summary>Deletes an empty directory.</summary>
        /// <param name="path">Path to directory</param>
        public override void Delete(string path)
        {
            this.Delete(path, false);
        }

        /// <summary>Moves directory.</summary>
        /// <param name="sourceDirName">Source directory name.</param>
        /// <param name="destDirName">Destination directory name.</param>
        public override void Move(string sourceDirName, string destDirName)
        {
            this.Move(sourceDirName, destDirName, 0);
        }

        /// <summary>
        /// Gets a FileSecurity object that encapsulates the access control list (ACL) entries for a specified file.
        /// </summary>
        /// <param name="path">Path to directory.</param>
        public override DirectorySecurity GetAccessControl(string path)
        {
            return new DirectorySecurity();
        }

        /// <summary>Prepares files for import. Converts them to lower case.</summary>
        /// <param name="path">Path.</param>
        public override void PrepareFilesForImport(string path)
        {
            path = PathHelper.GetValidPath(path);
            foreach (string objects in this.Provider.GetObjectsList(path, ObjectTypeEnum.FilesAndDirectories, true, false, true))
            {
                IS3ObjectInfo info1 = S3ObjectFactory.GetInfo(PathHelper.GetObjectKeyFromPath(objects, false), true);
                string lowerInvariant = info1.Key.ToLowerInvariant();
                if (lowerInvariant != info1.Key)
                {
                    IS3ObjectInfo info2 = S3ObjectFactory.GetInfo(info1.Key, true);
                    IS3ObjectInfo info3 = S3ObjectFactory.GetInfo(lowerInvariant, true);
                    this.Provider.CopyObjects(info2, info3);
                    this.Provider.DeleteObject(info2);
                }
            }
        }

        /// <summary>
        /// Deletes all files in the directory structure. It works also in a shared hosting environment.
        /// </summary>
        /// <param name="path">Full path of the directory to delete</param>
        public override void DeleteDirectoryStructure(string path)
        {
            this.DeleteInternal(path, false);
        }

        /// <summary>
        /// Determines whether the given path refers to an existing directory on Amazon S3 storage.
        /// </summary>
        /// <param name="path">Path to test.</param>
        public static bool ExistsInS3Storage(string path)
        {
            path = PathHelper.GetValidPath(path);
            string objectKeyFromPath = PathHelper.GetObjectKeyFromPath(path);
            if (!string.IsNullOrEmpty(objectKeyFromPath))
                return S3ObjectFactory.Provider.ObjectExists(S3ObjectFactory.GetInfo(objectKeyFromPath + "/", true));
            return true;
        }

        /// <summary>
        /// Determines whether the given path refers to an existing directory on file system
        /// </summary>
        /// <param name="path">Path to test.</param>
        public static bool ExistsInFileSystem(string path)
        {
            return System.IO.Directory.Exists(path);
        }

        /// <summary>Creates directory structure on disk for given path.</summary>
        /// <param name="path">Path with temporary file.</param>
        public static void CreateDiskDirectoryStructure(string path)
        {
            string directoryName = CMS.IO.Path.GetDirectoryName(path);
            if (System.IO.Directory.Exists(directoryName))
                return;
            System.IO.Directory.CreateDirectory(directoryName);
        }

        /// <summary>Moves directory.</summary>
        /// <param name="sourceDirName">Source directory name.</param>
        /// <param name="destDirName">Destination directory name.</param>
        /// <param name="level">Current nested level.</param>
        private void Move(string sourceDirName, string destDirName, int level)
        {
            sourceDirName = PathHelper.GetValidPath(sourceDirName);
            destDirName = PathHelper.GetValidPath(destDirName);
            destDirName = destDirName.Trim('\\');
            if (!this.Exists(sourceDirName))
                throw new Exception("Source path " + sourceDirName + " does not exist.");
            if (!Directory.ExistsInS3Storage(destDirName))
                this.CreateDirectory(destDirName);
            foreach (string file in this.GetFiles(sourceDirName))
                CMS.IO.File.Copy(file, destDirName + "\\" + CMS.IO.Path.GetFileName(file));
            foreach (string directory in this.GetDirectories(sourceDirName))
                this.Move(directory, destDirName + "\\" + CMS.IO.Path.GetFileName(directory), level + 1);
            if (level != 0)
                return;
            this.Delete(sourceDirName, true);
        }

        /// <summary>Deletes files and optionally directories in given path.</summary>
        /// <param name="path">Path to delete.</param>
        /// <param name="directories">Indicates, whether directories should be also deleted.</param>
        private void DeleteInternal(string path, bool directories)
        {
            path = PathHelper.GetValidPath(path);
            foreach (IEnumerable<KeyVersion> source in this.Provider.GetObjectsList(path, directories ? ObjectTypeEnum.FilesAndDirectories : ObjectTypeEnum.Files, true, true, false).ConvertAll<KeyVersion>((Converter<string, KeyVersion>)(p => new KeyVersion()
            {
                Key = PathHelper.GetObjectKeyFromPath(p, true)
            })).Batch<KeyVersion>(1000))
            {
                DeleteObjectsRequest request = new DeleteObjectsRequest()
                {
                    BucketName = AccountInfo.Current.BucketName
                };
                request.Objects = source.ToList<KeyVersion>();
                try
                {
                    AccountInfo.Current.S3Client.DeleteObjects(request);
                }
                catch (DeleteObjectsException ex)
                {
                    throw new Exception(string.Format("Some of the directory '{0}' underlying objects weren't deleted correctly", (object)path));
                }
            }
            this.Provider.DeleteObject(S3ObjectFactory.GetInfo(CMS.IO.Path.EnsureEndBackslash(path)));
        }
    }
}
