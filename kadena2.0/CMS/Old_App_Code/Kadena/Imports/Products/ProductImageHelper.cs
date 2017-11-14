using CMS.DocumentEngine;
using CMS.Ecommerce;
using CMS.SiteProvider;
using System;
using System.IO;
using System.Net.Http;

namespace Kadena.Old_App_Code.Kadena.Imports.Products
{
    public static class ProductImageHelper
    {
        /// <summary>
        /// Sets given <param name="imageUrl"></param> as SKUImagePath of product node
        /// </summary>
        public static void SetProductImage(SKUTreeNode product, string imageUrl)
        {            
            product.SetValue("SKUImagePath", imageUrl);
        }

        public static AttachmentInfo DownloadAttachmentThumbnail(string url, string skuNumber, int documentId, int siteId)
        {
            AttachmentInfo newAttachment = null;

            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, url))
                {

                    var response = client.SendAsync(request).Result;

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var stream = response.Content.ReadAsStreamAsync().Result;
                        var extension = Path.GetExtension(url);

                        // attach file as page attachment and set it's GUID as ProductThumbnail (of type guid) property of  Product
                        newAttachment = new AttachmentInfo()
                        {
                            InputStream = stream,
                            AttachmentSiteID = siteId,
                            AttachmentDocumentID = documentId,
                            AttachmentExtension = extension,
                            AttachmentName = $"Thumbnail{skuNumber}{extension}",
                            AttachmentLastModified = DateTime.Now,
                            AttachmentMimeType = response.Content.Headers.ContentType.MediaType,
                            AttachmentSize = (int)stream.Length
                        };

                    }
                    else
                    {
                        throw new Exception("Failed to download thumbnail image");
                    }

                }
            }

            if (newAttachment != null)
            {
                AttachmentInfoProvider.SetAttachmentInfo(newAttachment);
            }

            return newAttachment;
        }

        public static void AttachTumbnail(SKUTreeNode product, AttachmentInfo newAttachment)
        {
            if (newAttachment != null)
            {
                product.SetValue("ProductThumbnail", newAttachment.AttachmentGUID);
            }
        }

        public static void RemoveTumbnail(SKUTreeNode product, int siteId)
        {
            var oldAttachmentGuid = product.GetGuidValue("ProductThumbnail", Guid.Empty);
            var siteName = SiteInfoProvider.GetSiteInfo(siteId).SiteName;
            if (oldAttachmentGuid != Guid.Empty)
            {
                var oldAttachment = AttachmentInfoProvider.GetAttachmentInfo(oldAttachmentGuid, siteName);
                if (oldAttachment != null)
                {
                    AttachmentInfoProvider.DeleteAttachmentInfo(oldAttachment);
                }
            }
        }
    }
}