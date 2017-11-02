using System.ComponentModel.DataAnnotations;

namespace Kadena.Old_App_Code.Kadena.Imports.Products
{
    public class ProductDto
    {
        [Header(0, "Product Category")]
        [Required]
        public string ProductCategory { get; set; }

        [Header(1, "Product Name")]
        [Required]
        [MaxLength(100)]
        public string ProductName { get; set; }

        [Header(2, "SKU")]
        [Required]
        [MaxLength(30)]
        public string SKU { get; set; }

        [Header(3, "Price")]
        [Required]
        [MaxLength(30)]
        public string Price { get; set; }

        [Header(4, "Description")]
        public string Description { get; set; }

        [Header(5, "Product Type")]
        [Required]
        [MaxLength(250)]
        public string ProductType { get; set; }

        [Header(6, "Customer Reference Number")]
        [MaxLength(100)]
        public string CustomerReferenceNumber { get; set; }

        [Header(7, "Dynamic Price Min Items")]
        public string DynamicPriceMinItems { get; set; }

        [Header(8, "Dynamic Price Max Items")]
        public string DynamicPriceMaxItems { get; set; }

        [Header(9, "Dynamic Price")]
        public string DynamicPrice { get; set; }

        [Header(10, "Machine type")]
        [MaxLength(100)]
        public string MachineType { get; set; }

        [Header(11, "Color")]
        [MaxLength(100)]
        public string Color { get; set; }

        [Header(12, "Paper")]
        [MaxLength(100)]
        public string Paper { get; set; }

        [Header(13, "Sheet Size")]
        [MaxLength(100)]
        public string SheetSize { get; set; }

        [Header(14, "Trim Size")]
        [MaxLength(100)]
        public string TrimSize { get; set; }

        [Header(15, "Finished Size")]
        [MaxLength(100)]
        public string FinishedSize { get; set; }

        [Header(16, "Bindery")]
        [MaxLength(100)]
        public string Bindery { get; set; }

        [Header(17, "Chili Template ID")]
        [MaxLength(40)]
        public string ChiliTemplateID { get; set; }

        [Header(18, "Chili Workgroup ID ")]
        [MaxLength(40)]
        public string ChiliWorkgroupID { get; set; }

        [Header(19, "Product Chili Pdf generator SettingsId")]
        [MaxLength(40)]
        public string ChiliPdfGeneratorSettingsID { get; set; }

        [Header(20, "Production time")]
        [MaxLength(50)]
        public string ProductionTime { get; set; }

        [Header(21, "Ship Time")]
        [MaxLength(50)]
        public string ShipTime { get; set; }

        [Header(22, "Shipping cost")]
        [MaxLength(20)]
        public string ShippingCost { get; set; }

        [Header(23, "Number of items in package")]
        [Required]
        [MaxLength(20)]
        public string ItemsInPackage { get; set; }

        [Header(24, "Package weight")]
        [Required]
        [MaxLength(20)]
        public string PackageWeight { get; set; }

        [Header(25, "Needs shipping")]
        [MaxLength(10)]
        public string NeedsShipping{ get; set; }

        [Header(26, "Publish from")]
        [MaxLength(20)]
        public string PublishFrom { get; set; }

        [Header(27, "Publish to")]
        [MaxLength(20)]
        public string PublishTo { get; set; }

        [Header(28, "Track Inventory")]
        [MaxLength(20)]
        public string TrackInventory { get; set; }
    }
}