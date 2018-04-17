using System;

namespace Kadena.Models.Orders.Failed
{
    public class FailedOrder
    {
        public string Id { get; set; }
        public string SiteName { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public int SubmissionAttemptsCount { get; set; }
        public string OrderStatus { get; set; }
    }
}
