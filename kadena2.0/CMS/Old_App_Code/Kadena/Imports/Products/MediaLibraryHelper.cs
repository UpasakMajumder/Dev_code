using CMS.Ecommerce;
using CMS.MediaLibrary;
using CMS.SiteProvider;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace Kadena.Old_App_Code.Kadena.Imports.Products
{
    public class MediaLibraryHelper
    {
        private MediaLibraryInfo _mediaLibrary;

        private string _name;
        private int _siteId;

        public MediaLibraryHelper(int siteId, string name)
        {
            _siteId = siteId;
            _name = name;
        }

        public void EnsureLibrary(string libraryFolder, string libraryDescription = null)
        {
            var siteName = SiteInfoProvider.GetSiteInfo(_siteId).SiteName;
            _mediaLibrary = MediaLibraryInfoProvider.GetMediaLibraryInfo(_name, siteName);
            if (_mediaLibrary == null)
            {
                _mediaLibrary = new MediaLibraryInfo()
                {
                    LibraryDisplayName = _name,
                    LibraryName = _name,
                    LibraryDescription = libraryDescription,
                    LibraryFolder = libraryFolder,
                    LibrarySiteID = _siteId,
                    Access = CMS.Helpers.SecurityAccessEnum.AuthenticatedUsers
                };

                MediaLibraryInfoProvider.SetMediaLibraryInfo(_mediaLibrary);
            }
        }

        public string DownloadImageToMedialibrary(string fromUrl, string imageName, string imageDescription = null)
        {            
            MediaFileInfo mediaFile = null;

            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, fromUrl))
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

                        var img = Image.FromStream(stream);
                        stream.Seek(0, SeekOrigin.Begin);
                        var extension = Path.GetExtension(fromUrl);

                        if (string.IsNullOrEmpty(extension) && mimetype.StartsWith("image/"))
                        {
                            extension = $".{mimetype.Split('/')[1]}";
                        }

                        mediaFile = new MediaFileInfo()
                        {
                            FileBinaryStream = stream,
                            FileName = imageName,
                            FileTitle = imageName,
                            FileDescription = imageDescription,
                            FilePath = $"{_name}/",
                            FileExtension = extension,
                            FileMimeType = mimetype,
                            FileSiteID = _siteId,
                            FileLibraryID = _mediaLibrary.LibraryID,
                            FileSize = stream.Length,
                            FileImageHeight = img.Height,
                            FileImageWidth = img.Width
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

        public static void DeleteProductImage(SKUTreeNode product)
        {
            var oldImageUrl = product?.GetValue("SKUImagePath", string.Empty);

            if (string.IsNullOrEmpty(oldImageUrl) || !oldImageUrl.Contains("/"))
            {
                return;
            }

            var fileName = oldImageUrl.Substring(oldImageUrl.LastIndexOf('/')+1);
            if (!string.IsNullOrEmpty(fileName))
            {
                var oldImages = MediaFileInfoProvider.GetMediaFiles().WhereEquals("FileName", fileName).ToList();

                foreach(var oldImage in oldImages)
                {
                    MediaFileInfoProvider.DeleteMediaFile(oldImage.FileSiteID, oldImage.FileLibraryID, oldImage.FilePath);
                    MediaFileInfoProvider.DeleteMediaFileInfo(oldImage);
                }
            }
        }
    }
}