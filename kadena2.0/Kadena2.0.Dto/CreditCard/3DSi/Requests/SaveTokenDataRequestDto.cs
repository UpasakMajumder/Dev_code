namespace Kadena.Dto.CreditCard._3DSi.Requests
{
    public class SaveTokenDataRequestDto
    {
        public string SilentPostType { get; set; }
        public string SubmissionID { get; set; }
        public int Approved { get; set; }
        public int CardStored { get; set; }
        public string Token { get; set; }
        public string ApprovalResponseStatus { get; set; }
        public string ApprovalResponseMessage { get; set; }
        public string SubmissionStatusMessage { get; set; }
        public int CardVerified { get; set; }
        public string AddressAvsResponse { get; set; }
        public string PostalCodeAvsResponse { get; set; }
        public string CardSecurityCodeResponse { get; set; }
        public string ProcessorAvsResponse { get; set; }
        public string ProcessorCardSecurityCodeResponse { get; set; }
        public string TransactionKey { get; set; }
        public string ProcessorResponse { get; set; }
        public string CreditCardTransactionFailedSubStatus { get; set; }
    }
}
