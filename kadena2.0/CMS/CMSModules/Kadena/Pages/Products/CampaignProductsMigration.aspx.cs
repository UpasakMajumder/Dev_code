using CMS.DocumentEngine.Types.KDA;
using CMS.EventLog;
using CMS.MediaLibrary;
using CMS.SiteProvider;
using CMS.UIControls;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Kadena.CMSModules.Kadena.Pages.Products
{
    public partial class CampaignProductsMigration : CMSPage
    {
        private int CurrentSiteID = SiteContext.CurrentSiteID;

        protected void Page_Load(object sender, EventArgs e)
        {
            MediaLibrarySelector.SiteID = CurrentSiteID;
            HideResultMessage();
        }

        private void ShowErrorMessage(string message)
        {
            errorMessageContainer.Visible = true;
            errorMessage.Text = message;
        }

        private void ShowSuccessMessage(string message)
        {
            successMessageContainer.Visible = true;
            successMessage.Text = message;
        }

        private void HideResultMessage()
        {
            errorMessageContainer.Visible = false;
            successMessageContainer.Visible = false;
        }

        protected void btnMoveSKUToPageType_Click(object sender, EventArgs e)
        {
            try
            {
                CampaignsProductProvider.GetCampaignsProducts()
                        .Published(false)
                        .OnSite(CurrentSiteID)
                        .Where(x => string.IsNullOrWhiteSpace(x.ProductImage) && !string.IsNullOrWhiteSpace(x.SKU.SKUImagePath))
                        ?.ToList()
                        .ForEach(p =>
                        {
                            p.ProductImage = p.SKU.SKUImagePath;
                            p.Update();
                        });
                ShowSuccessMessage("Images moved successfully to page type");
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
                EventLogProvider.LogException(GetType().Name, "EXCEPTION", ex);
            }
        }

        protected void btnMoveToS3_Click(object sender, EventArgs e)
        {
            MediaLibraryInfo mediaLibrary = MediaLibraryInfoProvider.GetMediaLibraryInfo(MediaLibrarySelector.MediaLibraryID);
            if (mediaLibrary == null)
            {
                ShowErrorMessage("Please select target media library");
                return;
            }
            CampaignsProductProvider.GetCampaignsProducts()
                        .Published(false)
                        .OnSite(CurrentSiteID)
                        .Where(x => string.IsNullOrWhiteSpace(x.ProductImage) && !string.IsNullOrWhiteSpace(x.SKU.SKUImagePath))
                        ?.ToList()
                        .ForEach(p =>
                        {
                            MigrateImage(p, mediaLibrary);
                        });
            ShowSuccessMessage("Images moved successfully to S3 and to page type");
        }

        private void MigrateImage(CampaignsProduct product, MediaLibraryInfo mediaLibrary)
        {
            string imgUrl = GetImageFullURL(product.SKU.SKUImagePath);
            if (string.IsNullOrWhiteSpace(imgUrl))
            {
                return;
            }

            MediaFileInfo objMediaFileInfo = null;
            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, imgUrl))
                {
                    using (var response = client.SendAsync(request).Result)
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var mimetype = response.Content?.Headers?.ContentType?.MediaType ?? string.Empty;

                            if (!mimetype.StartsWith("image/"))
                            {
                                return;
                            }

                            var stream = response.Content.ReadAsStreamAsync().Result;

                            var extension = Path.GetExtension(imgUrl);
                            var fileName = Path.GetFileNameWithoutExtension(imgUrl);
                            if (string.IsNullOrEmpty(extension) && mimetype.StartsWith("image/"))
                            {
                                extension = mimetype.Split('/')[1];
                            }

                            Image image = Image.FromStream(stream);
                            string tempImagefilePath = HttpContext.Current.Server.MapPath("~/Uploads/") + Path.GetFileName(imgUrl);
                            image.Save(tempImagefilePath);
                            tempImagefilePath = MediaLibraryHelper.EnsurePhysicalPath(tempImagefilePath);

                            CMS.IO.FileInfo file = CMS.IO.FileInfo.New(tempImagefilePath);
                            if (file != null)
                            {
                                objMediaFileInfo = new MediaFileInfo(tempImagefilePath, mediaLibrary.LibraryID);
                                objMediaFileInfo.FileName = fileName;
                                objMediaFileInfo.FilePath = objMediaFileInfo.FileName;
                                objMediaFileInfo.FileSiteID = SiteContext.CurrentSiteID;
                                objMediaFileInfo.FileLibraryID = mediaLibrary.LibraryID;
                                objMediaFileInfo.FileExtension = extension.Contains(".") ? extension : "." + extension;
                                objMediaFileInfo.FileMimeType = "image/jpeg";
                                MediaFileInfoProvider.SetMediaFileInfo(objMediaFileInfo);
                                File.Delete(tempImagefilePath);
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                }
            }

            if (objMediaFileInfo != null)
            {
                product.ProductImage = MediaFileURLProvider.GetMediaFileUrl(objMediaFileInfo.FileGUID, objMediaFileInfo.FileName);
                product.Update();
            }
        }

        private static string GetImageFullURL(string url)
        {
            string imageFullURL = string.Empty;
            if (!string.IsNullOrEmpty(url))
            {
                Uri imgURL = null;
                if (Uri.TryCreate(url, UriKind.Absolute, out imgURL))
                {
                    imageFullURL = url;
                }
                else if (url.Contains("/getmedia/"))
                {
                    string strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
                    imageFullURL = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "") + url.Trim('~');
                }
            }
            return imageFullURL;
        }
    }
}