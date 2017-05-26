namespace Kadena.Dto.Checkout
{
    public class DeliveryMethodTypeDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Checked { get; set; }
        public string PricePrefix { get; set; }
        public string Price { get; set; }
        public string DatePrefix { get; set; }
        public string Date { get; set; }
        public bool Disabled { get; set; }
    }
}
