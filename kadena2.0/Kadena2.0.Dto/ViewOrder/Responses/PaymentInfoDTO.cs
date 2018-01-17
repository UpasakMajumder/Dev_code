using System;

namespace Kadena.Dto.ViewOrder.Responses
{
    public class PaymentInfoDTO
    {
        public string Title { get; set; }
        public string PaymentIcon { get; set; }
        public string PaidBy { get; set; }
        public string PaymentDetail { get; set; }
        public string DatePrefix { get; set; }
        public DateTime? Date { get; set; }
        public string BUnitLabel { get; set; }
        public string BUnitName { get; set; }
    }
}