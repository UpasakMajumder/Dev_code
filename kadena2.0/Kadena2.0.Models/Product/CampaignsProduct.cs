namespace Kadena.Models.Product
{
    public class CampaignsProduct
    {
        public int SKUID { get; set; }
        public string SKUNumber { get; set; }
        public string ProductName { get; set; }
        public decimal EstimatedPrice { get; set; }
        public decimal ActualPrice { get; set; }
        public int ProgramID { get; set; }
        public int CampaignID { get; set; }
        public string POSNumber { get; set; }
        public int DocumentId { get; set; }
    }
}
