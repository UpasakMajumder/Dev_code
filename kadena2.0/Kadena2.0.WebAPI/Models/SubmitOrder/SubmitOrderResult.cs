namespace Kadena.WebAPI.Models.SubmitOrder
{
    public class SubmitOrderResult
    {
        public string Payload { get; set; }
        public bool Success { get; set; }
        public string RedirectURL { get; set; }
        public SubmitOrderError Error { get; set; }
    }
}