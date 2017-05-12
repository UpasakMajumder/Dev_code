using System.Runtime.Serialization;

namespace Kadena.Old_App_Code.Helpers
{
    [DataContract]
    class AwsResponseMessage
    {
        [DataMember(Name = "success")]
        public bool Success { get; set; }

        [DataMember(Name = "payload")]
        public object Response { get; set; }

        [DataMember(Name = "errorMessages")]
        public string ErrorMessages { get; set; }
    }
}