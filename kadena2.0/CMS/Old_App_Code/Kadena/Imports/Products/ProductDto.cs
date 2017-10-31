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
        public string ProductName { get; set; }

        [Header(2, "SKU")]
        public string SKU { get; set; }

        [Header(3, "Price")]
        [Required]
        public string Price { get; set; }

        [Header(4, "Description")]
        public string Description { get; set; }

        [Header(5, "Representing")]
        
        public string Representing { get; set; }

        [Header(6, "Customer Reference Number")]
        public string CustomerReferenceNumber { get; set; }

        [Header(7, "Dynamic Price Min Items")]
        public string DynamicPriceMinItems { get; set; }

        [Header(8, "Dynamic Price Max Items")]
        public string DynamicPriceMaxItems { get; set; }

        [Header(9, "Dynamic Price")]
        public string DynamicPrice { get; set; }

        [Header(10, "Machine type")]
        public string MachineType { get; set; }

        [Header(11, "Color")]
        public string Color { get; set; }

        [Header(12, "Paper")]
        public string Paper { get; set; }

        [Header(13, "Sheet Size")]
        public string SheetSize { get; set; }

        [Header(14, "Trim Size")]
        public string TrimSize { get; set; }

        [Header(15, "Finished Size")]
        public string FinishedSize { get; set; }

        [Header(16, "Bindery")]
        public string Bindery { get; set; }

        [Header(17, "Product Type")]
        public string ProductType { get; set; }

        [Header(18, "Chili Template ID")]
        public string ChiliTemplateID { get; set; }

        [Header(19, "Chili Workgroup ID ")]
        public string ChiliWorkgroupID { get; set; }

        [Header(20, "Product Chili Pdf generator SettingsId")]
        public string ChiliPdfGeneratorSettingsID { get; set; }

        [Header(21, "Production time")]
        public string ProductionTime { get; set; }

        [Header(22, "Ship Time")]
        public string ShipTime { get; set; }

        [Header(23, "Shipping cost")]
        public string ShippingCost { get; set; }

        [Header(24, "Number of items in package")]
        [Required]
        public string ItemsInPackage { get; set; }

        [Header(25, "Package weight")]
        [Required]
        public string PackageWeight { get; set; }

        [Header(26, "Needs shipping")]
        public string NeedsShipping{ get; set; }

        [Header(27, "Publish from")]
        public string PublishFrom { get; set; }

        [Header(28, "Publish to")]
        public string PublishTo { get; set; }

        [Header(29, "Track Inventory")]
        public string TrackInventory { get; set; }
    }
}