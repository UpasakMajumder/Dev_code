using System.Runtime.Serialization;

namespace Kadena.Old_App_Code.Helpers
{
    [DataContract]
    public class ErrorMessage
    {
        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "innerError")]
        public ErrorMessage InnerError { get; set; }
    }
}