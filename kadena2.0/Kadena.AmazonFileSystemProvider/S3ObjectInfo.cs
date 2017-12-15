using Amazon.S3;
using Amazon.S3.Model;
using CMS.Helpers;
using CMS.IO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;

namespace Kadena.AmazonFileSystemProvider
{
    /// <summary>Represents S3 object.</summary>
    public class S3ObjectInfo : IS3ObjectInfo
    {
        /// <summary>Extension for metadata objects.</summary>
        public const string METADATA_EXT = ".meta";
        /// <summary>Metadata folder.</summary>
        public const string METADATA_FOLDER = "__metadata/";
        /// <summary>Storage key for Amazon storage objects.</summary>
        public const string STORAGE_KEY = "AmazonStorage|S3ObjectInfo|";
        private Dictionary<string, string> mMetadata;

        /// <summary>
        /// Initializes new instance of S3 object info with specified bucket name.
        /// </summary>
        /// <param name="path">Path to the file name.</param>
        public S3ObjectInfo(string path)
          : this(path, false)
        {
        }

        /// <summary>
        /// Initializes new instance of S3 object info with specified bucket name.
        /// </summary>
        /// <param name="path">Path with file name.</param>
        /// <param name="key">Specifies that given path is already object key.</param>
        public S3ObjectInfo(string path, bool key)
        {
            this.Key = key ? path : PathHelper.GetObjectKeyFromPath(path);
            if (path == null || !path.EndsWith("\\", StringComparison.Ordinal) || this.Key.EndsWith("/", StringComparison.Ordinal))
                return;
            this.Key += "/";
        }

        /// <summary>Returns bucket name.</summary>
        private string BucketName
        {
            get
            {
                return S3ObjectInfoProvider.GetBucketName(PathHelper.GetPathFromObjectKey(this.Key, true));
            }
        }

        /// <summary>Returns S3 client instance.</summary>
        private AmazonS3Client S3Client
        {
            get
            {
                return AccountInfo.Current.S3Client;
            }
        }

        /// <summary>Returns provider object.</summary>
        private IS3ObjectInfoProvider Provider
        {
            get
            {
                return S3ObjectInfoProvider.Current;
            }
        }

        /// <summary>Gets or sets collection with metadata.</summary>
        private Dictionary<string, string> Metadata
        {
            get
            {
                if (this.mMetadata == null)
                {
                    this.mMetadata = AbstractStockHelper<RequestStockHelper>.GetItem("AmazonStorage|S3ObjectInfo|", this.Key + "|Metadata", false) as Dictionary<string, string>;
                    if (this.mMetadata == null)
                        this.mMetadata = new Dictionary<string, string>();
                }
                return this.mMetadata;
            }
            set
            {
                this.mMetadata = value;
                AbstractStockHelper<RequestStockHelper>.AddToStorage("AmazonStorage|S3ObjectInfo|", this.Key + "|Metadata", (object)this.mMetadata, false);
            }
        }

        /// <summary>Gets or sets object key.</summary>
        public string Key { get; set; }

        /// <summary>Returns whether current object is locked.</summary>
        public bool IsLocked
        {
            get
            {
                return CMS.Helpers.ValidationHelper.GetBoolean((object)this.GetMetadata("Lock"), false, (CultureInfo)null);
            }
        }

        /// <summary>Returns E-tag from the object.</summary>
        public string ETag
        {
            get
            {
                return this.GetETag();
            }
            set
            {
                AbstractStockHelper<RequestStockHelper>.AddToStorage("AmazonStorage|S3ObjectInfo|", this.Key + "|ETag", (object)value, false);
            }
        }

        /// <summary>Gets whether current object is directory.</summary>
        public bool IsDirectory
        {
            get
            {
                return this.Key.EndsWith("/", StringComparison.Ordinal);
            }
        }

        /// <summary>Gets or sets content length of the object.</summary>
        public long Length
        {
            get
            {
                return this.GetLength();
            }
            set
            {
                AbstractStockHelper<RequestStockHelper>.AddToStorage("AmazonStorage|S3ObjectInfo|", this.Key + "|Length", (object)value, false);
            }
        }

