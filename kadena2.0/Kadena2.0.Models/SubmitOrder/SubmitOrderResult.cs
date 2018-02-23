namespace Kadena.Models.SubmitOrder
{
    public class SubmitOrderResult 
    {
        public string OrderId { get; set; }
        public bool Success { get; set; }
        public string RedirectURL { get; set; }
        public string Error { get; set; }
    }
}