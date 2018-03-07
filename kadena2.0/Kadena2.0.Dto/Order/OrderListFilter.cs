using System;

namespace Kadena.Dto.Order
{
    public struct OrderListFilter
    {
        public string SiteName { get; set; }
        public int? CustomerId { get; set; }
        public int? PageNumber { get; set; }
        public int? ItemsPerPage { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string SortBy { get; set; }
        public bool SortDescending { get; set; }
        public int? CampaignId { get; set; }
        public int? ProgramId { get; set; }
        public int? DistributorId { get; set; }
        public string OrderType { get; set; }
    }
}
