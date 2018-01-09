using System.ComponentModel.DataAnnotations;

namespace Kadena.Old_App_Code.Kadena.Imports.Products
{
    public class CampaignProductDto
    {
        [Header(0, "Campaign Name")]
        public string Campaign { get; set; }

        [Header(1, "Program Name")]
        [MaxLength(100)]
        public string ProgramName { get; set; }

        [Header(2, "POS Number *")]
        [Required]
        [MaxLength(30)]
        public string SKU { get; set; }

        [Header(3, "Cenveo ID")]
        [MaxLength(30)]
        public string CenveoID { get; set; }

        [Header(4, "Product Name *")]
        [Required]
        public string ProductName { get; set; }

        [Header(5, "Allowed States Group ID")]
        public string AllowedStates { get; set; }

        [Header(6, "Long Description")]
        public string LongDescription { get; set; }

        [Header(7, "Product Expiry Date (MM/dd/yyyy) *")]
        [MaxLength(20)]
        [Required]
        public string SKUValidUntil { get; set; }

        [Header(8, "Brand *")]
        [Required]
        [MaxLength(100)]
        public string Brand { get; set; }

        [Header(9, "Estimated Price")]
        [MaxLength(30)]
        public string EstimatedPrice { get; set; }

        [Header(10, "Actual Price")]
        [MaxLength(30)]
        public string ActualPrice { get; set; }

        [Header(11, "Product Category (Type) *")]
        [Required]
        [MaxLength(100)]
        public string ProductCategory { get; set; }

        [Header(12, "Bundle Quantity *")]
        [Required]
        [MaxLength(30)]
        public string BundleQuantity { get; set; }

        [Header(13, "Total Quantity")]
        [MaxLength(30)]
        public string TotalQuantity { get; set; }

        [Header(14, "Product Weight *")]
        [MaxLength(30)]
        [Required]
        public string ProductWeight { get; set; }

        [Header(15, "Status *")]
        [Required]
        [MaxLength(20)]
        public string Status { get; set; }

        [Header(16, "Item Specs ID")]
        public string ItemSpecsID { get; set; }

        [Header(17, "Image URL")]
        [MaxLength(200)]
        public string ImageURL { get; set; }

        [Header(18, "Thumbnail URL")]
        [MaxLength(200)]
        public string ThumbnailURL { get; set; }
    }
}