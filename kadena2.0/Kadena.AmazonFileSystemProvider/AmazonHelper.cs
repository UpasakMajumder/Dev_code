using CMS.Base;
using CMS.DataEngine;
using CMS.Helpers;

namespace Kadena.AmazonFileSystemProvider
{
    /// <summary>
    /// Helper methods and properties for Amazon web services integration.
    /// </summary>
    public static class AmazonHelper
    {
        private static bool? mPublicAccess;
        private static string mEndPoint;
        /// <summary>Path to azure file page.</summary>
        public const string AMAZON_FILE_PAGE = "~/CMSPages/GetAmazonFile.aspx";

        /// <summary>
        /// Returns whether Amazon S3 storage sets public read access to uploaded files and whether direct storage links are generated.
        /// </summary>
        public static bool PublicAccess
        {
            get
            {
                if (!mPublicAccess.HasValue)
                {
                    GetEndPointAndAccess();
                }
                return mPublicAccess.Value;
            }
            set
            {
                mPublicAccess = new bool?(value);
            }
        }

        /// <summary>Returns Amazon S3 endpoint.</summary>
        public static string EndPoint
        {
            get
            {
                if (mEndPoint == null)
                {
                    GetEndPointAndAccess();
                }
                return mEndPoint;
            }
            set
            {
                mEndPoint = value;
            }
        }

        /// <summary>Returns download path for given path.</summary>
        /// <param name="path">Path</param>
        public static string GetDownloadPath(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                return $"{AMAZON_FILE_PAGE}?path={URLHelper.EscapeSpecialCharacters(path)}";
            }
            return null;
        }

        /// <summary>
        /// Gets end point and public access from web.config and sets properties.
        /// </summary>
        private static void GetEndPointAndAccess()
        {
            mEndPoint = ValidationHelper.GetString(SettingsHelper.AppSettings["CMSAmazonEndPoint"], null);
            if (mEndPoint == null)
            {
                mEndPoint = $"http://{SettingsKeyInfoProvider.GetValue(SettingsKeyNames.AmazonS3BucketName)}.s3.amazonaws.com";
                mPublicAccess = new bool?(ValidationHelper.GetBoolean(SettingsHelper.AppSettings["CMSAmazonPublicAccess"], false));
            }
            else
            {
                mEndPoint = URLHelper.AddHTTPToUrl(mEndPoint);
                mPublicAccess = new bool?(ValidationHelper.GetBoolean(SettingsHelper.AppSettings["CMSAmazonPublicAccess"], true));
            }
        }
    }
}
