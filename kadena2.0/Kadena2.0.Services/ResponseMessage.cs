using System.Runtime.Serialization;

namespace Kadena.Services
{
    [DataContract]
    public class ResponseMessage
    {
        [DataMember]
        public bool IsSuccess { get; set; }

        [DataMember]
        public string Message { get; set; }
    }
}
