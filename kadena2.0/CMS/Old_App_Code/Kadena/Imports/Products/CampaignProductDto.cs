using System.ComponentModel.DataAnnotations;

namespace Kadena.Old_App_Code.Kadena.Imports.Products
{
    public class CampaignProductDto
    {
        [Header(0, "Campagin Name")]
        public string Campagin { get; set; }

        [Header(1, "Program Name")]
        [MaxLength(100)]
        public string ProgramName { get; set; }

        [Header(2, "POS Number *")]
        [Required]
        [MaxLength(30)]
        public string SKU { get; set; }

        [Header(3, "Product Name *")]
        [Required]
        public string ProductName { get; set; }

        [Header(4, "Allowed States Group ID")]
        public string AllowedStates { get; set; }

        [Header(5, "Long Description")]
        public string LongDescription { get; set; }

        [Header(6, "Product Expiry Date (MM/dd/yyyy)")]
        [MaxLength(20)]
        public string SKUValidUntil { get; set; }

        [Header(7, "Brand *")]
        [Required]
        [MaxLength(100)]
        public string Brand { get; set; }

        [Header(8, "Estimated Price")]
        [MaxLength(30)]
        public string EstimatedPrice { get; set; }

        [Header(9, "Actual Price")]
        [MaxLength(30)]
        public string ActualPrice { get; set; }

        [Header(10, "Product Category (Type) *")]
        [Required]
        [MaxLength(100)]
        public string ProductCategory { get; set; }

        [Header(11, "Bundle Quantity *")]
        [Required]
        [MaxLength(30)]
        public string BundleQuantity { get; set; }

        [Header(12, "Total Quantity")]
        [MaxLength(30)]
        public string TotalQuantity { get; set; }

        [Header(13, "Product Weight *")]
        [MaxLength(30)]
        [Required]
        public string ProductWeight { get; set; }

        [Header(14, "Status *")]
        [Required]
        [MaxLength(20)]
        public string Status { get; set; }

        [Header(15, "Item Specs ID")]
        public string ItemSpecsID { get; set; }

        [Header(16, "Image URL")]
        [MaxLength(200)]
        public string ImageURL { get; set; }

        [Header(17, "Thumbnail URL")]
        [MaxLength(200)]
        public string ThumbnailURL { get; set; }
    }
}