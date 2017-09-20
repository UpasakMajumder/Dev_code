namespace Kadena.Models.SubmitOrder
{
    public class SubmitOrderResult // TODO refactor use BaseResponse
    {
        public string Payload { get; set; }
        public bool Success { get; set; }
        public string RedirectURL { get; set; }
        public SubmitOrderError Error { get; set; }
    }
}