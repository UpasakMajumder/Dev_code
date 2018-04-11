namespace Kadena.Dto.SubmitOrder.MicroserviceRequests
{
    public class TotalsDto
    {
        public decimal? Shipping { get; set; }
        public decimal Price { get; set; }
        public decimal Tax { get; set; }
        public decimal PricedItemsTax { get; set; }
    }
}
