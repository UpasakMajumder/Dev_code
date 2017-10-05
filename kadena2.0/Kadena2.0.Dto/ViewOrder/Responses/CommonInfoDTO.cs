namespace Kadena.Dto.ViewOrder.Responses
{
    public class CommonInfoDTO
    {
        public TitleValuePairDto Status { get; set; }
        public TitleValuePairDto OrderDate { get; set; }
        public TitleValuePairDto ShippingDate { get; set; }
        public TitleValuePairDto TotalCost { get; set; }
    }
}