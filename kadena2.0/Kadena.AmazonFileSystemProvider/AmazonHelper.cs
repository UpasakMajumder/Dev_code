using CMS.Base;
using CMS.Helpers;
using System.Globalization;

namespace Kadena.AmazonFileSystemProvider
{
    /// <summary>
    /// Helper methods and properties for Amazon web services integration.
    /// </summary>
    public static class AmazonHelper
    {
        private static bool? mPublicAccess = new bool?();
        private static string mEndPoint = (string)null;
        /// <summary>Path to azure file page.</summary>
        public const string AMAZON_FILE_PAGE = "~/CMSPages/GetAmazonFile.aspx";

        /// <summary>
        /// Returns whether Amazon S3 storage sets public read access to uploaded files and whether direct storage links are generated.
        /// </summary>
        public static bool PublicAccess
        {
            get
            {
                if (!AmazonHelper.mPublicAccess.HasValue)
                    AmazonHelper.GetEndPointAndAccess();
                return AmazonHelper.mPublicAccess.Value;
            }
            set
            {
                AmazonHelper.mPublicAccess = new bool?(value);
            }
        }

        /// <summary>Returns Amazon S3 endpoint.</summary>
        public static string EndPoint
        {
            get
            {
                if (AmazonHelper.mEndPoint == null)
                {
                    AmazonHelper.mEndPoint = ValidationHelper.GetString((object)SettingsHelper.AppSettings["CMSAmazonEndPoint"], (string)null, (CultureInfo)null);
                    AmazonHelper.GetEndPointAndAccess();
                }
                return AmazonHelper.mEndPoint;
            }
            set
            {
                AmazonHelper.mEndPoint = value;
            }
        }

        /// <summary>Returns download path for given path.</summary>
        /// <param name="path">Path</param>
        public static string GetDownloadPath(string path)
        {
            if (!string.IsNullOrEmpty(path))
                return "~/CMSPages/GetAmazonFile.aspx?path=" + URLHelper.EscapeSpecialCharacters(path);
            return (string)null;
        }

        /// <summary>
        /// Gets end point and public access from web.config and sets properties.
        /// </summary>
        private static void GetEndPointAndAccess()
        {
            AmazonHelper.mEndPoint = ValidationHelper.GetString((object)SettingsHelper.AppSettings["CMSAmazonEndPoint"], (string)null, (CultureInfo)null);
            if (AmazonHelper.mEndPoint == null)
            {
                AmazonHelper.mEndPoint = "http://" + SettingsHelper.AppSettings["CMSAmazonBucketName"] + ".s3.amazonaws.com";
                AmazonHelper.mPublicAccess = new bool?(ValidationHelper.GetBoolean((object)SettingsHelper.AppSettings["CMSAmazonPublicAccess"], false, (CultureInfo)null));
            }
            else
            {
                AmazonHelper.mEndPoint = URLHelper.AddHTTPToUrl(AmazonHelper.mEndPoint);
                AmazonHelper.mPublicAccess = new bool?(ValidationHelper.GetBoolean((object)SettingsHelper.AppSettings["CMSAmazonPublicAccess"], true, (CultureInfo)null));
            }
        }
    }
}
