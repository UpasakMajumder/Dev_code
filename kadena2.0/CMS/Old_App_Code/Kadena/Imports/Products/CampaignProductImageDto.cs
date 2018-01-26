using System.ComponentModel.DataAnnotations;

namespace Kadena.Old_App_Code.Kadena.Imports.Products
{
    public class CampaignProductImageDto
    {
        [Header(0, "SKU")]
        [Required]
        [MaxLength(30)]
        public string SKU { get; set; }

        [Header(1, "Image URL")]
        [Required]
        [MaxLength(200)]
        public string ImageURL { get; set; }

        [Header(2, "Thumbnail URL")]
        [Required]
        [MaxLength(200)]
        public string ThumbnailURL { get; set; }
    }
}