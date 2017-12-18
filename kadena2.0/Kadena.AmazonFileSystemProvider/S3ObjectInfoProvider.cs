using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using CMS.Helpers;
using CMS.IO;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

namespace Kadena.AmazonFileSystemProvider
{
    /// <summary>Performs operations over the S3 objects.</summary>
    public class S3ObjectInfoProvider : IS3ObjectInfoProvider
    {
        private static IS3ObjectInfoProvider mCurrent = (IS3ObjectInfoProvider)null;
        private static readonly ConcurrentDictionary<string, AutoResetEvent> mS3ObjectEvents = new ConcurrentDictionary<string, AutoResetEvent>();
        /// <summary>
        /// Property name which returns if file is locked for writing.
        /// </summary>
        public const string LOCK = "Lock";
        /// <summary>Last write time field name in storage metadata.</summary>
        public const string LAST_WRITE_TIME = "LastWriteTime";
        /// <summary>Creation time field name in storage metadata.</summary>
        public const string CREATION_TIME = "CreationTime";
        /// <summary>Attributes field name in storage metadata.</summary>
        public const string ATTRIBUTES = "Attributes";
        /// <summary>Storage key for Amazon storage directory objects.</summary>
        private const string STORAGE_KEY = "AmazonStorage|GetObjectList|";
        /// <summary>
        /// Indicates maximum number of items processed by one request.
        /// </summary>
        public const int MAX_OBJECTS_PER_REQUEST = 1000;
        /// <summary>
        /// Recommended minimal size (15 MB) of the file in Bytes for which multipart upload to Amazon S3 storage should be used.
        /// </summary>
        private const long RECOMMENDED_SIZE_FOR_MULTIPART_UPLOAD = 15728640;
        /// <summary>
        /// Minimal size (5 MB) of the parts used for multipart upload.
        /// </summary>
        internal const long MINIMAL_PART_SIZE = 5242880;
        /// <summary>
        /// Maximal size (5 GB) of the parts used for multipart upload.
        /// </summary>
        internal const long MAXIMAL_PART_SIZE = 5242880000;
        private S3MultiPartUploader mMultiPartUploader;

        /// <summary>Returns AmazonS3 class from account info.</summary>
        private AmazonS3Client S3Client
        {
            get
            {
                return AccountInfo.Current.S3Client;
            }
        }

        /// <summary>
        /// Utility for uploading large files in smaller parts to Amazon S3 storage.
        /// </summary>
        private S3MultiPartUploader MultiPartUploader
        {
            get
            {
                if (this.mMultiPartUploader == null)
                    this.mMultiPartUploader = new S3MultiPartUploader(this.S3Client, 5242880L, 5242880000L);
                return this.mMultiPartUploader;
            }
        }

        /// <summary>Returns bucket name from account info.</summary>
        /// <param name="path">Path for which is bucket name returned.</param>
        public static string GetBucketName(string path)
        {
            AbstractStorageProvider storageProvider = StorageHelper.GetStorageProvider(path);
            if (!string.IsNullOrEmpty(storageProvider.CustomRootPath))
                return storageProvider.CustomRootPath;
            return AccountInfo.Current.BucketName;
        }

        /// <summary>Returns whether is access public.</summary>
        /// <param name="path">Path to check.</param>
        private bool IsPublicAccess(string path)
        {
            AbstractStorageProvider storageProvider = StorageHelper.GetStorageProvider(path);
            if (storageProvider.PublicExternalFolderObject.HasValue)
                return storageProvider.PublicExternalFolderObject.Value;
            return AmazonHelper.PublicAccess;
        }

        /// <summary>Returns current instance of S3ObjectInfo provider.</summary>
        public static IS3ObjectInfoProvider Current
        {
            get
            {
                if (S3ObjectInfoProvider.mCurrent == null)
                    S3ObjectInfoProvider.mCurrent = (IS3ObjectInfoProvider)new S3ObjectInfoProvider();
                return S3ObjectInfoProvider.mCurrent;
            }
        }

        /// <summary>
        /// Creates instance of S3ObjectInfoProvider. Default constructor is private, object is singleton.
        /// </summary>
        private S3ObjectInfoProvider()
        {
        }

