using System.ComponentModel.DataAnnotations;

namespace Kadena.Old_App_Code.Kadena.Imports.Products
{
    public class CampaignProductImageDto
    {
        [Header(0, "SKU")]
        [Required]
        [MaxLength(30)]
        public string SKU { get; set; }

        [Header(1, "Media Library Name")]
        [Required]
        [MaxLength(200)]
        public string ImageMediaLibraryName { get; set; }

        [Header(2, "Image Path")]
        [Required]
        [MaxLength(200)]
        public string ImagePath { get; set; }
    }
}