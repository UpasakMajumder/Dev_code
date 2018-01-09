using CMS.Base;
using CMS.Globalization;
using CMS.Helpers;
using CMS.IO;
using System;
using System.IO;
using System.Security.AccessControl;
using System.Text;

namespace Kadena.AmazonFileSystemProvider
{
    /// <summary>Implementation of File class for Amazon S3.</summary>
    public class File : AbstractFile
    {
        private static IS3ObjectInfoProvider mProvider;

        /// <summary>Get or set S3ObjectInfoProvider.</summary>
        public static IS3ObjectInfoProvider Provider
        {
            get
            {
                if (mProvider == null)
                {
                    mProvider = S3ObjectFactory.Provider;
                }
                return mProvider;
            }
            set
            {
                mProvider = value;
            }
        }

        /// <summary>Determines whether the specified file exists.</summary>
        /// <param name="path">Path to file.</param>
        public override bool Exists(string path)
        {
            if (!this.ExistsInFileSystem(path))
            {
                return this.ExistsInS3Storage(path);
            }
            return true;
        }

        /// <summary>Opens an existing UTF-8 encoded text file for reading.</summary>
        /// <param name="path">Path to file</param>
        public override CMS.IO.StreamReader OpenText(string path)
        {
            if (this.ExistsInS3Storage(path))
            {
                System.IO.Stream objectContent = Provider
                    .GetObjectContent(S3ObjectFactory.GetInfo(path), System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read, 4096);
                return objectContent == null ? null : CMS.IO.StreamReader.New(objectContent);
            }
            if (this.ExistsInFileSystem(path))
            {
                return CMS.IO.StreamReader.New(System.IO.File.OpenText(path));
            }
            throw GetFileNotFoundException(path);
        }

        /// <summary>
        /// Deletes the specified file. An exception is not thrown if the specified file does not exist.
        /// </summary>
        /// <param name="path">Path to file</param>
        public override void Delete(string path)
        {
            if (!this.Exists(path))
            {
                throw GetFileNotFoundException(path);
            }
            IS3ObjectInfo info1 = S3ObjectFactory.GetInfo(path);
            if (Provider.ObjectExists(info1))
            {
                Provider.DeleteObject(info1);
                IS3ObjectInfo info2 = S3ObjectFactory.GetInfo(CMS.IO.Path.GetDirectoryName(path));
                info2.Key = $"{info2.Key}/";
                if (!Provider.ObjectExists(info2))
                {
                    Provider.CreateEmptyObject(info2);
                }
                info2.SetMetadata(S3ObjectInfoProvider.LAST_WRITE_TIME, S3ObjectInfoProvider.GetDateTimeString(DateTime.Now));
            }
            else
            {
                throw new InvalidOperationException($"File '{path}' cannot be deleted because it exists only in application file system. \r\n                    This exception typically occurs when file system is mapped to Amazon S3 storage after the file or directory\r\n                    '{path}' was created in the local file system. To fix this issue remove specified file or directory.");
            }
        }

        /// <summary>
        /// Copies an existing file to a new file. Overwriting a file of the same name is not allowed.
        /// </summary>
        /// <param name="sourceFileName">Path to source file.</param>
        /// <param name="destFileName">Path to destination file.</param>
        public override void Copy(string sourceFileName, string destFileName)
        {
            this.Copy(sourceFileName, destFileName, false);
        }

        /// <summary>
        /// Copies an existing file to a new file. Overwriting a file of the same name is allowed.
        /// </summary>
        /// <param name="sourceFileName">Path to source file.</param>
        /// <param name="destFileName">Path to destination file.</param>
        /// <param name="overwrite">If destination file should be overwritten.</param>
        public override void Copy(string sourceFileName, string destFileName, bool overwrite)
        {
            if (!this.Exists(sourceFileName))
            {
                throw GetFileNotFoundException(sourceFileName);
            }
            bool destExists = CMS.IO.File.Exists(destFileName);
            if (destExists && !overwrite)
            {
                return;
            }
            if (!StorageHelper.IsSameStorageProvider(sourceFileName, destFileName))
            {
                StorageHelper.CopyFileAcrossProviders(sourceFileName, destFileName);
            }
            else
            {
                IS3ObjectInfo sourceInfo = S3ObjectFactory.GetInfo(sourceFileName);
                IS3ObjectInfo destInfo = S3ObjectFactory.GetInfo(destFileName);
                if (destExists)
                {
                    Provider.DeleteObject(destInfo);
                }
                if (Provider.ObjectExists(sourceInfo))
                {
                    Provider.CopyObjects(sourceInfo, destInfo);
                }
                else
                {
                    Provider.PutFileToObject(destInfo, sourceFileName);
                }
                IS3ObjectInfo destDirectoryInfo = S3ObjectFactory.GetInfo(CMS.IO.Path.GetDirectoryName(destFileName));
                destDirectoryInfo.Key = $"{destDirectoryInfo.Key}/";
                if (!Provider.ObjectExists(destDirectoryInfo))
                {
                    Provider.CreateEmptyObject(destDirectoryInfo);
                }
                var now = DateTime.Now;
                destDirectoryInfo.SetMetadata(S3ObjectInfoProvider.LAST_WRITE_TIME, S3ObjectInfoProvider.GetDateTimeString(now));
                destInfo.SetMetadata(S3ObjectInfoProvider.LAST_WRITE_TIME, S3ObjectInfoProvider.GetDateTimeString(now), false);
                destInfo.SetMetadata(S3ObjectInfoProvider.CREATION_TIME, S3ObjectInfoProvider.GetDateTimeString(now));
            }
        }

