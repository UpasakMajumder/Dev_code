using System.Runtime.Serialization;

namespace Kadena.Services
{
    [DataContract]
    public class AWSResponseMessage
    {
        [DataMember(Name = "success")]
        public bool Success { get; set; }

        [DataMember(Name = "response")]
        public string Response { get; set; }
    }
}
