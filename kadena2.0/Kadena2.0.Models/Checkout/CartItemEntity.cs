using System;

namespace Kadena.Models.Checkout
{
    public class CartItemEntity
    {
        public int CartItemID { get; set; }
        public Guid CartItemGUID { get; set; }
        public int ShoppingCartID { get; set; }
        public int SKUID { get; set; }
        public int SKUUnits { get; set; }
        public decimal? CartItemPrice { get; set; }
        public string CartItemText { get; set; }
        public string ProductType { get; set; }
        public Guid ChiliTemplateID { get; set; }
        public string ArtworkLocation { get; set; }
        public int ProductPageID { get; set; }
        public Guid ChilliEditorTemplateID { get; set; }
        public string MailingListName { get; set; }
        public Guid? MailingListGuid { get; set; }
        public Guid ProductChiliWorkspaceId { get; set; }
        public Guid ProductChiliPdfGeneratorSettingsId { get; set; }
        public string ProductShipTime { get; set; }
        public string ProductProductionTime { get; set; }
        public bool SendPriceToErp { get; set; }
    }
}