        /// <summary>
        /// Opens a binary file, reads the contents of the file into a byte array, and then closes the file.
        /// </summary>
        /// <param name="path">Path to file.</param>
        public override byte[] ReadAllBytes(string path)
        {
            if (!this.Exists(path))
            {
                throw GetFileNotFoundException(path);
            }
            IS3ObjectInfo info = S3ObjectFactory.GetInfo(path);
            if (!File.Provider.ObjectExists(info))
            {
                return System.IO.File.ReadAllBytes(path);
            }
            System.IO.Stream objectContent = Provider
                .GetObjectContent(info, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read, 4096);
            byte[] buffer = new byte[objectContent.Length];
            objectContent.Seek(0L, SeekOrigin.Begin);
            objectContent.Read(buffer, 0, ValidationHelper.GetInteger(objectContent.Length, 0));
            objectContent.Close();
            return buffer;
        }

        /// <summary>Creates or overwrites a file in the specified path.</summary>
        /// <param name="path">Path to file.</param>
        public override CMS.IO.FileStream Create(string path)
        {
            return this.GetFileStream(path, CMS.IO.FileMode.Create);
        }

        /// <summary>
        /// Moves a specified file to a new location, providing the option to specify a new file name.
        /// </summary>
        /// <param name="sourceFileName">Source file name.</param>
        /// <param name="destFileName">Destination file name.</param>
        public override void Move(string sourceFileName, string destFileName)
        {
            if (!StorageHelper.IsSameStorageProvider(sourceFileName, destFileName))
            {
                StorageHelper.MoveFileAcrossProviders(sourceFileName, destFileName);
            }
            else
            {
                this.Copy(sourceFileName, destFileName, false);
                this.Delete(sourceFileName);
            }
        }

        /// <summary>
        /// Opens a text file, reads all lines of the file, and then closes the file.
        /// </summary>
        /// <param name="path">Path to file.</param>
        public override string ReadAllText(string path)
        {
            if (!this.Exists(path))
            {
                throw GetFileNotFoundException(path);
            }
            IS3ObjectInfo info = S3ObjectFactory.GetInfo(path);
            if (!Provider.ObjectExists(info))
            {
                return System.IO.File.ReadAllText(path);
            }
            using (CMS.IO.StreamReader streamReader =
                CMS.IO.StreamReader.New(Provider
                .GetObjectContent(info, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read, 4096)
                ))
            {
                return streamReader.ReadToEnd();
            }
        }

        /// <summary>
        /// Opens a text file, reads all lines of the file, and then closes the file.
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <param name="encoding">The character encoding to use</param>
        public override string ReadAllText(string path, Encoding encoding)
        {
            return this.ReadAllText(path);
        }

        /// <summary>
        /// Creates a new file, write the contents to the file, and then closes the file. If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <param name="contents">Content to write.</param>
        public override void WriteAllText(string path, string contents)
        {
            string directoryName = CMS.IO.Path.GetDirectoryName(path);
            if (!CMS.IO.Directory.Exists(directoryName))
            {
                throw GetDirectoryNotFoundException(directoryName);
            }
            IS3ObjectInfo info = S3ObjectFactory.GetInfo(path);
            File.Provider.PutTextToObject(info, contents);
            info.SetMetadata(S3ObjectInfoProvider.LAST_WRITE_TIME, S3ObjectInfoProvider.GetDateTimeString(DateTime.Now));
        }

        /// <summary>
        /// Creates a new file, write the contents to the file, and then closes the file. If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <param name="contents">Content to write</param>
        /// <param name="encoding">The character encoding to use</param>
        public override void WriteAllText(string path, string contents, Encoding encoding)
        {
            byte[] bytes = encoding.GetBytes(contents);
            this.WriteAllBytes(path, bytes);
        }