        /// <summary>
        /// Returns list with objects from given bucket and under given path.
        /// </summary>
        /// <param name="path">Path.</param>
        /// <param name="type">Specifies which objects are returned (files, directories, both).</param>
        /// <param name="useFlatListing">Whether flat listing is used (all files from all subdirectories all in the result).</param>
        /// <param name="lower">Specifies whether path should be lowered inside method.</param>
        /// <param name="useCache">Indicates if results should be primary taken from cache to get better performance</param>
        /// <remarks>
        /// In order to allow to distinguish between files and directories, directories are listed with a trailing backslash.
        /// </remarks>
        public List<string> GetObjectsList(string path, ObjectTypeEnum type, bool useFlatListing = false, bool lower = true, bool useCache = true)
        {
            string bucketName = S3ObjectInfoProvider.GetBucketName(path);
            if (string.IsNullOrEmpty(bucketName))
                return (List<string>)null;
            ListObjectsRequest request = new ListObjectsRequest();
            request.BucketName = bucketName;
            if (!string.IsNullOrEmpty(path))
                request.Prefix = PathHelper.GetObjectKeyFromPath(path).TrimEnd('/') + "/";
            if (!useFlatListing)
                request.Delimiter = "/";
            HashSet<string> source1 = new HashSet<string>();
            string key = request.Prefix + "|" + type.ToString("F") + "|" + (object)useFlatListing + "|" + (object)lower;
            if (useCache && AbstractStockHelper<RequestStockHelper>.Contains("AmazonStorage|GetObjectList|", key, false))
            {
                CMS.IO.Directory.LogDirectoryOperation(path, "GetObjectListFromCache", "Amazon");
                HashSet<string> source2 = AbstractStockHelper<RequestStockHelper>.GetItem("AmazonStorage|GetObjectList|", key, false) as HashSet<string>;
                if (source2 == null)
                    return new List<string>();
                return source2.ToList<string>();
            }
            ListObjectsResponse listObjectsResponse;
            do
            {
                listObjectsResponse = this.S3Client.ListObjects(request);
                if (type == ObjectTypeEnum.Directories && !useFlatListing)
                {
                    foreach (string commonPrefix in listObjectsResponse.CommonPrefixes)
                        source1.Add(path + "\\" + CMS.IO.Path.GetFileName(commonPrefix.TrimEnd('/')) + (object)'\\');
                }
                else
                {
                    bool flag1 = type == ObjectTypeEnum.FilesAndDirectories || type == ObjectTypeEnum.Directories;
                    bool flag2 = type == ObjectTypeEnum.FilesAndDirectories || type == ObjectTypeEnum.Files;
                    foreach (S3Object s3Object in listObjectsResponse.S3Objects)
                    {
                        bool directory = s3Object.Key.EndsWith("/", StringComparison.Ordinal);
                        if (directory && flag1 || !directory && flag2)
                            source1.Add(PathHelper.GetPathFromObjectKey(s3Object.Key, true, directory, lower));
                    }
                }
                if (listObjectsResponse.IsTruncated)
                    request.Marker = listObjectsResponse.NextMarker;
            }
            while (listObjectsResponse.IsTruncated);
            source1.Remove(PathHelper.GetPathFromObjectKey(request.Prefix, true, true, lower));
            CMS.IO.Directory.LogDirectoryOperation(path, "ListObjects", "Amazon");
            AbstractStockHelper<RequestStockHelper>.AddToStorage("AmazonStorage|GetObjectList|", key, (object)source1, false);
            return source1.ToList<string>();
        }

        /// <summary>Returns whether object exists.</summary>
        /// <param name="obj">Object info.</param>
        public bool ObjectExists(IS3ObjectInfo obj)
        {
            return obj.Exists();
        }

