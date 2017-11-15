using CMS.Ecommerce;
using CMS.MediaLibrary;
using CMS.SiteProvider;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace Kadena.Old_App_Code.Kadena.Imports.Products
{
    public static class MediaLibraryHelper
    {
        public static MediaLibraryInfo EnsureLibrary(int siteId)
        {
            string libraryName = "ProductImages";
            var siteName = SiteInfoProvider.GetSiteInfo(siteId).SiteName;
            var meidaLibrary = MediaLibraryInfoProvider.GetMediaLibraryInfo(libraryName, siteName);
            if (meidaLibrary == null)
            {
                meidaLibrary = new MediaLibraryInfo();

                // Sets the library properties
                meidaLibrary.LibraryDisplayName = libraryName;
                meidaLibrary.LibraryName = libraryName;
                meidaLibrary.LibraryDescription = "Media library for storing product images";
                meidaLibrary.LibraryFolder = $"/Products";
                meidaLibrary.LibrarySiteID = SiteContext.CurrentSiteID;

                // Saves the new media library to the database
                MediaLibraryInfoProvider.SetMediaLibraryInfo(meidaLibrary);
            }

            return meidaLibrary;
        }

        public static string DownloadImageToMedialibrary(string url, string skuNumber, int documentId, int libraryId, int siteId)
        {            
            MediaFileInfo mediaFile = null;

            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, url))
                {
                    var response = client.SendAsync(request).Result;

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var mimetype = response.Content?.Headers?.ContentType?.MediaType ?? string.Empty;

                        if (!mimetype.StartsWith("image/"))
                        {
                            throw new Exception("Image is not of image MIME type");
                        }

                        var stream = response.Content.ReadAsStreamAsync().Result;
                        var imageName = $"Image{skuNumber}";
                        var extension = Path.GetExtension(url);

                        if (string.IsNullOrEmpty(extension) && mimetype.StartsWith("image/"))
                        {
                            extension = $".{mimetype.Split('/')[1]}";
                        }

                        mediaFile = new MediaFileInfo()
                        {
                            FileBinaryStream = stream,
                            FileName = imageName,
                            FileTitle = imageName,
                            FileDescription = $"Product image for SKU {skuNumber}",
                            FilePath = "ProductImages/",
                            FileExtension = extension,
                            FileMimeType = mimetype,
                            FileSiteID = siteId,
                            FileLibraryID = libraryId,
                            FileSize = stream.Length,
                        };

                        MediaFileInfoProvider.SetMediaFileInfo(mediaFile);
                    }
                    else
                    {
                        throw new Exception("Failed to download product image");
                    }
                }
            }

            return $"/getmedia/{mediaFile?.FileGUID.ToString()}/{mediaFile?.FileName}";
        }

        public static void DeleteProductImage(SKUTreeNode product, int libraryId, int siteId)
        {
            var oldImageUrl = product?.GetValue("SKUImagePath", string.Empty);

            if (string.IsNullOrEmpty(oldImageUrl) || !oldImageUrl.Contains("/"))
                return;

            var fileName = oldImageUrl.Substring(oldImageUrl.LastIndexOf('/')+1);
            if (!string.IsNullOrEmpty(fileName))
            {
                var oldImages = MediaFileInfoProvider.GetMediaFiles().WhereEquals("FileName", fileName).ToList();

                foreach(var oldImage in oldImages)
                {
                    MediaFileInfoProvider.DeleteMediaFile(siteId, libraryId, oldImage.FilePath);
                    MediaFileInfoProvider.DeleteMediaFileInfo(oldImage);
                }
            }
        }
    }
}