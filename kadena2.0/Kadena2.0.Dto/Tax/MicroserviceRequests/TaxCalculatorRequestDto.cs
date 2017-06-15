namespace Kadena2.MicroserviceClients.MicroserviceRequests
{
    public class TaxCalculatorRequestDto
    {
        public string ShipFromState { get; set; }
        public string ShipFromCity { get; set; }
        public string ShipFromZip { get; set; }
        public string ShipToState { get; set; }
        public string ShipToCity { get; set; }
        public string ShipToZip { get; set; }
        public double TotalBasePrice { get; set; }
        public double ShipCost { get; set; }
    }
}
