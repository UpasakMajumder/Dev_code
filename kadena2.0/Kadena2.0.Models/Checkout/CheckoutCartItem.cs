using Kadena.Models.Common;
using Kadena.Models.Product;
using System;
using System.Collections.Generic;

namespace Kadena.Models.Checkout
{
    public class CheckoutCartItem
    {
        public int Id { get; set; }
        public string CartItemText { get; set; }
        public string ProductType { get; set; }
        public string Image { get; set; }
        public string Template { get; set; }
        public int ProductPageId { get; set; }
        public string SKUName { get; set; }
        public string SKUNumber { get; set; }

        public bool IsMailingList
        {
            get
            {
                return ProductType.Contains(ProductTypes.MailingProduct);
            }
        }
        public bool IsTemplated
        {
            get
            {
                return ProductType.Contains(ProductTypes.TemplatedProduct);
            }
        }
        public bool IsInventory
        {
            get
            {
                return ProductType.Contains(ProductTypes.InventoryProduct);
            }
        }
        public bool IsPOD
        {
            get
            {
                return ProductType.Contains(ProductTypes.POD);
            }
        }
        public bool IsStatic
        {
            get
            {
                return ProductType.Contains(ProductTypes.StaticProduct);
            }
        }
        public string MailingListName { get; set; }
        public Guid MailingListGuid { get; set; }
        public string Delivery
        {
            get
            {
                return IsMailingList && IsTemplated ? $"Delivery to {Quantity} addresses" : string.Empty;
            }
        }
        public string PricePrefix { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
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
        public decimal TotalTax { get; set; }
        public string PriceText { get; set; }
        

        public Button Preview { get; set; }
        public Button EmailProof { get; set; }

        public bool IsEditable
        {
            get
            {
                return !IsMailingList && IsTemplated;
            }
        }

        public string EditorURL { get; set; }
        public string UnitOfMeasureName { get; set; }
        public string MailingListPrefix { get; set; }
        public string TemplatePrefix { get; set; }
        public string ProductionTime { get; set; }
        public string ShipTime { get; set; }
        public IEnumerable<ItemOption> Options { get; set; }
        public string CustomName { get; set; }
        public bool RequiresApproval { get; set; }
        public bool HiResPdfAllowed { get; set; }
    }
}