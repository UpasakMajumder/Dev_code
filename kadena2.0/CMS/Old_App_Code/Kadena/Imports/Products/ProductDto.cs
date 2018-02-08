using System.ComponentModel.DataAnnotations;

namespace Kadena.Old_App_Code.Kadena.Imports.Products
{
    public class ProductDto
    {
        [Header(0, "Product Category *")]
        [Required]
        public string ProductCategory { get; set; }

        [Header(1, "Product Name *")]
        [Required]
        [MaxLength(100)]
        public string ProductName { get; set; }

        [Header(2, "SKU *")]
        [Required]
        [MaxLength(30)]
        public string SKU { get; set; }

        [Header(3, "Price *")]
        [Required]
        [MaxLength(30)]
        public string Price { get; set; }

        [Header(4, "Description")]
        public string Description { get; set; }

        [Header(5, "Product Type *")]
        [Required]
        [MaxLength(250)]
        public string ProductType { get; set; }

        [Header(6, "Chili 3D Enabled")]
        public string Chili3DEnabled { get; set; }

        [Header(7, "Customer Reference Number")]
        [MaxLength(100)]
        public string CustomerReferenceNumber { get; set; }

        [Header(8, "Dynamic Price Min Items")]
        public string DynamicPriceMinItems { get; set; }

        [Header(9, "Dynamic Price Max Items")]
        public string DynamicPriceMaxItems { get; set; }

        [Header(10, "Dynamic Price")]
        public string DynamicPrice { get; set; }

        [Header(11, "Machine type")]
        [MaxLength(100)]
        public string MachineType { get; set; }

        [Header(12, "Color")]
        [MaxLength(100)]
        public string Color { get; set; }

        [Header(13, "Paper")]
        [MaxLength(100)]
        public string Paper { get; set; }

        [Header(14, "Sheet Size")]
        [MaxLength(100)]
        public string SheetSize { get; set; }

        [Header(15, "Trim Size")]
        [MaxLength(100)]
        public string TrimSize { get; set; }

        [Header(16, "Finished Size")]
        [MaxLength(100)]
        public string FinishedSize { get; set; }

        [Header(17, "Bindery")]
        [MaxLength(100)]
        public string Bindery { get; set; }

        [Header(18, "Chili Template ID")]
        [MaxLength(40)]
        public string ChiliTemplateID { get; set; }

        [Header(19, "Chili Workgroup ID")]
        [MaxLength(40)]
        public string ChiliWorkgroupID { get; set; }

        [Header(20, "Product Chili Pdf generator SettingsId")]
        [MaxLength(40)]
        public string ChiliPdfGeneratorSettingsID { get; set; }

        [Header(21, "Production time")]
        [MaxLength(50)]
        public string ProductionTime { get; set; }

        [Header(22, "Ship Time")]
        [MaxLength(50)]
        public string ShipTime { get; set; }

        [Header(23, "Shipping cost")]
        [MaxLength(20)]
        public string ShippingCost { get; set; }

        [Header(24, "Number of items in package *")]
        [Required]
        [MaxLength(20)]
        public string ItemsInPackage { get; set; }

        [Header(25, "Package weight *")]
        [Required]
        [MaxLength(20)]
        public string PackageWeight { get; set; }

        [Header(26, "Needs shipping")]
        [MaxLength(10)]
        public string NeedsShipping { get; set; }

        [Header(27, "Publish from (MM/dd/yyyy)")]
        [MaxLength(20)]
        public string PublishFrom { get; set; }

        [Header(28, "Publish to (MM/dd/yyyy)")]
        [MaxLength(20)]
        public string PublishTo { get; set; }

        [Header(29, "Track Inventory")]
        [MaxLength(20)]
        public string TrackInventory { get; set; }

        [Header(30, "Sell only if items available")]
        [MaxLength(20)]
        public string SellOnlyIfItemsAvailable { get; set; }

        [Header(31, "Min Items in Order")]
        [MaxLength(20)]
        public string MinItemsInOrder { get; set; }

        [Header(32, "Max Items in Order")]
        [MaxLength(20)]
        public string MaxItemsInOrder { get; set; }

        [Header(33, "Media Library Name")]
        [MaxLength(200)]
        public string ImageMediaLibraryName { get; set; }

        [Header(34, "Image Path")]
        [MaxLength(200)]
        public string ImagePath { get; set; }
    }
}