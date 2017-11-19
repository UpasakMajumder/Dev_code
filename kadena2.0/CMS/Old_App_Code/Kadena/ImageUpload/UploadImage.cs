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
        public static Guid UploadImageToMeadiaLibrary(FileUpload productImage)
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
                if (File.Exists("~/Uploads/" + filePath))
                {
                    filePath = filePath + new Random().Next(1, 1000);
                }
                productImage.PostedFile.SaveAs(System.Web.HttpContext.Current.Server.MapPath("~/Uploads/" + filePath));
                MediaLibraryInfo library = MediaLibraryInfoProvider.GetMediaLibraryInfo("CampaignProducts", SiteContext.CurrentSiteName);
                if (library == null)
                {
                    CreateMeadiaLiabrary();
                    library = MediaLibraryInfoProvider.GetMediaLibraryInfo("CampaignProducts", SiteContext.CurrentSiteName);
                }
                filePath = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/" + filePath);
                CMS.IO.FileInfo file = CMS.IO.FileInfo.New(filePath);
                if (file != null)
                {
                    MediaFileInfo mediaFile = new MediaFileInfo(filePath, library.LibraryID);
                    mediaFile.FileName = Path.GetFileName(productImage.PostedFile.FileName);
                    mediaFile.FileTitle = Path.GetFileName(productImage.PostedFile.FileName);
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
        public static void CreateMeadiaLiabrary()
        {
            try
            {
                MediaLibraryInfo newLibrary = new MediaLibraryInfo();
                newLibrary.LibraryDisplayName = "Campaign Products";
                newLibrary.LibraryName = "CampaignProducts";
                newLibrary.LibraryFolder = "CampaignProducts";
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
        public static void DeleteImage(Guid imageGuid)
        {
            try
            {
                MediaLibraryInfo library = MediaLibraryInfoProvider.GetMediaLibraryInfo("CampaignProducts", SiteContext.CurrentSiteName);
                if (library != null)
                {
                    MediaFileInfo deleteFile = MediaFileInfoProvider.GetMediaFileInfo(imageGuid, SiteContext.CurrentSiteName);
                    if (deleteFile != null)
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