        /// <summary>Sets meta data to object.</summary>
        /// <param name="key">MetaData key.</param>
        /// <param name="value">Metadata value.</param>
        public void SetMetadata(string key, string value)
        {
            this.SetMetadata(key, value, true);
        }

        /// <summary>Sets meta data to object.</summary>
        /// <param name="key">MetaData key.</param>
        /// <param name="value">Metadata value.</param>
        /// <param name="update">Indicates whether data are updated in S3 storage.</param>
        public void SetMetadata(string key, string value, bool update)
        {
            this.SetMetadata(key, value, update, true);
        }

        /// <summary>Sets meta data to object.</summary>
        /// <param name="key">MetaData key.</param>
        /// <param name="value">Metadata value.</param>
        /// <param name="update">Indicates whether data are updated in S3 storage.</param>
        /// <param name="log">Indicates whether is operation logged.</param>
        public void SetMetadata(string key, string value, bool update, bool log)
        {
            if (this.Metadata.ContainsKey(key))
                this.Metadata[key] = value;
            else
                this.Metadata.Add(key, value);
            if (update)
                this.SaveMetadata(PathHelper.GetPathFromObjectKey(this.Key, true), this.Metadata);
            if (!log)
                return;
            FileDebug.LogFileOperation(PathHelper.GetPathFromObjectKey(this.Key, true), nameof(SetMetadata), "Amazon");
        }

        /// <summary>Returns object meta data.</summary>
        /// <param name="key">Metadata key.</param>
        public string GetMetadata(string key)
        {
            this.FetchMetadata();
            if (this.Metadata.ContainsKey(key))
                return this.Metadata[key];
            return (string)null;
        }

        /// <summary>Deletes metadata file.</summary>
        public void DeleteMetadataFile()
        {
            this.S3Client.DeleteObject(new DeleteObjectRequest()
            {
                BucketName = this.BucketName,
                Key = "__metadata/" + this.Key + ".meta"
            });
        }

        /// <summary>Locks current object.</summary>
        public void Lock()
        {
            if (!this.Provider.ObjectExists((IS3ObjectInfo)this))
                return;
            FileDebug.LogFileOperation(PathHelper.GetPathFromObjectKey(this.Key, true), nameof(Lock), "Amazon");
            this.SetMetadata(nameof(Lock), "True", true, false);
        }

        /// <summary>Unlocks current object.</summary>
        public void UnLock()
        {
            if (!this.Provider.ObjectExists((IS3ObjectInfo)this))
                return;
            FileDebug.LogFileOperation(PathHelper.GetPathFromObjectKey(this.Key, true), "Unlock", "Amazon");
            this.SetMetadata("Lock", "False", true, false);
        }

        /// <summary>Returns whether object exists.</summary>
        public bool Exists()
        {
            if (string.IsNullOrEmpty(this.Key))
                return false;
            this.FetchMetadata();
            return CMS.Helpers.ValidationHelper.GetBoolean(AbstractStockHelper<RequestStockHelper>.GetItem("AmazonStorage|S3ObjectInfo|", this.Key + "|Exists", false), false, (CultureInfo)null);
        }

        /// <summary>Returns E-tag of the current object.</summary>
        private string GetETag()
        {
            string key = this.Key + "|ETag";
            if (!AbstractStockHelper<RequestStockHelper>.Contains("AmazonStorage|S3ObjectInfo|", key, false))
                this.FetchMetadata();
            return CMS.Helpers.ValidationHelper.GetString(AbstractStockHelper<RequestStockHelper>.GetItem("AmazonStorage|S3ObjectInfo|", key, false), string.Empty, (CultureInfo)null);
        }

        /// <summary>Returns content length of the current object.</summary>
        private long GetLength()
        {
            string key = this.Key + "|Length";
            if (!AbstractStockHelper<RequestStockHelper>.Contains("AmazonStorage|S3ObjectInfo|", key, false))
                this.FetchMetadata();
            return CMS.Helpers.ValidationHelper.GetLong(AbstractStockHelper<RequestStockHelper>.GetItem("AmazonStorage|S3ObjectInfo|", key, false), 0L, (CultureInfo)null);
        }

