using CMS.EventLog;
using CMS.MediaLibrary;
using CMS.SiteProvider;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace Kadena.Old_App_Code.Kadena.Imports.Products
{
    public class MediaLibrary
    {
        private MediaLibraryInfo _mediaLibrary;

        public string LibraryName { get; set; }
        public string LibraryFolder { get; set; }
        public string LibraryDescription { get; set; }
        public int SiteId { get; set; }

        private void EnsureLibrary()
        {
            var siteName = SiteInfoProvider.GetSiteInfo(SiteId).SiteName;
            _mediaLibrary = MediaLibraryInfoProvider.GetMediaLibraryInfo(LibraryName, siteName);
            if (_mediaLibrary == null)
            {
                _mediaLibrary = new MediaLibraryInfo()
                {
                    LibraryDisplayName = LibraryName,
                    LibraryName = LibraryName,
                    LibraryDescription = LibraryDescription,
                    LibraryFolder = LibraryFolder,
                    LibrarySiteID = SiteId,
                    Access = CMS.Helpers.SecurityAccessEnum.AuthenticatedUsers
                };

                MediaLibraryInfoProvider.SetMediaLibraryInfo(_mediaLibrary);
            }
        }

        public string DownloadImageToMedialibrary(string fromUrl, string imageName, string imageDescription = null)
        {
            EnsureLibrary();
            if (_mediaLibrary == null)
            {
                EventLogProvider.LogEvent(EventType.ERROR, this.GetType().Name, "ENSUREOBJ", "Media library failed to ensure");
                return null;
            }
            MediaFileInfo mediaFile = null;

            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, fromUrl))
                {
                    using (var response = client.SendAsync(request).Result)
                    {
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
                                FilePath = $"{LibraryName}/",
                                FileExtension = extension,
                                FileMimeType = mimetype,
                                FileSiteID = SiteId,
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
            }

            return MediaFileInfoProvider.GetMediaFileAbsoluteUrl(mediaFile.FileGUID, mediaFile.FileName);
        }

        public static void RemoveMediaFile(string url)
        {
            if (string.IsNullOrWhiteSpace(url) || !url.Contains("/"))
            {
                return;
            }

            var fileName = url.Substring(url.LastIndexOf('/') + 1);
            if (!string.IsNullOrEmpty(fileName))
            {
                var oldImages = MediaFileInfoProvider.GetMediaFiles().WhereEquals("FileName", fileName).ToList();

                foreach (var oldImage in oldImages)
                {
                    MediaFileInfoProvider.DeleteMediaFile(oldImage.FileSiteID, oldImage.FileLibraryID, oldImage.FilePath);
                    MediaFileInfoProvider.DeleteMediaFileInfo(oldImage);
                }
            }
        }
    }
}