        /// <summary>
        /// Opens a file, appends the specified string to the file, and then closes the file. If the file does not exist, this method creates a file, writes the specified string to the file, then closes the file.
        /// </summary>
        /// <param name="path">Path</param>
        /// <param name="contents">Content to write.</param>
        public override void AppendAllText(string path, string contents)
        {
            string directoryName = CMS.IO.Path.GetDirectoryName(path);
            if (!CMS.IO.Directory.Exists(directoryName))
            {
                throw GetDirectoryNotFoundException(directoryName);
            }
            IS3ObjectInfo info = S3ObjectFactory.GetInfo(path);
            File.Provider.AppendTextToObject(info, contents);
            info.SetMetadata(S3ObjectInfoProvider.LAST_WRITE_TIME, S3ObjectInfoProvider.GetDateTimeString(DateTime.Now));
        }

        /// <summary>
        /// Opens a file, appends the specified string to the file, and then closes the file. If the file does not exist, this method creates a file, writes the specified string to the file, then closes the file.
        /// </summary>
        /// <param name="path">Path</param>
        /// <param name="contents">Content to write.</param>
        /// <param name="encoding">The character encoding to use</param>
        public override void AppendAllText(string path, string contents, Encoding encoding)
        {
            byte[] bytes = encoding.GetBytes(contents);
            this.WriteAllBytes(path, bytes);
        }

        /// <summary>
        /// Creates a new file, writes the specified byte array to the file, and then closes the file. If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="path">Path to file.</param>
        /// <param name="bytes">Bytes to write.</param>
        public override void WriteAllBytes(string path, byte[] bytes)
        {
            string directoryName = CMS.IO.Path.GetDirectoryName(path);
            if (!CMS.IO.Directory.Exists(directoryName))
            {
                throw GetDirectoryNotFoundException(directoryName);
            }
            var memoryStream = new System.IO.MemoryStream(bytes);
            IS3ObjectInfo info = S3ObjectFactory.GetInfo(path);
            Provider.PutDataFromStreamToObject(info, memoryStream);
            info.SetMetadata(S3ObjectInfoProvider.LAST_WRITE_TIME, S3ObjectInfoProvider.GetDateTimeString(DateTime.Now));
        }

        /// <summary>Opens an existing file for reading.</summary>
        /// <param name="path">Path to file.</param>
        public override CMS.IO.FileStream OpenRead(string path)
        {
            if (!this.Exists(path))
            {
                throw GetFileNotFoundException(path);
            }
            return this.GetFileStream(path, CMS.IO.FileMode.Open, CMS.IO.FileAccess.Read);
        }

        /// <summary>
        /// Sets the specified FileAttributes  of the file on the specified path.
        /// </summary>
        /// <param name="path">Path to file.</param>
        /// <param name="fileAttributes">File attributes.</param>
        public override void SetAttributes(string path, CMS.IO.FileAttributes fileAttributes)
        {
            if (!this.Exists(path))
            {
                throw GetFileNotFoundException(path);
            }
            IS3ObjectInfo info = S3ObjectFactory.GetInfo(path);
            if (File.Provider.ObjectExists(info))
            {
                info.SetMetadata(S3ObjectInfoProvider.ATTRIBUTES, ValidationHelper.GetString(ValidationHelper.GetInteger(fileAttributes, 0), string.Empty), false);
                info.SetMetadata(S3ObjectInfoProvider.LAST_WRITE_TIME, S3ObjectInfoProvider.GetDateTimeString(DateTime.Now));
            }
            else
            {
                throw new InvalidOperationException($"Cannot set attributes to file '{path}' because it exists only in application file system. \r\n                    This exception typically occurs when file system is mapped to Amazon S3 storage after the file or directory\r\n                    '{path}' was created in the local file system. To fix this issue move given file to Amazon S3 storage.");
            }
        }

        /// <summary>
        /// Opens a FileStream  on the specified path, with the specified mode and access.
        /// </summary>
        /// <param name="path">Path to file.</param>
        /// <param name="mode">File mode.</param>
        /// <param name="access">File access.</param>
        public override CMS.IO.FileStream Open(string path, CMS.IO.FileMode mode, CMS.IO.FileAccess access)
        {
            return new FileStream(path, mode, access);
        }

        /// <summary>
        /// Sets the date and time, in coordinated universal time (UTC), that the specified file was last written to.
        /// </summary>
        /// <param name="path">Path.</param>
        /// <param name="lastWriteTimeUtc">Specified time.</param>
        public override void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
            this.SetLastWriteTime(path, TimeZoneHelper.ConvertTimeToUTC(lastWriteTimeUtc, TimeZoneHelper.ServerTimeZone));
        }

        /// <summary>Creates or opens a file for writing UTF-8 encoded text.</summary>
        /// <param name="path">Path to file.</param>
        public override CMS.IO.StreamWriter CreateText(string path)
        {
            S3ObjectFactory.GetInfo(path);
            return CMS.IO.StreamWriter.New(this.GetFileStream(path, CMS.IO.FileMode.Create));
        }

