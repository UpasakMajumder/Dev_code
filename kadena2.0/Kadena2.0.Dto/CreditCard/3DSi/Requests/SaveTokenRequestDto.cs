namespace Kadena.Dto.CreditCard._3DSi.Requests
{
    /// <summary>
    /// Request from 3DSi to Kadena to save the token
    /// </summary>
    public class SaveTokenRequestDto
    {
        /// <summary>
        /// A constant value sent from 3DSI:
        /// 
        /// AddStoredCreditCard 
        /// VerifyAndAddStoredCreditCard
        /// </summary>
        public string SilentPostType { get; set; }

        /// <summary>
        /// A unique identifier associated with the current attempt to submit data to CardVault.
        /// </summary>
        public string SubmissionID { get; set; }

        /// <summary>
        /// A boolean value (0 for Denied or 1 for Approved) indicating whether approval of storage of the card succeeded.
        /// </summary>
        public bool Approved { get; set; }

        /// <summary>
        /// The ResponseStatus value returned from the client’s ApprovalURL.
        /// </summary>
        public string  ApprovalResponseStatus { get; set; }

        /// <summary>
        /// The ResponseMessage value returned from the client’s ApprovalURL.
        /// </summary>
        public string ApprovalResponseMessage { get; set; }

        /// <summary>
        /// A boolean value (0 for Failure or 1 for Success) indicating whether storage of the card succeeded.
        /// </summary>
        public bool CardStored { get; set; }

        /// <summary>
        /// A message describing the overall status of the submission and any card verification 
        /// transaction validation failures.
        /// </summary>
        public string SubmissionStatusMessage { get; set; }

        /// <summary>
        /// The value of the token representing the stored card.
        /// </summary>
        public string Token { get; set; }
    } 
}
