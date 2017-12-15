using CMS.Helpers;
using System;
using System.Globalization;

namespace Kadena.AmazonFileSystemProvider
{
    /// <summary>Implementation of FileInfo class for Amazon S3 object.</summary>
    public class FileInfo : CMS.IO.FileInfo
    {
        private string mExtension = string.Empty;
        private string mFullName = string.Empty;
        private string mName = string.Empty;
        private DateTime mLastWriteTime = DateTimeHelper.ZERO_TIME;
        private DateTime mCreationTime = DateTimeHelper.ZERO_TIME;
        private bool mExists;
        private CMS.IO.FileAttributes mAttributes;
        private DirectoryInfo mDirectory;
        private long mLength;
        private IS3ObjectInfo obj;
        private bool existsInS3Storage;

        /// <summary>Returns S3Object provider.</summary>
        private IS3ObjectInfoProvider Provider
        {
            get
            {
                return S3ObjectFactory.Provider;
            }
        }

        /// <summary>Initializes new instance of FileInfo class.</summary>
        /// <param name="filename">File name.</param>
        public FileInfo(string filename)
        {
            this.mExtension = CMS.IO.Path.GetExtension(filename);
            this.mFullName = filename;
            this.mName = CMS.IO.Path.GetFileName(filename);
            this.mExists = CMS.IO.File.Exists(filename);
            this.IsReadOnly = false;
            this.Attributes = CMS.IO.FileAttributes.Normal;
            this.obj = S3ObjectFactory.GetInfo(filename);
            if (!this.Provider.ObjectExists(this.obj))
            {
                if (System.IO.File.Exists(filename))
                    this.mSystemInfo = new System.IO.FileInfo(filename);
            }
            else
            {
                this.mExists = true;
                this.existsInS3Storage = true;
            }
            this.InitCMSValues();
        }

        /// <summary>Length of file.</summary>
        public override long Length
        {
            get
            {
                return this.mLength;
            }
        }

        /// <summary>File extension.</summary>
        public override string Extension
        {
            get
            {
                return this.mExtension;
            }
        }

        /// <summary>Full name of file (with whole path).</summary>
        public override string FullName
        {
            get
            {
                return this.mFullName;
            }
        }

        /// <summary>File name of file (without path).</summary>
        public override string Name
        {
            get
            {
                return this.mName;
            }
        }

        /// <summary>Last write time to file.</summary>
        public override DateTime LastWriteTime
        {
            get
            {
                return this.mLastWriteTime;
            }
            set
            {
                this.mLastWriteTime = value;
                if (!this.existsInS3Storage)
                    return;
                this.obj.SetMetadata(nameof(LastWriteTime), S3ObjectInfoProvider.GetDateTimeString(this.mLastWriteTime));
            }
        }

        /// <summary>If file exists.</summary>
        public override bool Exists
        {
            get
            {
                return this.mExists;
            }
        }

        /// <summary>Creation date of file.</summary>
        public override DateTime CreationTime
        {
            get
            {
                return this.mCreationTime;
            }
            set
            {
                this.mCreationTime = value;
                if (!this.existsInS3Storage)
                    return;
                this.obj.SetMetadata(nameof(CreationTime), S3ObjectInfoProvider.GetDateTimeString(this.mCreationTime));
            }
        }

        /// <summary>Directory of file.</summary>
        public override CMS.IO.DirectoryInfo Directory
        {
            get
            {
                return (CMS.IO.DirectoryInfo)this.mDirectory;
            }
        }

        /// <summary>If is read only.</summary>
        public override bool IsReadOnly { get; set; }

        /// <summary>File attributes.</summary>
        public override CMS.IO.FileAttributes Attributes
        {
            get
            {
                return this.mAttributes;
            }
            set
            {
                this.mAttributes = value;
                if (!this.existsInS3Storage)
                    return;
                this.obj.SetMetadata(nameof(Attributes), ValidationHelper.GetString((object)ValidationHelper.GetInteger((object)this.mAttributes, 0, (CultureInfo)null), string.Empty, (CultureInfo)null));
            }
        }

        /// <summary>Directory name.</summary>
        public override string DirectoryName
        {
            get
            {
                return this.Directory.Name;
            }
        }

        /// <summary>Last access time.</summary>
        public override DateTime LastAccessTime
        {
            get
            {
                return S3ObjectInfoProvider.GetStringDateTime(this.obj.GetMetadata("LastWriteTime"));
            }
            set
            {
                if (!this.existsInS3Storage)
                    return;
                this.obj.SetMetadata("LastWriteTime", S3ObjectInfoProvider.GetDateTimeString(value));
            }
        }

