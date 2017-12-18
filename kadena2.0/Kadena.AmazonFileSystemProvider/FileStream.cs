using Amazon.S3.Model;
using CMS.Core;
using CMS.EventLog;
using CMS.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Kadena.AmazonFileSystemProvider
{
    /// <summary>
    /// Implementation of FileStream class for Amazon simple storage.
    /// </summary>
    public class FileStream : CMS.IO.FileStream, IMultiPartUploadStream
    {
        private CMS.IO.FileMode fileMode = CMS.IO.FileMode.Open;
        private CMS.IO.FileAccess fileAccess = CMS.IO.FileAccess.ReadWrite;
        private CMS.IO.FileShare fileShare = CMS.IO.FileShare.Read;
        private int mReadSize = -1;
        private string mPath;
        private int bufferSize;
        private bool mMultiPartUploadMode;
        private System.IO.FileStream fsTemp;
        private IS3ObjectInfo obj;
        private S3MultiPartUploader mMultiPartUploader;
        private System.IO.FileStream fsStream;
        private bool disposed;

        /// <summary>Returns S3Object provider.</summary>
        private IS3ObjectInfoProvider Provider
        {
            get
            {
                return S3ObjectFactory.Provider;
            }
        }

        /// <summary>
        /// Instance for uploading large files in smaller parts to Amazon S3 storage.
        /// </summary>
        private S3MultiPartUploader MultiPartUploader
        {
            get
            {
                if (this.mMultiPartUploader == null)
                {
                    this.mMultiPartUploader = new S3MultiPartUploader(AccountInfo.Current.S3Client, this.MinimalPartSize, this.MaximalPartSize);
                }
                return this.mMultiPartUploader;
            }
        }

        /// <summary>
        /// Initializes new instance and initializes new system file stream.
        /// </summary>
        /// <param name="path">Path to file.</param>
        /// <param name="mode">File mode.</param>
        public FileStream(string path, CMS.IO.FileMode mode)
          : this(path, mode, mode == CMS.IO.FileMode.Append ? CMS.IO.FileAccess.Write : CMS.IO.FileAccess.ReadWrite)
        {
        }

        /// <summary>
        /// Initializes new instance and initializes new system file stream.
        /// </summary>
        /// <param name="path">Path to file.</param>
        /// <param name="mode">File mode.</param>
        /// <param name="access">File access.</param>
        public FileStream(string path, CMS.IO.FileMode mode, CMS.IO.FileAccess access)
          : this(path, mode, access, CMS.IO.FileShare.Read)
        {
        }

        /// <summary>
        /// Initializes new instance and initializes new system file stream.
        /// </summary>
        /// <param name="path">Path to file.</param>
        /// <param name="mode">File mode.</param>
        /// <param name="access">File access.</param>
        /// <param name="share">Sharing permissions.</param>
        public FileStream(string path, CMS.IO.FileMode mode, CMS.IO.FileAccess access, CMS.IO.FileShare share)
          : this(path, mode, access, share, 4096)
        {
        }

        /// <summary>
        /// Initializes new instance and initializes new system file stream.
        /// </summary>
        /// <param name="path">Path to file.</param>
        /// <param name="mode">File mode.</param>
        /// <param name="access">File access.</param>
        /// <param name="bSize">Buffer size.</param>
        /// <param name="share">Sharing permissions.</param>
        public FileStream(string path, CMS.IO.FileMode mode, CMS.IO.FileAccess access, CMS.IO.FileShare share, int bSize)
          : base(path)
        {
            this.mPath = path;
            this.fileMode = mode;
            this.fileAccess = access;
            this.fileShare = share;
            this.bufferSize = bSize;
            this.InitFileStream();
        }

        /// <summary>
        /// Gets a value indicating whether the current stream supports reading.
        /// </summary>
        public override bool CanRead
        {
            get
            {
                if (this.fsStream != null)
                {
                    return this.fsStream.CanRead;
                }
                return this.fsTemp.CanRead;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current stream supports seeking.
        /// </summary>
        /// <returns>True if the stream supports seeking, false otherwise.</returns>
        public override bool CanSeek
        {
            get
            {
                if (this.fsStream != null)
                {
                    return this.fsStream.CanSeek;
                }
                return this.fsTemp.CanSeek;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current stream supports writing.
        /// </summary>
        public override bool CanWrite
        {
            get
            {
                if (this.fsStream != null)
                {
                    return this.fsStream.CanWrite;
                }
                return this.fsTemp.CanWrite;
            }
        }

        /// <summary>Length of stream.</summary>
        public override long Length
        {
            get
            {
                if (this.fsStream != null)
                {
                    return this.fsStream.Length;
                }
                return this.fsTemp.Length;
            }
        }

        /// <summary>Gets or sets position of current stream.</summary>
        public override long Position
        {
            get
            {
                if (this.fsStream != null)
                {
                    return this.fsStream.Position;
                }
                return this.fsTemp.Position;
            }
            set
            {
                if (this.fsStream != null)
                {
                    this.fsStream.Position = value;
                }
                else
                {
                    this.fsTemp.Position = value;
                }
            }
        }

        /// <summary>
        /// Returns minimal size of the part used in multipart upload process to Amazon S3 storage.
        /// </summary>
        public long MinimalPartSize
        {
            get
            {
                return S3ObjectInfoProvider.MINIMAL_PART_SIZE;
            }
        }

        /// <summary>
        /// Maximal size of the part used in multipart upload process to Amazon S3 storage.
        /// </summary>
        public long MaximalPartSize
        {
            get
            {
                return S3ObjectInfoProvider.MAXIMAL_PART_SIZE;
            }
        }

        /// <summary>Reads data from stream and stores them into array.</summary>
        /// <param name="array">Array where result is stored.</param>
        /// <param name="offset">Offset from file begin.</param>
        /// <param name="count">Number of characters which are read.</param>
        public override int Read(byte[] array, int offset, int count)
        {
            if (this.mReadSize == -1)
            {
                this.LogFileOperation(this.mPath, nameof(Read), count);
                this.mReadSize = count;
            }
            else
            {
                this.mReadSize += count;
            }
            if (this.fsStream != null)
            {
                return this.fsStream.Read(array, offset, count);
            }
            return this.fsTemp.Read(array, offset, count);
        }

        /// <summary>Closes current stream.</summary>
        public override void Close()
        {
            if (this.fsStream != null)
            {
                this.fsStream.Close();
            }
            else
            {
                this.Dispose(true);
                this.fsTemp.Close();
            }
            if (this.mReadSize > 0)
            {
                FileDebug.LogReadEnd(this.mReadSize);
                this.mReadSize = -1;
            }
            this.LogFileOperation(this.mPath, nameof(Close), -1);
        }

        /// <summary>
        /// Clears all buffers for this stream and causes any buffered data to be written to the underlying device.
        /// </summary>
        public override void Flush()
        {
            if (this.fsStream != null && this.fsStream.CanWrite)
            {
                this.fsStream.Flush();
            }
            else
            {
                if (this.fsTemp == null || !this.fsTemp.CanWrite
                    || this.fileAccess == CMS.IO.FileAccess.Read && this.fileMode == CMS.IO.FileMode.Open)
                {
                    return;
                }
                this.fsTemp.Flush();
            }
        }

        /// <summary>Writes sequence of bytes to stream.</summary>
        /// <param name="buffer">Buffer.</param>
        /// <param name="offset">Offset.</param>
        /// <param name="count">Count.</param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (this.fsStream != null)
            {
                throw new Exception($"Cannot write into file because it exists only in application file system. \r\n                    This exception typically occurs when file system is mapped to Amazon S3 storage after the file or directory\r\n                    '{this.mPath}' was created in the local file system. To fix this issue move given file to Amazon S3 storage.");
            }
            this.fsTemp.Write(buffer, offset, count);
            this.LogFileOperation(this.mPath, nameof(Write), count);
        }

        /// <summary>
        /// Sets the position within the current stream to the specified value.
        /// </summary>
        /// <param name="offset">Offset</param>
        /// <param name="loc">Location</param>
        public override long Seek(long offset, SeekOrigin loc)
        {
            if (this.fsStream != null)
            {
                return this.fsStream.Seek(offset, loc);
            }
            return this.fsTemp.Seek(offset, loc);
        }

        /// <summary>Set length to stream.</summary>
        /// <param name="value">Value to set.</param>
        public override void SetLength(long value)
        {
            this.fsTemp.SetLength(value);
        }

        /// <summary>Writes byte to the stream.</summary>
        /// <param name="value">Value to write.</param>
        public override void WriteByte(byte value)
        {
            if (this.fsStream != null)
            {
                throw new Exception($"Cannot write into file because it exists only in application file system. \r\n                    This exception typically occurs when file system is mapped to Amazon S3 storage after the file or directory\r\n                    '{this.mPath}' was created in the local file system. To fix this issue move given file to Amazon S3 storage.");
            }
            this.fsTemp.WriteByte(value);
            this.LogFileOperation(this.mPath, "Write", 1);
        }

        /// <summary>Inits multipart upload for given path.</summary>
        /// <returns>
        /// Upload ID, unique identifier for one multipart upload to Amazon S3 storage.
        /// Returned upload ID is needed for each subsequent multipart upload operation.
        /// </returns>
        public string InitMultiPartUpload()
        {
            this.mMultiPartUploadMode = true;
            return this.MultiPartUploader.InitMultiPartUpload(this.obj.Key, this.obj.BucketName);
        }

        /// <summary>
        /// Uploads stream's content to Amazon S3 storage as one part of the file in multipart upload process
        /// identified by <paramref name="uploadSessionId" />.
        /// </summary>
        /// <remarks>
        /// Always returns one ETag in collection. If stream's length is more than 5GB then exception is thrown.
        /// </remarks>
        /// <param name="uploadSessionId">Unique identifier for one multipart upload. Can be obtained by <see cref="M:CMS.AmazonStorage.FileStream.InitMultiPartUpload" />.</param>
        /// <param name="nextPartNumber">Number that defines position of the data obtained by the stream in the whole multipart upload process.</param>
        /// <returns>One unique identifier of the uploaded part in collection.</returns>
        public IEnumerable<string> UploadStreamContentAsMultiPart(string uploadSessionId, int nextPartNumber)
        {
            if (this.Length > this.MaximalPartSize)
            {
                throw new Exception($"Maximal size of part for upload to Amazon S3 storage is {this.MaximalPartSize} current stream has length {this.Length}.");
            }
            this.mMultiPartUploadMode = true;
            if (this.obj.IsLocked)
            {
                throw new Exception($"Couldn't upload part of the object {this.obj.Key} because it is used by another process.");
            }
            this.obj.Lock();
            this.obj.Length += this.Length;
            List<string> stringList = new List<string>();
            this.MultiPartUploader.UploadPartFromStream(uploadSessionId, this.obj.Key, this.obj.BucketName, nextPartNumber, this);
            this.obj.UnLock();
            S3ObjectInfoProvider.RemoveRequestCache(this.obj.Key);
            return stringList;
        }

        /// <summary>
        /// Uploads one large file to Amazon S3 storage in smaller parts.
        /// </summary>
        /// <remarks>Stream still needs to be disposed.</remarks>
        /// <param name="uploadSessionId">Unique identifier for one multipart upload. Can be obtained by <see cref="M:CMS.AmazonStorage.FileStream.InitMultiPartUpload" /> method.</param>
        /// <param name="partIdentifiers">List of identifiers from Amazon S3 received after uploading each part by <see cref="M:CMS.AmazonStorage.FileStream.UploadStreamContentAsMultiPart(System.String,System.Int32)" /> method.</param>
        /// <returns>ETag of the uploaded file.</returns>
        public string CompleteMultiPartUploadProcess(string uploadSessionId, IEnumerable<string> partIdentifiers)
        {
            this.mMultiPartUploadMode = true;
            List<UploadPartResponse> uploadPartResponseList = partIdentifiers.Select((id, index) => new UploadPartResponse
            {
                PartNumber = index + 1,
                ETag = id
            }).ToList();

            if (this.obj.IsLocked)
            {
                throw new Exception($"Couldn't upload part of the object {this.obj.Key} because it is used by another process.");
            }
            this.obj.Lock();
            this.obj.ETag = this.MultiPartUploader.CompleteMultiPartUploadProcess(this.obj.Key, this.obj.BucketName, uploadSessionId, uploadPartResponseList).ETag;
            this.obj.UnLock();
            this.SetLastWriteTimeAndCreationTimeToS3Object();
            S3ObjectInfoProvider.RemoveRequestCache(this.obj.Key);
            return this.obj.ETag;
        }

        /// <summary>
        /// Aborts multipart upload to Amazon S3 storage and removes all resources already uploaded.
        /// </summary>
        /// <param name="uploadSessionId">
        /// Unique identifier for multipart upload process to external storage.
        /// Is obtained by <see cref="M:CMS.AmazonStorage.FileStream.InitMultiPartUpload" />.
        /// </param>
        public void AbortMultiPartUpload(string uploadSessionId)
        {
            S3ObjectInfoProvider.RemoveRequestCache(this.obj.Key);
            this.MultiPartUploader.AbortMultiPartUpload(this.obj.Key, this.obj.BucketName, uploadSessionId);
        }

        /// <summary>
        /// Releases all unmanaged and optionally managed resources.
        /// </summary>
        /// <param name="disposing">When true, managed resources are released.</param>
        protected override void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }
            if (disposing)
            {
                if (this.fsStream != null)
                {
                    this.fsStream.Dispose();
                }
                else
                {
                    try
                    {
                        this.Flush();
                        if (this.fileAccess != CMS.IO.FileAccess.Read
                            && this.fileMode != CMS.IO.FileMode.Open
                            && this.fsTemp.CanWrite
                            && !this.mMultiPartUploadMode)
                        {
                            this.fsTemp.Seek(0L, SeekOrigin.Begin);
                            this.Provider.PutDataFromStreamToObject(this.obj, this.fsTemp);
                            this.SetLastWriteTimeAndCreationTimeToS3Object();
                        }
                    }
                    catch (Exception ex)
                    {
                        EventLogProvider.LogException("Custom Amazon", "STREAMDISPOSE", ex, 0);
                    }
                    finally
                    {
                        this.fsTemp.Dispose();
                        if (this.mReadSize > 0)
                        {
                            FileDebug.LogReadEnd(this.mReadSize);
                            this.mReadSize = -1;
                        }
                        this.LogFileOperation(this.mPath, "Close", -1);
                    }
                }
            }
            this.disposed = true;
            base.Dispose(disposing);
        }

        /// <summary>Initializes file stream object.</summary>
        protected virtual void InitFileStream()
        {
            string path = CMS.IO.Path.Combine(PathHelper.TempPath, PathHelper.GetRelativePath(this.mPath));
            Directory.CreateDiskDirectoryStructure(path);
            this.obj = S3ObjectFactory.GetInfo(this.mPath);
            if (this.Provider.ObjectExists(this.obj))
            {
                if (this.fileMode == CMS.IO.FileMode.CreateNew)
                {
                    throw new Exception("Cannot create a new file, the file is already exist.");
                }
                this.fsTemp = (System.IO.FileStream)this.Provider.GetObjectContent(this.obj, (System.IO.FileMode)this.fileMode,
                    (System.IO.FileAccess)this.fileAccess, (System.IO.FileShare)this.fileShare, this.bufferSize);
                if (this.fileMode == CMS.IO.FileMode.Append)
                {
                    this.fsTemp.Position = this.fsTemp.Length;
                }
            }
            else
            {
                if (System.IO.File.Exists(this.mPath))
                {
                    this.fsStream = new System.IO.FileStream(this.mPath, (System.IO.FileMode)this.fileMode, 
                        (System.IO.FileAccess)this.fileAccess, (System.IO.FileShare)this.fileShare, this.bufferSize);
                }
            }
            if (this.fsTemp != null
                || this.fsStream != null)
            {
                return;
            }
            try
            {
                this.fsTemp = new System.IO.FileStream(path, System.IO.FileMode.Create, 
                    System.IO.FileAccess.ReadWrite, (System.IO.FileShare)this.fileShare, this.bufferSize);
            }
            catch (FileNotFoundException)
            {
            }
        }

        /// <summary>Sets last write time and creation time to S3 object.</summary>
        private void SetLastWriteTimeAndCreationTimeToS3Object()
        {
            string dateTimeString = S3ObjectInfoProvider.GetDateTimeString(DateTime.Now);
            if (this.obj.GetMetadata(S3ObjectInfoProvider.CREATION_TIME) == null)
            {
                this.obj.SetMetadata(S3ObjectInfoProvider.CREATION_TIME, dateTimeString, false);
            }
            this.obj.SetMetadata(S3ObjectInfoProvider.LAST_WRITE_TIME, dateTimeString);
        }
    }
}