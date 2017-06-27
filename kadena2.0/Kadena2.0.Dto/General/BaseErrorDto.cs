using System.Runtime.Serialization;

namespace Kadena.Dto.General
{
    [DataContract]
    public class BaseErrorDto
    {
        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "innerError")]
        public BaseErrorDto InnerError { get; set; }
    }
}