        /// <summary>Creates or opens a file for writing UTF-8 encoded text.</summary>
        protected override CMS.IO.StreamWriter CreateTextInternal()
        {
            this.mExists = true;
            this.existsInS3Storage = true;
            return CMS.IO.StreamWriter.New((System.IO.Stream)CMS.IO.FileStream.New(this.FullName, CMS.IO.FileMode.Create));
        }

        /// <summary>Deletes file.</summary>
        protected override void DeleteInternal()
        {
            this.Provider.DeleteObject(this.obj);
            this.mExists = false;
            this.existsInS3Storage = false;
        }

        /// <summary>Creates a read-only ICMSFileStream.</summary>
        protected override CMS.IO.FileStream OpenReadInternal()
        {
            return (CMS.IO.FileStream)new FileStream(this.FullName, CMS.IO.FileMode.Open);
        }

        /// <summary>Copies current file to destination.</summary>
        /// <param name="destFileName">Destination file name.</param>
        /// <param name="overwrite">Specifies whether destination file is overwritten.</param>
        protected override CMS.IO.FileInfo CopyToInternal(string destFileName, bool overwrite)
        {
            CMS.IO.File.Copy(this.FullName, destFileName, overwrite);
            return CMS.IO.FileInfo.New(destFileName);
        }

        /// <summary>Moves an existing file to a new file.</summary>
        /// <param name="destFileName">Destination file name.</param>
        protected override void MoveToInternal(string destFileName)
        {
            CMS.IO.File.Move(this.FullName, destFileName);
        }

        /// <summary>
        /// Creates a StreamReader with UTF8 encoding that reads from an existing text file.
        /// </summary>
        protected override CMS.IO.StreamReader OpenTextInternal()
        {
            return CMS.IO.File.OpenText(this.FullName);
        }

        /// <summary>Sets values from System.IO.FileInfo to this file info</summary>
        private void InitCMSValues()
        {
            if (this.mSystemInfo != null)
            {
                this.mExtension = this.mSystemInfo.Extension;
                this.mFullName = this.mSystemInfo.FullName;
                this.mName = this.mSystemInfo.Name;
                this.mExists = this.mSystemInfo.Exists;
                this.LastWriteTime = this.mSystemInfo.LastWriteTime;
                this.CreationTime = this.mSystemInfo.CreationTime;
                this.IsReadOnly = this.mSystemInfo.IsReadOnly;
                this.Attributes = (CMS.IO.FileAttributes)this.mSystemInfo.Attributes;
                if (this.mExists)
                    this.mLength = this.mSystemInfo.Length;
                if (this.mDirectory == null)
                    this.mDirectory = new DirectoryInfo(this.mSystemInfo.Directory.FullName);
                this.mDirectory.CreationTime = this.mSystemInfo.Directory.CreationTime;
                this.mDirectory.Exists = this.mSystemInfo.Directory.Exists;
                this.mDirectory.FullName = this.mSystemInfo.Directory.FullName;
                this.mDirectory.LastWriteTime = this.mSystemInfo.Directory.LastWriteTime;
                this.mDirectory.Name = this.mSystemInfo.Directory.Name;
            }
            else
            {
                if (this.mExists)
                {
                    this.mLastWriteTime = S3ObjectInfoProvider.GetStringDateTime(this.obj.GetMetadata("LastWriteTime"));
                    this.mCreationTime = S3ObjectInfoProvider.GetStringDateTime(this.obj.GetMetadata("CreationTime"));
                    this.mAttributes = (CMS.IO.FileAttributes)ValidationHelper.GetInteger((object)this.obj.GetMetadata("CreationTime"), ValidationHelper.GetInteger((object)CMS.IO.FileAttributes.Normal, 0, (CultureInfo)null), (CultureInfo)null);
                    this.mLength = this.obj.Length;
                }
                else
                {
                    this.LastWriteTime = DateTimeHelper.ZERO_TIME;
                    this.CreationTime = DateTimeHelper.ZERO_TIME;
                    this.mAttributes = CMS.IO.FileAttributes.Normal;
                }
                this.mDirectory = new DirectoryInfo(CMS.IO.Path.GetDirectoryName(this.FullName));
            }
        }

        /// <summary>Converts current info to string.</summary>
        public override string ToString()
        {
            return this.FullName;
        }
    }
}