        /// <summary>Returns object content as a stream.</summary>
        /// <param name="obj">Object info.</param>
        /// <param name="fileMode">File mode.</param>
        /// <param name="fileAccess">File access.</param>
        /// <param name="fileShare">Sharing permissions.</param>
        /// <param name="bufferSize">Buffer size.</param>
        public System.IO.Stream GetObjectContent(IS3ObjectInfo obj, System.IO.FileMode fileMode = System.IO.FileMode.Open, System.IO.FileAccess fileAccess = System.IO.FileAccess.Read, System.IO.FileShare fileShare = System.IO.FileShare.ReadWrite, int bufferSize = 4096)
        {
            if (!this.ObjectExists(obj))
                return (System.IO.Stream)null;
            AutoResetEvent orAdd = S3ObjectInfoProvider.mS3ObjectEvents.GetOrAdd(obj.Key, new AutoResetEvent(true));
            try
            {
                string str1 = CMS.IO.Path.Combine(PathHelper.TempPath, PathHelper.GetPathFromObjectKey(obj.Key, false));
                Directory.CreateDiskDirectoryStructure(str1);
                string str2 = CMS.IO.Path.Combine(PathHelper.CachePath, PathHelper.GetPathFromObjectKey(obj.Key, false));
                string path = str2 + ".etag";
                orAdd.WaitOne();
                if (CMS.IO.File.Exists(str2) && System.IO.File.ReadAllText(path).Trim() == obj.ETag)
                {
                    orAdd.Set();
                    System.IO.FileStream fileStream = new System.IO.FileStream(str2, fileMode, fileAccess, fileShare, bufferSize);
                    FileDebug.LogFileOperation(PathHelper.GetPathFromObjectKey(obj.Key, true), "GetObjectFromCache", "Amazon");
                    return (System.IO.Stream)fileStream;
                }
                using (GetObjectResponse getObjectResponse = this.S3Client.GetObject(new GetObjectRequest()
                {
                    BucketName = obj.BucketName,
                    Key = obj.Key
                }))
                    getObjectResponse.WriteResponseStreamToFile(str1);
                FileDebug.LogFileOperation(PathHelper.GetPathFromObjectKey(obj.Key, true), "GetObjectFromS3Storage", "Amazon");
                Directory.CreateDiskDirectoryStructure(str2);
                System.IO.File.Copy(str1, str2, true);
                System.IO.File.WriteAllText(path, obj.ETag);
                if (fileMode == System.IO.FileMode.Append && fileAccess != System.IO.FileAccess.Read)
                {
                    fileMode = System.IO.FileMode.Open;
                    fileAccess = System.IO.FileAccess.ReadWrite;
                }
                return (System.IO.Stream)new System.IO.FileStream(str1, fileMode, fileAccess, fileShare, bufferSize);
            }
            finally
            {
                orAdd.Set();
                S3ObjectInfoProvider.mS3ObjectEvents.TryRemove(obj.Key, out orAdd);
            }
        }

        /// <summary>Puts local file to Amazon S3 storage.</summary>
        /// <remarks>
        /// For uploading a file from different file system other than local file system to Amazon S3 storage,
        /// <see cref="M:CMS.AmazonStorage.S3ObjectInfoProvider.PutDataFromStreamToObject(CMS.AmazonStorage.IS3ObjectInfo,System.IO.Stream)" /> method should be used.
        /// </remarks>
        /// <param name="obj">Object info.</param>
        /// <param name="pathToSource">Path to local file.</param>
        public void PutFileToObject(IS3ObjectInfo obj, string pathToSource)
        {
            if (obj.IsLocked)
                throw new Exception("[IS3ObjectInfoProvider.PutFileToObject]: Couldn't upload object " + obj.Key + " because it is used by another process.");
            obj.Lock();
            string bucketName = obj.BucketName;
            long length = new System.IO.FileInfo(pathToSource).Length;
            if (length >= 15728640L)
            {
                CompleteMultipartUploadResponse response = this.MultiPartUploader.UploadFromFilePath(obj.Key, bucketName, pathToSource);
                this.SetS3ObjectMetadaFromResponse(obj, response, length);
            }
            else
            {
                PutObjectRequest putRequest = S3ObjectInfoProvider.CreatePutRequest(obj.Key, bucketName);
                putRequest.FilePath = pathToSource;
                PutObjectResponse response = this.S3Client.PutObject(putRequest);
                this.SetS3ObjectMetadaFromResponse(obj, response, length);
            }
            FileDebug.LogFileOperation(PathHelper.GetPathFromObjectKey(obj.Key, true), nameof(PutFileToObject), "Amazon");
            obj.UnLock();
            S3ObjectInfoProvider.RemoveRequestCache(obj.Key);
        }