        /// <summary>
        /// Gets a FileSecurity object that encapsulates the access control list (ACL) entries for a specified directory.
        /// </summary>
        /// <param name="path">Path to file.</param>
        public override FileSecurity GetAccessControl(string path)
        {
            return new FileSecurity();
        }

        /// <summary>
        /// Returns the date and time the specified file or directory was last written to.
        /// </summary>
        /// <param name="path">Path to file.</param>
        public override DateTime GetLastWriteTime(string path)
        {
            if (!this.Exists(path))
            {
                throw GetFileNotFoundException(path);
            }
            IS3ObjectInfo info = S3ObjectFactory.GetInfo(path);
            if (Provider.ObjectExists(info))
            {
                return S3ObjectInfoProvider.GetStringDateTime(info.GetMetadata(S3ObjectInfoProvider.LAST_WRITE_TIME));
            }
            return System.IO.File.GetLastAccessTime(path);
        }

        /// <summary>
        /// Sets the date and time that the specified file was last written to.
        /// </summary>
        /// <param name="path">Path to file.</param>
        /// <param name="lastWriteTime">Last write time.</param>
        public override void SetLastWriteTime(string path, DateTime lastWriteTime)
        {
            if (!this.Exists(path))
            {
                throw GetFileNotFoundException(path);
            }
            IS3ObjectInfo info = S3ObjectFactory.GetInfo(path);
            if (Provider.ObjectExists(info))
            {
                info.SetMetadata(S3ObjectInfoProvider.LAST_WRITE_TIME, S3ObjectInfoProvider.GetDateTimeString(lastWriteTime));
            }
            else
            {
                throw new InvalidOperationException($"Cannot last write time to file '{path}' because is located only in application file system. \r\n                    This exception typically occurs when file system is mapped to Amazon S3 storage after the file or directory\r\n                    '{path}' was created in the local file system. To fix this issue move given file to Amazon S3 storage.");
            }
        }

        /// <summary>
        /// Returns URL to file. If can be accessed directly then direct URL is generated else URL with GetFile page is generated.
        /// </summary>
        /// <param name="path">Virtual path starting with ~ or absolute path.</param>
        /// <param name="siteName">Site name.</param>
        public override string GetFileUrl(string path, string siteName)
        {
            if (!this.ExistsInS3Storage(path))
            {
                return null;
            }
            string objectKeyFromPath = PathHelper.GetObjectKeyFromPath(path);
            if (AmazonHelper.PublicAccess)
            {
                return $"{AmazonHelper.EndPoint}/{objectKeyFromPath}".ToLowerCSafe();
            }
            string downloadPath = AmazonHelper.GetDownloadPath(objectKeyFromPath);
            return this.ResolveUrl(downloadPath);
        }

        private static FileNotFoundException GetFileNotFoundException(string path)
        {
            return new FileNotFoundException($"Path {path} does not exist.");
        }

        private static DirectoryNotFoundException GetDirectoryNotFoundException(string directoryPath)
        {
            return new DirectoryNotFoundException($"Directory '{directoryPath}' does not exist.");
        }

        /// <summary>Returns whether given file exist in file system.</summary>
        /// <param name="path">Path to given file.</param>
        protected virtual bool ExistsInFileSystem(string path)
        {
            return System.IO.File.Exists(path);
        }

        /// <summary>Returns whether given file exists in S3 storage.</summary>
        /// <param name="path">Path to file.</param>
        private bool ExistsInS3Storage(string path)
        {
            return Provider.ObjectExists(S3ObjectFactory.GetInfo(path));
        }

        /// <summary>Returns new instance of FileStream class.</summary>
        /// <param name="path">Path.</param>
        /// <param name="mode">File mode.</param>
        protected virtual CMS.IO.FileStream GetFileStream(string path, CMS.IO.FileMode mode)
        {
            return new FileStream(path, mode);
        }

        /// <summary>Returns new instance of FileStream class.</summary>
        /// <param name="path">Path.</param>
        /// <param name="mode">File mode.</param>
        /// <param name="access">File access.</param>
        protected virtual CMS.IO.FileStream GetFileStream(string path, CMS.IO.FileMode mode, CMS.IO.FileAccess access)
        {
            return new FileStream(path, mode, access);
        }

        /// <summary>
        /// Returns Hashed string from specified query string. Hashed string is not user specific.
        /// </summary>
        /// <param name="query">Query to hash.</param>
        protected virtual string GetHashString(string query)
        {
            return ValidationHelper.GetHashString(query, new HashSettings());
        }

        /// <summary>Returns resolved URL.</summary>
        /// <param name="url">URL to resolve.</param>
        protected virtual string ResolveUrl(string url)
        {
            return URLHelper.ResolveUrl(url, false);
        }
    }
}
