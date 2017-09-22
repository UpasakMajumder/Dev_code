using System.Runtime.Serialization;

namespace Kadena.Dto.CreditCard._3DSi.Responses
{
    /// <summary>
    /// Response from Kadena to 3DSi
    /// </summary>
    [DataContract(Name = "ApprovalResponse", Namespace = "")]
    public class ApproveResponseDto
    {
        public static readonly string SubmissionApprovedMessage = "Submission approved.";
        public static readonly string SubmissionDeniedMessage = "Submission denied.";

        [DataMember(Name ="Succeeded")]
        public int Succeeded { get; set; }
        [DataMember(Name = "ResponseStatus")]
        public string ResponseStatus { get; set; }
        [DataMember(Name = "ResponseMessage")]
        public string ResponseMessage { get; set; }

    
        public static ApproveResponseDto SubmissionApproved => new ApproveResponseDto
        {
            Succeeded = 1,
            ResponseStatus = "001",
            ResponseMessage = SubmissionApprovedMessage
        };
            

        public static ApproveResponseDto SubmisstionDenied => new ApproveResponseDto
        {
            Succeeded = 0,
            ResponseStatus = "000",
            ResponseMessage = SubmissionDeniedMessage
        };
        
    }
}
