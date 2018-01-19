using Amazon.S3;
using Amazon.S3.Model;
using CMS.EventLog;
using System;
using System.Collections.Generic;
using System.IO;

namespace Kadena.AmazonFileSystemProvider
{
    /// <summary>Class for uploading large files to Amazon S3 storage.</summary>
    internal class S3MultiPartUploader
    {
        private readonly AmazonS3Client mS3Client;
        private readonly long mMinimalPartSize;
        private readonly long mMaximalPartSize;

        /// <summary>
        /// Creates utility for uploading large files in smaller parts to Amazon S3 storage.
        /// </summary>
        /// <param name="s3Client">Client for accessing the Amazon S3 storage.</param>
        /// <param name="minimalPartSize">Minimal size of the part sent in one request to Amazon S3 storage.</param>
        /// <param name="maximalPartSize">Maximal possible size of the part sent in one request to Amazon S3 storage.</param>
        internal S3MultiPartUploader(AmazonS3Client s3Client, long minimalPartSize, long maximalPartSize)
        {
            if (s3Client == null)
            {
                throw new ArgumentNullException(nameof(s3Client));
            }
            this.mS3Client = s3Client;
            if (minimalPartSize < 1L)
            {
                throw new ArgumentOutOfRangeException(nameof(minimalPartSize), "minimalPartSize cannot be smaller than 1.");
            }
            if (maximalPartSize <= minimalPartSize)
            {
                throw new ArgumentOutOfRangeException(nameof(maximalPartSize), "maximalPartSize cannot be smaller than minimalPartSize.");
            }
            this.mMinimalPartSize = minimalPartSize;
            this.mMaximalPartSize = maximalPartSize;
        }

