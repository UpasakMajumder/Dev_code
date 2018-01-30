using System.ComponentModel.DataAnnotations;

namespace Kadena.Old_App_Code.Kadena.Imports.Products
{
    public class ProductImageDto
    {
        [Header(0, "SKU")]
        [Required]
        [MaxLength(30)]
        public string SKU { get; set; }

        [Header(1, "Image Media Library")]
        [Required]
        [MaxLength(200)]
        public string ImageMediaLibraryName { get; set; }

        [Header(2, "Image Name")]
        [Required]
        [MaxLength(200)]
        public string ImageName { get; set; }
    }
}