        /// <summary>
        /// Downloads metadata from the cloud. It ensures that RequestStockHelper always contains the "Exists" key.
        /// </summary>
        private void FetchMetadata()
        {
            if (AbstractStockHelper<RequestStockHelper>.Contains("AmazonStorage|S3ObjectInfo|", this.Key + "|Exists", false))
                return;
            GetObjectMetadataRequest request = new GetObjectMetadataRequest();
            request.BucketName = this.BucketName;
            request.Key = this.Key;
            try
            {
                GetObjectMetadataResponse objectMetadata = this.S3Client.GetObjectMetadata(request);
                AbstractStockHelper<RequestStockHelper>.AddToStorage("AmazonStorage|S3ObjectInfo|", this.Key + "|Length", (object)objectMetadata.ContentLength, false);
                AbstractStockHelper<RequestStockHelper>.AddToStorage("AmazonStorage|S3ObjectInfo|", this.Key + "|ETag", (object)objectMetadata.ETag, false);
                AbstractStockHelper<RequestStockHelper>.AddToStorage("AmazonStorage|S3ObjectInfo|", this.Key + "|Exists", (object)true, false);
                this.Metadata = this.LoadMetadata(PathHelper.GetPathFromObjectKey(this.Key, true));
                if (!this.Metadata.ContainsKey("LastWriteTime"))
                    this.Metadata.Add("LastWriteTime", CMS.Helpers.ValidationHelper.GetString((object)objectMetadata.LastModified, string.Empty, "en-us"));
                FileDebug.LogFileOperation(PathHelper.GetPathFromObjectKey(this.Key, true), nameof(FetchMetadata), "Amazon");
            }
            catch (AmazonS3Exception ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                    AbstractStockHelper<RequestStockHelper>.AddToStorage("AmazonStorage|S3ObjectInfo|", this.Key + "|Exists", (object)false, false);
                else
                    throw;
            }
        }

        /// <summary>Saves metadata to special file into S3 storage.</summary>
        /// <param name="path">Path.</param>
        /// <param name="metadata">Metadata.</param>
        private void SaveMetadata(string path, Dictionary<string, string> metadata)
        {
            string path1 = path + ".meta";
            string empty = string.Empty;
            foreach (KeyValuePair<string, string> keyValuePair in metadata)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append((object)keyValuePair.Key, (object)";", (object)keyValuePair.Value, (object)"#");
                empty += sb.ToString();
            }
            this.S3Client.PutObject(new PutObjectRequest()
            {
                BucketName = this.BucketName,
                ContentBody = empty,
                Key = "__metadata/" + PathHelper.GetObjectKeyFromPath(path1)
            });
        }

        /// <summary>Loads metadata from special file from S3 storage.</summary>
        /// <param name="path">Path.</param>
        private Dictionary<string, string> LoadMetadata(string path)
        {
            string path1 = path + ".meta";
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            S3ObjectInfo s3ObjectInfo = new S3ObjectInfo(path1);
            GetObjectRequest request = new GetObjectRequest();
            request.BucketName = this.BucketName;
            request.Key = "__metadata/" + PathHelper.GetObjectKeyFromPath(path1);
            if (path1.StartsWith(Directory.CurrentDirectory, StringComparison.Ordinal))
                path1 = path1.Substring(Directory.CurrentDirectory.Length);
            string str1 = CMS.IO.Path.Combine(PathHelper.TempPath, "__metadata" + path1);
            try
            {
                using (GetObjectResponse getObjectResponse = this.S3Client.GetObject(request))
                    getObjectResponse.WriteResponseStreamToFile(str1);
            }
            catch (AmazonS3Exception ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                    return dictionary;
            }
            string str2 = System.IO.File.ReadAllText(str1);
            char[] separator = new char[1] { '#' };
            int num = 1;
            foreach (string str3 in str2.Split(separator, (StringSplitOptions)num))
            {
                char[] chArray = new char[1] { ';' };
                string[] strArray = str3.Split(chArray);
                dictionary.Add(strArray[0], strArray[1]);
            }
            return dictionary;
        }
    }
}