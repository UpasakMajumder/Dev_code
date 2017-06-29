using System;

namespace Kadena.WebAPI.Models.Checkout
{
    public class CartItem
    {
        public int Id { get; set; }
        public string ProductType { get; set; }
        public string Image { get; set; }
        public string Template { get; set; }
        public string EditorTemplateId { get; set; }
        public int ProductPageId { get; set; }
        public int SKUID { get;set;}
        public string SKUNumber { get; set; }
        public string SKUName { get; set; }
        public int LineNumber { get; set; }
        public bool IsMailingList
        {
            get
            {
                return ProductType.Contains("KDA.MailingProduct");
            }
        }
        public bool IsTemplated
        {
            get
            {
                return ProductType.Contains("KDA.TemplatedProduct");
            }
        }
        public bool IsInventory
        {
            get
            {
                return ProductType.Contains("KDA.InventoryProduct");
            }
        }
        public bool IsPOD
        {
            get
            {
                return ProductType.Contains("KDA.POD");
            }
        }
        public bool IsStatic
        {
            get
            {
                return ProductType.Contains("KDA.StaticProduct");
            }
        }
        public string MailingListName { get; set; }
        public Guid MailingListGuid { get; set; }
        public string Delivery { get; set; }
        public string PricePrefix { get; set; }
        public double UnitPrice { get; set; }
        public double TotalPrice { get; set; }
        public bool IsQuantityEditable
        {
            get
            {
                return IsInventory || IsPOD || IsStatic;
            }
        }
        public string QuantityPrefix { get; set; }
        public int Quantity { get; set; }
        public int StockQuantity { get; set; }
        public double TotalTax { get; set; }
        public string PriceText { get; set; }
        public bool IsEditable
        {
            get
            {
                return !IsMailingList && IsTemplated;
            }
        }

        public string EditorURL
        {
            get
            {
                return $"/products/product-tools/product-editor?id={ProductPageId}&skuid={SKUID}&templateid={EditorTemplateId}";
            }
        }

        /// <summary>
        /// Main Chilli template ID
        /// </summary>
        public Guid ChilliTemplateId { get; set; }

        /// <summary>
        /// Selected template instance ID
        /// </summary>
        public Guid ChilliEditorTemplateId { get; set; }

        public Guid ProductChilliPdfGeneratorSettingsId { get; set; }

        public string DesignFilePath { get; set; }

        /// <summary>
        /// Indicates if it is necessary to obtain design file path
        /// via calling Template product service
        /// </summary>
        public bool DesignFilePathRequired
        {
            get
            {
                return IsTemplated;
            }
        }

        public string UnitOfMeasure { get; set; }

               

        /// <summary>
        /// Template product service's task Id
        /// </summary>
        public string DesignFilePathTaskId { get; set; }

        /// <summary>
        /// indicates if DesignFilePath was already obtained
        /// </summary>
        public bool DesignFilePathObtained { get; set; } = false;
    }
}