        /// <summary>Puts data from stream to Amazon S3 storage.</summary>
        /// <param name="obj">Object info.</param>
        /// <param name="stream">Stream to upload.</param>
        public void PutDataFromStreamToObject(IS3ObjectInfo obj, System.IO.Stream stream)
        {
            if (obj.IsLocked)
                throw new Exception("[IS3ObjectInfoProvider.PutDataFromStreamToObject]: Couldn't upload object " + obj.Key + " because it is used by another process.");
            obj.Lock();
            string bucketName = obj.BucketName;
            long length = stream.Length;
            if (length > 15728640L)
            {
                CompleteMultipartUploadResponse response = this.MultiPartUploader.UploadFromStream(obj.Key, bucketName, stream);
                this.SetS3ObjectMetadaFromResponse(obj, response, length);
            }
            else
            {
                PutObjectRequest putRequest = S3ObjectInfoProvider.CreatePutRequest(obj.Key, bucketName);
                putRequest.InputStream = stream;
                PutObjectResponse response = this.S3Client.PutObject(putRequest);
                this.SetS3ObjectMetadaFromResponse(obj, response, length);
            }
            FileDebug.LogFileOperation(PathHelper.GetPathFromObjectKey(obj.Key, true), "PutStreamToObject", "Amazon");
            obj.UnLock();
            S3ObjectInfoProvider.RemoveRequestCache(obj.Key);
        }

        /// <summary>Puts text to Amazon S3 storage object.</summary>
        /// <param name="obj">Object info.</param>
        /// <param name="content">Content to add.</param>
        public void PutTextToObject(IS3ObjectInfo obj, string content)
        {
            if (obj.IsLocked)
                throw new Exception("[IS3ObjectInfoProvider.PutTextToObject]: Couldn't upload object " + obj.Key + " because it is used by another process.");
            string pathFromObjectKey = PathHelper.GetPathFromObjectKey(obj.Key, true);
            obj.Lock();
            PutObjectRequest putRequest = S3ObjectInfoProvider.CreatePutRequest(obj.Key, S3ObjectInfoProvider.GetBucketName(pathFromObjectKey));
            putRequest.ContentBody = content;
            PutObjectResponse response = this.S3Client.PutObject(putRequest);
            this.SetS3ObjectMetadaFromResponse(obj, response, 0L);
            FileDebug.LogFileOperation(PathHelper.GetPathFromObjectKey(obj.Key, true), nameof(PutTextToObject), "Amazon");
            obj.UnLock();
            S3ObjectInfoProvider.RemoveRequestCache(obj.Key);
        }

        /// <summary>Appends text to Amazon S3 storage object.</summary>
        /// <param name="obj">Object info.</param>
        /// <param name="content">Content to append.</param>
        public void AppendTextToObject(IS3ObjectInfo obj, string content)
        {
            if (this.ObjectExists(obj))
            {
                if (obj.IsLocked)
                    throw new Exception("[IS3ObjectInfoProvider.AppendTextToObject]: Couldn't upload object " + obj.Key + " because it is used by another process.");
                obj.Lock();
                System.IO.Stream objectContent = this.GetObjectContent(obj, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite, 4096);
                string str = (string)null;
                using (CMS.IO.StreamReader streamReader = CMS.IO.StreamReader.New(objectContent))
                    str = streamReader.ReadToEnd();
                PutObjectRequest putRequest = S3ObjectInfoProvider.CreatePutRequest(obj.Key, obj.BucketName);
                putRequest.ContentBody = str + content;
                PutObjectResponse response = this.S3Client.PutObject(putRequest);
                this.SetS3ObjectMetadaFromResponse(obj, response, 0L);
                FileDebug.LogFileOperation(PathHelper.GetPathFromObjectKey(obj.Key, true), nameof(AppendTextToObject), "Amazon");
                obj.UnLock();
                S3ObjectInfoProvider.RemoveRequestCache(obj.Key);
            }
            else
                this.PutTextToObject(obj, content);
        }

        /// <summary>Deletes object from Amazon S3 storage.</summary>
        /// <param name="obj">Object info.</param>
        public void DeleteObject(IS3ObjectInfo obj)
        {
            this.S3Client.DeleteObject(new DeleteObjectRequest()
            {
                BucketName = obj.BucketName,
                Key = obj.Key
            });
            obj.DeleteMetadataFile();
            FileDebug.LogFileOperation(PathHelper.GetPathFromObjectKey(obj.Key, true), nameof(DeleteObject), "Amazon");
            S3ObjectInfoProvider.RemoveRequestCache(obj.Key);
            try
            {
                S3ObjectInfoProvider.RemoveFromTemp(obj);
                S3ObjectInfoProvider.RemoveFromCache(obj);
            }
            catch (IOException ex)
            {
            }
        }

