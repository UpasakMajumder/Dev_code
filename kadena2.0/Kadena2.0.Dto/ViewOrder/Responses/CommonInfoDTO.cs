using System;

namespace Kadena.Dto.ViewOrder.Responses
{
    public class CommonInfoDTO
    {
        public TitleValuePairDto<string> Status { get; set; }
        public TitleValuePairDto<DateTime> OrderDate { get; set; }
        public TitleValuePairDto<DateTime?> ShippingDate { get; set; }
        public TitleValuePairDto<string> TotalCost { get; set; }
    }
}