using Amazon;
using Amazon.S3;
using System;

namespace Kadena.AmazonFileSystemProvider
{
    /// <summary>
    /// Class that encapsulates creation of the <see cref="T:Amazon.S3.AmazonS3Client" /> instance.
    /// </summary>
    internal class AmazonS3ClientFactory
    {
        private const string NOT_RECOGNIZED_REGION_ENDPOINT = "Unknown";
        private const string S3_SERVICE_ENDPOINT = "http://s3.amazonaws.com";

        /// <summary>
        /// Creates <see cref="T:Amazon.S3.AmazonS3Client" /> instance that can access Amazon S3 via REST interface.
        /// </summary>
        /// <param name="accessKeyID">It is alphanumeric text string that uniquely identifies the user who owns the account.</param>
        /// <param name="accessKey">Password to the Amazon S3 storage.</param>
        /// <param name="bucketName">Name of the bucket in Amazon S3 storage.</param>
        /// <returns>Returns <see cref="T:Amazon.S3.AmazonS3Client" /> instance that can access Amazon S3 via REST interface.</returns>
        public AmazonS3Client Create(string accessKeyID, string accessKey, string bucketName)
        {
            if (string.IsNullOrWhiteSpace(accessKeyID))
                throw new ArgumentException("accessKeyID is null or empty", nameof(accessKeyID));
            if (string.IsNullOrWhiteSpace(accessKey))
                throw new ArgumentException("accessKey is null or empty", nameof(accessKey));
            if (string.IsNullOrWhiteSpace(bucketName))
                throw new ArgumentException("bucketName is null or empty", nameof(bucketName));
            return this.CreateInternal(accessKeyID, accessKey, bucketName);
        }

        /// <summary>
        /// Creates <see cref="T:Amazon.S3.AmazonS3Client" /> instance that can access Amazon S3 via REST interface.
        /// </summary>
        /// <param name="accessKeyID">It is alphanumeric text string that uniquely identifies the user who owns the account.</param>
        /// <param name="accessKey">Password to the Amazon S3 storage.</param>
        /// <param name="bucketName">Name of the bucket in Amazon S3 storage.</param>
        /// <returns>Returns <see cref="T:Amazon.S3.AmazonS3Client" /> instance that can access Amazon S3 via REST interface.</returns>
        private AmazonS3Client CreateInternal(string accessKeyID, string accessKey, string bucketName)
        {
            var clientConfig = new AmazonS3Config
            {
                ServiceURL = S3_SERVICE_ENDPOINT
            };

            using (var amazonS3Client = new AmazonS3Client(clientConfig))
            {
                var bucketLocation = amazonS3Client.GetBucketLocation(bucketName);
                var regionEndpoint = this.GetRegionEndpoint(bucketLocation.Location);
                return new AmazonS3Client(regionEndpoint);
            }
        }

        /// <summary>
        /// Gets existing <see cref="T:Amazon.RegionEndpoint" /> from <paramref name="location" />.
        /// </summary>
        /// <param name="location">String value returned from Amazon S3 service that defines bucket location.</param>
        internal RegionEndpoint GetRegionEndpoint(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
            {
                return RegionEndpoint.USEast1;
            }
            RegionEndpoint regionEndpoint = RegionEndpoint.GetBySystemName(location);
            if (regionEndpoint.SystemName.Equals(NOT_RECOGNIZED_REGION_ENDPOINT, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("The specified Amazon S3 bucket does not exist or is located in region that is currently not supported.");
            }
            if (regionEndpoint.SystemName.Equals("EU", StringComparison.OrdinalIgnoreCase)
                && regionEndpoint.DisplayName.Equals(NOT_RECOGNIZED_REGION_ENDPOINT, StringComparison.OrdinalIgnoreCase))
            {
                regionEndpoint = RegionEndpoint.EUWest1;
            }
            return regionEndpoint;
        }
    }
}
