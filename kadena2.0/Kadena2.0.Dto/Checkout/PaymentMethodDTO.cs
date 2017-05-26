namespace Kadena.Dto.Checkout
{
    public class PaymentMethodDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public bool Disabled { get; set; }
        public bool Checked { get; set; }
    }
}
