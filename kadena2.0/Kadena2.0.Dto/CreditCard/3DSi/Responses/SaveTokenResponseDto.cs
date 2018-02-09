using System.Runtime.Serialization;

namespace Kadena.Dto.CreditCard._3DSi.Responses
{
    /// <summary>
    /// Response from Kadena BE to 3DSi
    /// </summary>
    [DataContract(Name = "ResultResponse", Namespace = "")]
    public class SaveTokenResponseDto
    {
        public static readonly string ResultApprovedMessage = "Result approved.";
        public static readonly string ResultFailedMessage = "Result failed.";

        [DataMember(Name = "Succeeded")]
        public int Succeeded { get; set; }
        [DataMember(Name = "ResponseStatus")]
        public string ResponseStatus { get; set; }
        [DataMember(Name = "ResponseMessage")]
        public string ResponseMessage { get; set; }


        public static SaveTokenResponseDto ResultApproved => new SaveTokenResponseDto
        {
            Succeeded = 1,
            ResponseStatus = "001",
            ResponseMessage = ResultApprovedMessage
        };


        public static SaveTokenResponseDto ResultFailed => new SaveTokenResponseDto
        {
            Succeeded = 0,
            ResponseStatus = "000",
            ResponseMessage = ResultFailedMessage
        };
    }

}
