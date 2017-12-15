using Amazon.S3;
using CMS.Base;
using CMS.Helpers;
using System;
using System.Globalization;

namespace Kadena.AmazonFileSystemProvider
{
    /// <summary>Represents Account of Amazon web services.</summary>
    public class AccountInfo
    {
        private static readonly Lazy<AccountInfo> mCurrent = new Lazy<AccountInfo>((Func<AccountInfo>)(() => new AccountInfo()), true);
        private readonly AmazonS3Client mS3Client;

        /// <summary>Default constructor is private.</summary>
        private AccountInfo()
        {
            this.AccessKeyID = ValidationHelper.GetString((object)SettingsHelper.AppSettings["CMSAmazonAccessKeyID"], string.Empty, (CultureInfo)null);
            this.AccessKey = ValidationHelper.GetString((object)SettingsHelper.AppSettings["CMSAmazonAccessKey"], string.Empty, (CultureInfo)null);
            this.BucketName = ValidationHelper.GetString((object)SettingsHelper.AppSettings["CMSAmazonBucketName"], string.Empty, (CultureInfo)null);
            if (string.IsNullOrEmpty(this.BucketName))
                throw new InvalidOperationException("Amazon S3 bucket name could not be found. You must specify it in web.config file by CMSAmazonBucketName application setting key.");
            if (string.IsNullOrEmpty(this.AccessKey) || string.IsNullOrEmpty(this.AccessKeyID))
                throw new InvalidOperationException("Amazon S3 access key or access key id could not be found. You must specify it in web.config file by CMSAmazonAccessKey and CMSAmazonAccessKeyID settings keys.");
            this.mS3Client = new AmazonS3ClientFactory().Create(this.AccessKeyID, this.AccessKey, this.BucketName);
        }

        /// <summary>Constructor where credentials could be specified.</summary>
        /// <param name="accessKeyID">Access key ID.</param>
        /// <param name="accessKey">Access secret key.</param>
        /// <param name="bucketName">Bucket name.</param>
        public AccountInfo(string accessKeyID, string accessKey, string bucketName)
        {
            if (string.IsNullOrEmpty(bucketName))
                throw new ArgumentException("Bucket name can not be null or empty.", nameof(bucketName));
            if (string.IsNullOrEmpty(accessKey) || string.IsNullOrEmpty(accessKeyID))
                throw new ArgumentException("Access key ID and Access key can not be null or empty.");
            this.AccessKeyID = accessKeyID;
            this.AccessKey = accessKey;
            this.BucketName = bucketName;
            this.mS3Client = new AmazonS3ClientFactory().Create(this.AccessKeyID, this.AccessKey, this.BucketName);
        }

        /// <summary>Returns instance of Account Info.</summary>
        public static AccountInfo Current
        {
            get
            {
                return AccountInfo.mCurrent.Value;
            }
        }

        /// <summary>Sets or gets bucket name.</summary>
        public string BucketName { get; set; }

        /// <summary>Gets or sets access key id.</summary>
        public string AccessKeyID { get; set; }

        /// <summary>Gets or sets secret access key.</summary>
        public string AccessKey { get; set; }

        /// <summary>
        /// Gets instance of AmazonS3 class (main class providing operations with storage).
        /// </summary>
        public AmazonS3Client S3Client
        {
            get
            {
                return this.mS3Client;
            }
        }
    }
}