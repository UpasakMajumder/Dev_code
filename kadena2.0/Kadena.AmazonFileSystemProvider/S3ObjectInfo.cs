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
            {
                return;
            }
            this.Key += "/";
        }

        /// <summary>Returns bucket name.</summary>
        public string BucketName
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
                    this.mMetadata = AbstractStockHelper<RequestStockHelper>.GetItem(STORAGE_KEY, this.Key + "|Metadata", false) as Dictionary<string, string>;
                    if (this.mMetadata == null)
                    {
                        this.mMetadata = new Dictionary<string, string>();
                    }
                }
                return this.mMetadata;
            }
            set
            {
                this.mMetadata = value;
                AbstractStockHelper<RequestStockHelper>.AddToStorage(STORAGE_KEY, this.Key + "|Metadata", (object)this.mMetadata, false);
            }
        }

        /// <summary>Gets or sets object key.</summary>
        public string Key { get; set; }

        /// <summary>Returns whether current object is locked.</summary>
        public bool IsLocked
        {
            get
            {
                return ValidationHelper.GetBoolean(this.GetMetadata("Lock"), false);
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
                AbstractStockHelper<RequestStockHelper>.AddToStorage(STORAGE_KEY, this.Key + "|ETag", value, false);
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
                AbstractStockHelper<RequestStockHelper>.AddToStorage(STORAGE_KEY, this.Key + "|Length", value, false);
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
            {
                this.Metadata[key] = value;
            }
            else
            {
                this.Metadata.Add(key, value);
            }
            if (update)
            {
                this.SaveMetadata(PathHelper.GetPathFromObjectKey(this.Key, true), this.Metadata);
            }
            if (!log)
            {
                return;
            }
            FileDebug.LogFileOperation(PathHelper.GetPathFromObjectKey(this.Key, true), nameof(SetMetadata), "Custom Amazon");
        }

        /// <summary>Returns object meta data.</summary>
        /// <param name="key">Metadata key.</param>
        public string GetMetadata(string key)
        {
            this.FetchMetadata();
            if (this.Metadata.ContainsKey(key))
            {
                return this.Metadata[key];
            }
            return null;
        }

        /// <summary>Deletes metadata file.</summary>
        public void DeleteMetadataFile()
        {
            this.S3Client.DeleteObject(new DeleteObjectRequest()
            {
                BucketName = this.BucketName,
                Key = $"{METADATA_FOLDER}{this.Key}{METADATA_EXT}"
            });
        }

        /// <summary>Locks current object.</summary>
        public void Lock()
        {
            if (!this.Provider.ObjectExists(this))
            {
                return;
            }
            FileDebug.LogFileOperation(PathHelper.GetPathFromObjectKey(this.Key, true), nameof(Lock), "Custom Amazon");
            this.SetMetadata(nameof(Lock), "True", true, false);
        }

        /// <summary>Unlocks current object.</summary>
        public void UnLock()
        {
            if (!this.Provider.ObjectExists(this))
            {
                return;
            }
            FileDebug.LogFileOperation(PathHelper.GetPathFromObjectKey(this.Key, true), "Unlock", "Custom Amazon");
            this.SetMetadata("Lock", "False", true, false);
        }

        /// <summary>Returns whether object exists.</summary>
        public bool Exists()
        {
            if (string.IsNullOrEmpty(this.Key))
            {
                return false;
            }
            this.FetchMetadata();
            return ValidationHelper.GetBoolean(AbstractStockHelper<RequestStockHelper>.GetItem(STORAGE_KEY, this.Key + "|Exists", false), false);
        }

        /// <summary>Returns E-tag of the current object.</summary>
        private string GetETag()
        {
            string key = this.Key + "|ETag";
            if (!AbstractStockHelper<RequestStockHelper>.Contains(STORAGE_KEY, key, false))
            {
                this.FetchMetadata();
            }
            return ValidationHelper.GetString(AbstractStockHelper<RequestStockHelper>.GetItem(STORAGE_KEY, key, false), string.Empty);
        }

        /// <summary>Returns content length of the current object.</summary>
        private long GetLength()
        {
            string key = this.Key + "|Length";
            if (!AbstractStockHelper<RequestStockHelper>.Contains(STORAGE_KEY, key, false))
            {
                this.FetchMetadata();
            }
            return ValidationHelper.GetLong(AbstractStockHelper<RequestStockHelper>.GetItem(STORAGE_KEY, key, false), 0L);
        }

        /// <summary>
        /// Downloads metadata from the cloud. It ensures that RequestStockHelper always contains the "Exists" key.
        /// </summary>
        private void FetchMetadata()
        {
            if (AbstractStockHelper<RequestStockHelper>.Contains(STORAGE_KEY, this.Key + "|Exists", false))
            {
                return;
            }
            GetObjectMetadataRequest request = new GetObjectMetadataRequest
            {
                BucketName = this.BucketName,
                Key = this.Key
            };
            try
            {
                GetObjectMetadataResponse objectMetadata = this.S3Client.GetObjectMetadata(request);
                AbstractStockHelper<RequestStockHelper>.AddToStorage(STORAGE_KEY, this.Key + "|Length", objectMetadata.ContentLength, false);
                AbstractStockHelper<RequestStockHelper>.AddToStorage(STORAGE_KEY, this.Key + "|ETag", objectMetadata.ETag, false);
                AbstractStockHelper<RequestStockHelper>.AddToStorage(STORAGE_KEY, this.Key + "|Exists", true, false);
                this.Metadata = this.LoadMetadata(PathHelper.GetPathFromObjectKey(this.Key, true));
                if (!this.Metadata.ContainsKey("LastWriteTime"))
                {
                    this.Metadata.Add("LastWriteTime", ValidationHelper.GetString(objectMetadata.LastModified, string.Empty, "en-us"));
                }
                FileDebug.LogFileOperation(PathHelper.GetPathFromObjectKey(this.Key, true), nameof(FetchMetadata), "Custom Amazon");
            }
            catch (AmazonS3Exception ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    AbstractStockHelper<RequestStockHelper>.AddToStorage(STORAGE_KEY, this.Key + "|Exists", false, false);
                }
                else
                    throw;
            }
        }

        /// <summary>Saves metadata to special file into S3 storage.</summary>
        /// <param name="path">Path.</param>
        /// <param name="metadata">Metadata.</param>
        private void SaveMetadata(string path, Dictionary<string, string> metadata)
        {
            string path1 = $"{path}{METADATA_EXT}";
            var sb = new StringBuilder();
            foreach (var keyValuePair in metadata)
            {
                sb.Append($"{keyValuePair.Key};{keyValuePair.Value}#");
            }
            this.S3Client.PutObject(new PutObjectRequest()
            {
                BucketName = this.BucketName,
                ContentBody = sb.ToString(),
                Key = $"{METADATA_FOLDER}{PathHelper.GetObjectKeyFromPath(path1)}"
            });
        }

        /// <summary>Loads metadata from special file from S3 storage.</summary>
        /// <param name="path">Path.</param>
        private Dictionary<string, string> LoadMetadata(string path)
        {
            string path1 = $"{path}{METADATA_EXT}";
            var dictionary = new Dictionary<string, string>();
            var s3ObjectInfo = new S3ObjectInfo(path1);
            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = this.BucketName,
                Key = $"{METADATA_FOLDER}{PathHelper.GetObjectKeyFromPath(path1)}"
            };
            if (path1.StartsWith(Directory.CurrentDirectory, StringComparison.Ordinal))
            {
                path1 = path1.Substring(Directory.CurrentDirectory.Length);
            }
            string str1 = Path.Combine(PathHelper.TempPath, $"{METADATA_FOLDER}{path1}");
            try
            {
                using (GetObjectResponse getObjectResponse = this.S3Client.GetObject(request))
                {
                    getObjectResponse.WriteResponseStreamToFile(str1);
                }
            }
            catch (AmazonS3Exception ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    return dictionary;
                }
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