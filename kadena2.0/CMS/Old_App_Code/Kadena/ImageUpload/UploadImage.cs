using CMS.EventLog;
using CMS.Helpers;
using CMS.MediaLibrary;
using CMS.SiteProvider;
using System;
using System.IO;
using System.Web.UI.WebControls;

namespace Kadena.Old_App_Code.Kadena.ImageUpload
{
    public static class UploadImage
    {
        /// <summary>
        /// Insert image to Media Library
        /// </summary>
        /// <param name="productImage"></param>
        /// <returns></returns>
        public static string UploadImageToMeadiaLibrary(FileUpload productImage, string folderName)
        {
            string imagePath = string.Empty;
            try
            {
                string folderPath = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string filePath = Path.GetFileName(productImage.PostedFile.FileName);
                productImage.PostedFile.SaveAs(System.Web.HttpContext.Current.Server.MapPath("~/Uploads/" + filePath));
                MediaLibraryInfo library = MediaLibraryInfoProvider.GetMediaLibraryInfo(!string.IsNullOrEmpty(folderName) ? folderName.Replace(" ", "") : "CampaignProducts", SiteContext.CurrentSiteName);
                if (library == null)
                {
                    CreateMeadiaLiabrary(folderName);
                    library = MediaLibraryInfoProvider.GetMediaLibraryInfo(!string.IsNullOrEmpty(folderName) ? folderName.Replace(" ", "") : "CampaignProducts", SiteContext.CurrentSiteName);
                }
                filePath = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/" + filePath);
                CMS.IO.FileInfo file = CMS.IO.FileInfo.New(filePath);
                if (file != null)
                {
                    MediaFileInfo mediaFile = new MediaFileInfo(filePath, library.LibraryID);
                    mediaFile.FileName = Path.GetFileNameWithoutExtension(productImage.PostedFile.FileName);
                    mediaFile.FileTitle = Path.GetFileNameWithoutExtension(productImage.PostedFile.FileName);
                    mediaFile.FilePath = "Images/";
                    mediaFile.FileExtension = file.Extension;
                    mediaFile.FileMimeType = MimeTypeHelper.GetMimetype(file.Extension);
                    mediaFile.FileSiteID = SiteContext.CurrentSiteID;
                    mediaFile.FileLibraryID = library.LibraryID;
                    mediaFile.FileSize = file.Length;
                    MediaFileInfoProvider.SetMediaFileInfo(mediaFile);
                    File.Delete(filePath);
                    imagePath = MediaFileURLProvider.GetMediaFileUrl(mediaFile.FileGUID, mediaFile.FileName);
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("UploadImage", "UploadImageToMeadiaLibrary", ex, SiteContext.CurrentSite.SiteID, ex.Message);
            }
            return imagePath;
        }

        /// <summary>
        /// Create new Meadia Library
        /// </summary>
        public static void CreateMeadiaLiabrary(string folderName)
        {
            try
            {
                MediaLibraryInfo newLibrary = new MediaLibraryInfo();
                newLibrary.LibraryDisplayName = !string.IsNullOrEmpty(folderName) ? folderName : "Campaign Products";
                newLibrary.LibraryName = !string.IsNullOrEmpty(folderName) ? folderName.Replace(" ", "") : "CampaignProducts";
                newLibrary.LibraryFolder = !string.IsNullOrEmpty(folderName) ? folderName.Replace(" ", "") : "CampaignProducts";
                newLibrary.LibrarySiteID = SiteContext.CurrentSiteID;
                MediaLibraryInfoProvider.SetMediaLibraryInfo(newLibrary);
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("UploadImage", "CreateMeadiaLiabrary", ex, SiteContext.CurrentSite.SiteID, ex.Message);
            }
        }

        /// <summary>
        /// Delete Image by Guid
        /// </summary>
        /// <param name="imageGuid"></param>
        public static void DeleteImage(string imagePath, string folderName)
        {
            try
            {
                MediaLibraryInfo library = MediaLibraryInfoProvider.GetMediaLibraryInfo(!string.IsNullOrEmpty(folderName) ? folderName : "CampaignProducts", SiteContext.CurrentSiteName);
                if (!DataHelper.DataSourceIsEmpty(library))
                {
                    MediaFileInfo deleteFile = MediaFileInfoProvider.GetMediaFileInfo(SiteContext.CurrentSiteName, imagePath, !string.IsNullOrEmpty(folderName) ? folderName : "CampaignProducts");
                    if (!DataHelper.IsEmpty(deleteFile))
                    {
                        MediaFileInfoProvider.DeleteMediaFileInfo(deleteFile);
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("UploadImage", "DeleteImage", ex, SiteContext.CurrentSite.SiteID, ex.Message);
            }
        }
    }
}