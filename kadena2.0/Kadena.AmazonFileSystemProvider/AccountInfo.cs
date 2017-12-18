using Amazon.S3;
using CMS.DataEngine;
using System;

namespace Kadena.AmazonFileSystemProvider
{
    /// <summary>Represents Account of Amazon web services.</summary>
    public class AccountInfo
    {
        private static readonly Lazy<AccountInfo> mCurrent = new Lazy<AccountInfo>(() => new AccountInfo(), true);
        private readonly AmazonS3Client mS3Client;

        /// <summary>Default constructor is private.</summary>
        private AccountInfo()
        {
            this.BucketName = SettingsKeyInfoProvider.GetValue(SettingsKeyNames.AmazonS3BucketName);
            if (string.IsNullOrEmpty(this.BucketName))
            {
                throw new InvalidOperationException($"Amazon S3 bucket name could not be found. You must specify it in Settings by {SettingsKeyNames.AmazonS3BucketName} setting key.");
            }
            this.mS3Client = new AmazonS3ClientFactory().Create(this.BucketName);
        }

        /// <summary>Returns instance of Account Info.</summary>
        public static AccountInfo Current
        {
            get
            {
                return mCurrent.Value;
            }
        }

        /// <summary>Sets or gets bucket name.</summary>
        public string BucketName { get; set; }

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