        /// <summary>Copies object to another.</summary>
        /// <param name="sourceObject">Source object info.</param>
        /// <param name="destObject">Destination object info.</param>
        public void CopyObjects(IS3ObjectInfo sourceObject, IS3ObjectInfo destObject)
        {
            string pathFromObjectKey = PathHelper.GetPathFromObjectKey(destObject.Key, true);
            CopyObjectRequest request = new CopyObjectRequest()
            {
                SourceBucket = sourceObject.BucketName,
                DestinationBucket = S3ObjectInfoProvider.GetBucketName(pathFromObjectKey),
                SourceKey = sourceObject.Key,
                DestinationKey = destObject.Key
            };
            if (this.IsPublicAccess(pathFromObjectKey))
                request.CannedACL = S3CannedACL.PublicRead;
            CopyObjectResponse copyObjectResponse = this.S3Client.CopyObject(request);
            destObject.ETag = copyObjectResponse.ETag;
            destObject.Length = ValidationHelper.GetLong((object)copyObjectResponse.ContentLength, 0L, (CultureInfo)null);
            FileDebug.LogFileOperation(PathHelper.GetPathFromObjectKey(sourceObject.Key, true) + "|" + PathHelper.GetPathFromObjectKey(destObject.Key, true), nameof(CopyObjects), "Amazon");
            S3ObjectInfoProvider.RemoveRequestCache(destObject.Key);
        }

        /// <summary>Creates empty object.</summary>
        /// <param name="obj">Object info.</param>
        public void CreateEmptyObject(IS3ObjectInfo obj)
        {
            string pathFromObjectKey = PathHelper.GetPathFromObjectKey(obj.Key, true);
            PutObjectRequest putRequest = S3ObjectInfoProvider.CreatePutRequest(obj.Key, S3ObjectInfoProvider.GetBucketName(pathFromObjectKey));
            putRequest.InputStream = (System.IO.Stream)new System.IO.MemoryStream();
            PutObjectResponse response = this.S3Client.PutObject(putRequest);
            this.SetS3ObjectMetadaFromResponse(obj, response, 0L);
            FileDebug.LogFileOperation(PathHelper.GetPathFromObjectKey(obj.Key, true), nameof(CreateEmptyObject), "Amazon");
            S3ObjectInfoProvider.RemoveRequestCache(obj.Key);
        }

        /// <summary>Returns new instance of IS3ObjectInfo.</summary>
        /// <param name="path">Path with file name.</param>
        public IS3ObjectInfo GetInfo(string path)
        {
            return (IS3ObjectInfo)new S3ObjectInfo(path);
        }

        /// <summary>
        /// Initializes new instance of S3 object info with specified bucket name.
        /// </summary>
        /// <param name="path">Path with file name.</param>
        /// <param name="key">Specifies that given path is already object key.</param>
        public IS3ObjectInfo GetInfo(string path, bool key)
        {
            return (IS3ObjectInfo)new S3ObjectInfo(path, key);
        }

        /// <summary>Removes cached object's items from request cache.</summary>
        /// <param name="objectKey">Object key.</param>
        internal static void RemoveRequestCache(string objectKey)
        {
            string storageKey = "AmazonStorage|S3ObjectInfo|";
            AbstractStockHelper<RequestStockHelper>.Remove(storageKey, objectKey + "|Exists", false);
            AbstractStockHelper<RequestStockHelper>.Remove(storageKey, objectKey + "|Length", false);
            AbstractStockHelper<RequestStockHelper>.Remove(storageKey, objectKey + "|Metadata", false);
            AbstractStockHelper<RequestStockHelper>.Remove(storageKey, objectKey + "|ETag", false);
            AbstractStockHelper<RequestStockHelper>.DropStorage("AmazonStorage|GetObjectList|", false);
        }

        /// <summary>Remove S3 file from temporary local storage.</summary>
        /// <param name="obj">S3 object to be removed from temporary local storage</param>
        private static void RemoveFromTemp(IS3ObjectInfo obj)
        {
            S3ObjectInfoProvider.DeleteFileFromLocalPath(CMS.IO.Path.Combine(PathHelper.TempPath, PathHelper.GetPathFromObjectKey(obj.Key, false)));
        }

        /// <summary>Remove S3 file from cache local storage.</summary>
        /// <param name="obj">S3 object to be removed from cache local storage</param>
        private static void RemoveFromCache(IS3ObjectInfo obj)
        {
            string path = CMS.IO.Path.Combine(PathHelper.CachePath, PathHelper.GetPathFromObjectKey(obj.Key, false));
            S3ObjectInfoProvider.DeleteFileFromLocalPath(path);
            S3ObjectInfoProvider.DeleteFileFromLocalPath(path + ".etag");
        }

