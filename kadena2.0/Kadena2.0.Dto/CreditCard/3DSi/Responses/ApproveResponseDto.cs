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

        public static readonly string ResultApprovedMessage = "Result approved.";
        public static readonly string ResultDeniedMessage = "Result denied.";

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
            

        public static ApproveResponseDto SubmissionDenied => new ApproveResponseDto
        {
            Succeeded = 0,
            ResponseStatus = "000",
            ResponseMessage = SubmissionDeniedMessage
        };

        public static ApproveResponseDto ResultApproved => new ApproveResponseDto
        {
            Succeeded = 1,
            ResponseStatus = "001",
            ResponseMessage = ResultApprovedMessage
        };


        public static ApproveResponseDto ResultDenied => new ApproveResponseDto
        {
            Succeeded = 0,
            ResponseStatus = "000",
            ResponseMessage = ResultDeniedMessage
        };

    }
}
