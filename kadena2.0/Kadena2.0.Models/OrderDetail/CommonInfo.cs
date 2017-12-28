using System;

namespace Kadena.Models.OrderDetail
{
    public class CommonInfo
    {
        public TitleValuePair<string> Status { get; set; }
        public TitleValuePair<DateTime> OrderDate { get; set; }
        public TitleValuePair<DateTime?> ShippingDate { get; set; }
        public TitleValuePair<string> TotalCost { get; set; }
    }
}