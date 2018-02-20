namespace Kadena.Models.IBTF
{
    public class IBTFAdjustment
    {
        public int ItemID { get; set; }
        public int UserID { get; set; }
        public int CampaignID { get; set; }
        public int SKUID { get; set; }
        public int OrderedQuantity { get; set; }
        public decimal OrderedProductPrice { get; set; }
    }
}
