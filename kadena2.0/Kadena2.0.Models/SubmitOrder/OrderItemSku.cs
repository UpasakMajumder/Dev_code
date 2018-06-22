namespace Kadena.Models.SubmitOrder
{
    public class OrderItemSku
    {
        public int KenticoSKUID { get; set; }
        public string SKUNumber { get; set; }
        public string Name { get; set; }
        public bool HiResPdfAllowed { get; set; }
        public Weight Weight { get; set; }
    }
}
