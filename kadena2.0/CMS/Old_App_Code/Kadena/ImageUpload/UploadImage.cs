using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.IO;

using CMS.PortalEngine.Web.UI;
using CMS.Helpers;
using CMS.DocumentEngine.Types.KDA;
using System.Collections.Generic;
using CMS.DocumentEngine;
using CMS.Membership;
using CMS.SiteProvider;
using CMS.EventLog;
using CMS.MediaLibrary;
using CMS.CustomTables;
using CMS.CustomTables.Types.KDA;

namespace Kadena.Old_App_Code.Kadena.ImageUpload
{
    public static class UploadImage
    {
        /// <summary>
        /// Insert image to Media Library
        /// </summary>
        /// <param name="productImage"></param>
        /// <returns></returns>
        public static Guid UploadImageToMeadiaLibrary(FileUpload productImage, string folderName)
        {
            Guid fileGuid = default(Guid);
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
                    fileGuid = mediaFile.FileGUID;
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena.Old_App_Code.Kadena.ImageUpload", "UploadImageToMeadiaLibrary", ex.Message);
            }
            return fileGuid;
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
                EventLogProvider.LogInformation("Kadena.Old_App_Code.Kadena.ImageUpload", "CreateMeadiaLiabrary", ex.Message);
            }
        }
        /// <summary>
        /// Delete Image by Guid
        /// </summary>
        /// <param name="imageGuid"></param>
        public static void DeleteImage(Guid imageGuid, string folderName)
        {
            try
            {
                MediaLibraryInfo library = MediaLibraryInfoProvider.GetMediaLibraryInfo(!string.IsNullOrEmpty(folderName) ? folderName : "CampaignProducts", SiteContext.CurrentSiteName);
                if (!DataHelper.DataSourceIsEmpty(library))
                {
                    MediaFileInfo deleteFile = MediaFileInfoProvider.GetMediaFileInfo(imageGuid, SiteContext.CurrentSiteName);
                    if (!DataHelper.IsEmpty(deleteFile))
                    {
                        MediaFileInfoProvider.DeleteMediaFileInfo(deleteFile);
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena.Old_App_Code.Kadena.ImageUpload", "DeleteImage", ex.Message);
            }
        }
    }
}