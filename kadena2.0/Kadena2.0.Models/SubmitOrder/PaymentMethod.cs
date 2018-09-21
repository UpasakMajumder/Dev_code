namespace Kadena.Models.SubmitOrder
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        public string Invoice { get; set; }
        public string Card { get; set; }
        public bool ApprovalRequired { get; set; }
    }
}