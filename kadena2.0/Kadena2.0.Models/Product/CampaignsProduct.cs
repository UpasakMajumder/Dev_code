using System;

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
        public int BrandId { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public int StateId { get; set; }
        public int NumberOfItemsInPackage { get; set; }
        public string ImageUrl { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