        /// <summary>
        /// Inits multipart upload with given <paramref name="key" /> inside <paramref name="bucket" /> with Amazon S3 storage.
        /// </summary>
        /// <param name="key">Unique identifier for an object within a bucket.</param>
        /// <param name="bucket">Existing Amazon S3 bucket.</param>
        /// <returns>
        /// Upload ID, unique identifier for one multipart upload to Amazon S3 storage.
        /// Returned upload ID is needed for each subsequent multipart upload operation.
        /// </returns>
        public string InitMultiPartUpload(string key, string bucket)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(bucket))
            {
                throw new ArgumentException("key and bucket cannot be empty.");
            }
            return this.mS3Client.InitiateMultipartUpload(new InitiateMultipartUploadRequest()
            {
                Key = key,
                BucketName = bucket,
                CannedACL = AmazonHelper.PublicAccess ? S3CannedACL.PublicRead : S3CannedACL.NoACL
            }).UploadId;
        }

        /// <summary>Uploads one part of a file to Amazon S3 storage.</summary>
        /// <remarks>
        /// If some of the Amazon S3 policies about multipart upload is violated then exception will be thrown.
        /// For example, if part smaller than 5 MB is uploaded and given part is not the last part of the multipart upload process.
        /// </remarks>
        /// <param name="uploadId">Unique identifier for one multipart upload. Can be obtained by <see cref="M:CMS.AmazonStorage.S3MultiPartUploader.InitMultiPartUpload(System.String,System.String)" /> method.</param>
        /// <param name="key">Unique identifier for an object within a bucket.</param>
        /// <param name="bucket">Existing Amazon S3 bucket.</param>
        /// <param name="partNumber">Number that defines position of the data obtained by the stream in the whole multipart upload process.</param>
        /// <param name="stream">Stream with data to upload. Supplied stream has always it's position set to origin.</param>
        /// <returns>Unique identifier of the uploaded part.</returns>
        public string UploadPartFromStream(string uploadId, string key, string bucket, int partNumber, Stream stream)
        {
            if (stream.Length > this.mMaximalPartSize)
            {
                throw new ArgumentException($"stream's length exceeds maximal part size ({this.mMaximalPartSize}B) that can be uploaded to Amazon S3 storage.");
            }
            UploadPartRequest uploadPartRequest = this.CreateUploadPartRequest(key, bucket, uploadId);
            stream.Seek(0L, SeekOrigin.Begin);
            uploadPartRequest.PartSize = stream.Length;
            uploadPartRequest.InputStream = stream;
            uploadPartRequest.PartNumber = partNumber;
            return this.mS3Client.UploadPart(uploadPartRequest).ETag;
        }

        /// <summary>
        /// Uploads one large file to Amazon S3 storage in smaller parts.
        /// </summary>
        /// <param name="key">Unique identifier for an object within a bucket.</param>
        /// <param name="bucket">Existing Amazon S3 bucket.</param>
        /// <param name="stream">Stream with data to upload. Supplied stream has always it's position set to origin.</param>
        /// <returns>Response containing metadata of uploaded file.</returns>
        public CompleteMultipartUploadResponse UploadFromStream(string key, string bucket, Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            return this.MultiPartUploadFromStream(key, bucket, stream);
        }

        /// <summary>
        /// Uploads large file from local file system to Amazon S3 storage in smaller parts.
        /// </summary>
        /// <param name="key">Unique identifier for an object within a bucket.</param>
        /// <param name="bucket">Existing Amazon S3 bucket.</param>
        /// <param name="filePath">Path to a local file.</param>
        /// <returns>Response contains metadata of uploaded file like ETag.</returns>
        public CompleteMultipartUploadResponse UploadFromFilePath(string key, string bucket, string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("filePath cannot be null or empty.");
            }
            using (var fileStream = new System.IO.FileStream(filePath, FileMode.Open))
            {
                return this.UploadFromStream(key, bucket, fileStream);
            }
        }

        /// <summary>
        /// Completes multiple part upload process.
        /// Sends final request to Amazon S3 to merge all parts already sent to storage.
        /// </summary>
        /// <param name="key">Unique identifier for an object within a bucket.</param>
        /// <param name="bucket">Existing Amazon S3 bucket.</param>
        /// <param name="uploadId">Unique identifier for one multipart upload. Can be obtained by <see cref="M:CMS.AmazonStorage.S3MultiPartUploader.InitMultiPartUpload(System.String,System.String)" /> method.</param>
        /// <param name="uploadedPartResponses">List of responses from Amazon S3 received after uploading each part.</param>
        /// <returns>Response from Amazon S3 storage after finishing the multipart upload.</returns>
        public CompleteMultipartUploadResponse CompleteMultiPartUploadProcess(string key, string bucket, string uploadId, IEnumerable<UploadPartResponse> uploadedPartResponses)
        {
            var request = new CompleteMultipartUploadRequest
            {
                Key = key,
                BucketName = bucket,
                UploadId = uploadId
            };
            request.AddPartETags(uploadedPartResponses);
            return this.mS3Client.CompleteMultipartUpload(request);
        }

        /// <summary>
        /// Aborts multipart upload, so Amazon S3 storage can delete uploaded parts.
        /// </summary>
        /// <param name="key">Unique identifier for an object within a bucket.</param>
        /// <param name="bucket">Existing Amazon S3 bucket.</param>
        /// <param name="uploadId">Unique identifier for one multipart upload. Can be obtained by <see cref="M:CMS.AmazonStorage.S3MultiPartUploader.InitMultiPartUpload(System.String,System.String)" /> method.</param>
        public void AbortMultiPartUpload(string key, string bucket, string uploadId)
        {
            this.mS3Client.AbortMultipartUpload(new AbortMultipartUploadRequest()
            {
                BucketName = bucket,
                Key = key,
                UploadId = uploadId
            });
        }

        /// <summary>
        /// Uploads one large file stored in <paramref name="stream" /> to Amazon S3 storage in smaller parts.
        /// </summary>
        /// <param name="key">Unique identifier for an object within a bucket.</param>
        /// <param name="bucket">Existing Amazon S3 bucket.</param>
        /// <param name="stream">Stream with data to upload. Supplied stream has always it's position set to origin.</param>
        /// <returns>
        /// Response from Amazon S3 storage after finishing the multipart upload.
        /// Response contains metadata of the uploaded file.
        /// </returns>
        private CompleteMultipartUploadResponse MultiPartUploadFromStream(string key, string bucket, Stream stream)
        {
            var uploadPartResponseList = new List<UploadPartResponse>();
            string uploadId = this.InitMultiPartUpload(key, bucket);
            UploadPartRequest uploadPartRequest = this.CreateUploadPartRequest(key, bucket, uploadId);
            stream.Seek(0L, SeekOrigin.Begin);
            Stream stream1 = stream;
            try
            {
                uploadPartRequest.PartNumber = 1;
                while (uploadPartRequest.FilePosition < stream1.Length)
                {
                    uploadPartRequest.InputStream = stream1;
                    uploadPartResponseList.Add(this.mS3Client.UploadPart(uploadPartRequest));
                    uploadPartRequest.FilePosition += uploadPartRequest.PartSize;
                    ++uploadPartRequest.PartNumber;
                }
                return this.CompleteMultiPartUploadProcess(uploadPartRequest.Key, uploadPartRequest.BucketName, 
                    uploadPartRequest.UploadId, uploadPartResponseList);
            }
            catch (AmazonS3Exception ex)
            {
                EventLogProvider.LogException("AmazonStorage", "MULTIPARTUPLOAD", ex, 0);
                this.AbortMultiPartUpload(uploadPartRequest.Key, uploadPartRequest.BucketName, uploadPartRequest.UploadId);
                throw;
            }
        }

        /// <summary>Creates first request used for multipart upload.</summary>
        /// <param name="key">Unique identifier for an object within a bucket.</param>
        /// <param name="bucket">Existing Amazon S3 bucket.</param>
        /// <param name="uploadId">Unique identifier for one multipart upload. Can be obtained by <see cref="M:CMS.AmazonStorage.S3MultiPartUploader.InitMultiPartUpload(System.String,System.String)" /> method.</param>
        /// <returns>Returns first request used for multipart upload.</returns>
        private UploadPartRequest CreateUploadPartRequest(string key, string bucket, string uploadId)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(bucket) || string.IsNullOrEmpty(uploadId))
            {
                throw new ArgumentException("key, bucket and uploadId cannot be empty.");
            }
            return new UploadPartRequest()
            {
                Key = key,
                BucketName = bucket,
                PartSize = this.mMinimalPartSize,
                FilePosition = 0,
                UploadId = uploadId,
                PartNumber = 1
            };
        }
    }
}