        /// <summary>Delete file from local filesystem</summary>
        /// <param name="path">Path to the file to be deleted</param>
        private static void DeleteFileFromLocalPath(string path)
        {
            if (string.IsNullOrEmpty(path) || !System.IO.File.Exists(path))
                return;
            System.IO.File.Delete(path);
        }

        /// <summary>
        /// Creates request for uploading data to Amazon S3 storage.
        /// </summary>
        /// <param name="key">Unique identifier for an object within a bucket.</param>
        /// <param name="bucket">Existing Amazon S3 bucket.</param>
        private static PutObjectRequest CreatePutRequest(string key, string bucket)
        {
            PutObjectRequest putObjectRequest = new PutObjectRequest()
            {
                BucketName = bucket,
                Key = key
            };
            if (AmazonHelper.PublicAccess)
                putObjectRequest.CannedACL = S3CannedACL.PublicRead;
            return putObjectRequest;
        }

        /// <summary>
        /// Sets metadata from response acquired from Amazon S3 storage to S3ObjectInfo.
        /// </summary>
        /// <param name="obj">Representation of the file on Amazon S3 storage.</param>
        /// <param name="eTag">ETag assigned to file uploaded to Amazon S3 storage.</param>
        /// <param name="response">Response acquired from Amazon S3 storage after uploading file.</param>
        /// <param name="length">
        /// Amazon S3 storage does not return length of the uploaded file in <paramref name="response" />,
        /// if the file was uploaded via multipart upload. In case of multipart upload is <see cref="P:CMS.AmazonStorage.IS3ObjectInfo.Length" />
        /// of the <paramref name="obj" /> set via this parameter.
        /// </param>
        private void SetS3ObjectMetadaFromResponse(IS3ObjectInfo obj, string eTag, AmazonWebServiceResponse response, long length)
        {
            long num = ValidationHelper.GetLong((object)response.ContentLength, 0L, (CultureInfo)null);
            if (num == 0L)
                num = length;
            obj.ETag = eTag;
            obj.Length = num;
        }

        /// <summary>
        /// Sets metadata from response acquired from Amazon S3 storage to S3ObjectInfo.
        /// </summary>
        /// <param name="obj">Representation of the file on Amazon S3 storage.</param>
        /// <param name="response">Response acquired from Amazon S3 storage after uploading file.</param>
        /// <param name="length">
        /// Amazon S3 storage does not return length of the uploaded file in <paramref name="response" />,
        /// if the file was uploaded via multipart upload. In case of multipart upload is <see cref="P:CMS.AmazonStorage.IS3ObjectInfo.Length" />
        /// of the <paramref name="obj" /> set via this parameter.
        /// </param>
        private void SetS3ObjectMetadaFromResponse(IS3ObjectInfo obj, PutObjectResponse response, long length = 0)
        {
            this.SetS3ObjectMetadaFromResponse(obj, response.ETag, (AmazonWebServiceResponse)response, length);
        }

        /// <summary>
        /// Sets metadata from response acquired from Amazon S3 storage to S3ObjectInfo.
        /// </summary>
        /// <param name="obj">Representation of the file on Amazon S3 storage.</param>
        /// <param name="response">Response acquired from Amazon S3 storage after uploading file.</param>
        /// <param name="length">
        /// Amazon S3 storage does not return length of the uploaded file in <paramref name="response" />,
        /// if the file was uploaded via multipart upload. In case of multipart upload is <see cref="P:CMS.AmazonStorage.IS3ObjectInfo.Length" />
        /// of the <paramref name="obj" /> set via this parameter.
        /// </param>
        private void SetS3ObjectMetadaFromResponse(IS3ObjectInfo obj, CompleteMultipartUploadResponse response, long length = 0)
        {
            this.SetS3ObjectMetadaFromResponse(obj, response.ETag, (AmazonWebServiceResponse)response, length);
        }

        /// <summary>Returns date time as a string type in english culture.</summary>
        /// <param name="datetime">Date time.</param>
        public static string GetDateTimeString(DateTime datetime)
        {
            return ValidationHelper.GetString((object)datetime, string.Empty, CultureHelper.EnglishCulture);
        }

        /// <summary>
        /// Returns date time as a DateTime type converted using english culture.
        /// </summary>
        /// <param name="datetime">String date time.</param>
        public static DateTime GetStringDateTime(string datetime)
        {
            return ValidationHelper.GetDateTime((object)datetime, DateTimeHelper.ZERO_TIME, CultureHelper.EnglishCulture);
        }
    }
}