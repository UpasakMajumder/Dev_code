using CMS.DocumentEngine;
using CMS.Ecommerce;
using CMS.MediaLibrary;
using CMS.SiteProvider;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace Kadena.Old_App_Code.Kadena.Imports.Products
{
    public static class SKUTreeNodeExtensions
    {
        /// <summary>
        /// Sets given <param name="imageUrl"></param> as SKUImagePath of product node
        /// </summary>
        public static void SetImage(this SKUTreeNode product, string imageUrl)
        {
            product.SetValue("SKUImagePath", imageUrl);
        }

        public static AttachmentInfo DownloadAttachmentThumbnail(this SKUTreeNode product, string fromUrl)
        {
            if (product.SKU == null)
            {
                throw new ArgumentNullException(nameof(product.SKU));
            }

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
                                throw new Exception("Thumbnail is not of image MIME type");
                            }

                            var stream = response.Content.ReadAsStreamAsync().Result;

                            var extension = Path.GetExtension(fromUrl);

                            if (string.IsNullOrEmpty(extension) && mimetype.StartsWith("image/"))
                            {
                                extension = mimetype.Split('/')[1];
                            }

                            // attach file as page attachment and set it's GUID as ProductThumbnail (of type guid) property of  Product
                            var newAttachment = new AttachmentInfo()
                            {
                                InputStream = stream,
                                AttachmentSiteID = product.NodeSiteID,
                                AttachmentDocumentID = product.DocumentID,
                                AttachmentExtension = extension,
                                AttachmentName = $"Thumbnail{product.SKU.SKUNumber}.{extension}",
                                AttachmentLastModified = DateTime.Now,
                                AttachmentMimeType = mimetype,
                                AttachmentSize = (int)stream.Length
                            };

                            AttachmentInfoProvider.SetAttachmentInfo(newAttachment);

                            return newAttachment;
                        }
                        else
                        {
                            throw new Exception("Failed to download thumbnail image");
                        }
                    }
                }
            }
        }

        public static void AttachThumbnail(this SKUTreeNode product, AttachmentInfo newAttachment)
        {
            if (newAttachment != null)
            {
                product.SetValue("ProductThumbnail", newAttachment.AttachmentGUID);
            }
        }

        public static void AttachThumbnail(this SKUTreeNode product, string fromUrl)
        {
            var newAttachment = DownloadAttachmentThumbnail(product, fromUrl);
            AttachThumbnail(product, newAttachment);
        }

        public static void RemoveTumbnail(this SKUTreeNode product)
        {
            var oldAttachmentGuid = product.GetGuidValue("ProductThumbnail", Guid.Empty);
            var siteName = (product.Site ?? SiteInfoProvider.GetSiteInfo(product.NodeSiteID)).SiteName;
            if (oldAttachmentGuid != Guid.Empty)
            {
                var oldAttachment = AttachmentInfoProvider.GetAttachmentInfo(oldAttachmentGuid, siteName);
                if (oldAttachment != null)
                {
                    AttachmentInfoProvider.DeleteAttachmentInfo(oldAttachment);
                }
            }
        }

        public static void RemoveImage(this SKUTreeNode product)
        {
            var oldImageUrl = product?.GetValue("SKUImagePath", string.Empty);

            MediaLibrary.RemoveMediaFile(oldImageUrl);
        }
    }
}