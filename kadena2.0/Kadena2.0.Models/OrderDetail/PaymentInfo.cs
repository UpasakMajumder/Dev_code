using System;

namespace Kadena.Models.OrderDetail
{
    public class PaymentInfo
    {
        public string Title { get; set; }
        public string PaymentIcon { get; set; }
        public string PaidBy { get; set; }
        public string PaymentDetail { get; set; }
        public string DatePrefix { get; set; }
        public DateTime? Date { get; set; }

    }
}