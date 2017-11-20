namespace Kadena.Models.CreditCard
{
    public class SaveTokenData
    {
        public string SubmissionID { get; set; }
        public bool Approved { get; set; }
        public string ApprovalResponseStatus { get; set; }
        public string ApprovalResponseMessage { get; set; }
        public bool CardStored { get; set; }
        public string SubmissionStatusMessage { get; set; }
        public string Token { get; set; }
    }
}
