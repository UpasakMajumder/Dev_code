using System.Net;
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

        [DataMember]
        public HttpStatusCode AWSStatusCode { get; set; }

        [DataMember]
        public string AWSResponse { get; set; }
    }
}
