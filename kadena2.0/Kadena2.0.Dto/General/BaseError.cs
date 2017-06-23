using System.Runtime.Serialization;

namespace Kadena.Dto.General
{
    [DataContract]
    public class BaseError
    {
        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "innerError")]
        public BaseError InnerError { get; set; }
    }
}