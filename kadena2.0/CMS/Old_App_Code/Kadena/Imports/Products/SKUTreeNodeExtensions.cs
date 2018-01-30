using CMS.Ecommerce;
using CMS.MediaLibrary;

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

        public static void SetImage(this SKUTreeNode product, MediaFileInfo image)
        {
            product.SetImage(MediaFileInfoProvider.GetMediaFileUrl(image.FileGUID, image.FilePath));
        }

        public static void RemoveImage(this SKUTreeNode product)
        {
            product?.SetValue("SKUImagePath", null);
        }
    }
}