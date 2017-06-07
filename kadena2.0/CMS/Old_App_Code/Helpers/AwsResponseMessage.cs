using Newtonsoft.Json;
using System.Net.Http;
using System.Runtime.Serialization;

namespace Kadena.Old_App_Code.Helpers
{
    [DataContract]
    class AwsResponseMessage<TResponse>
    {
        [DataMember(Name = "success")]
        public bool Success { get; set; }

        [DataMember(Name = "payload")]
        public TResponse Response { get; set; }

        [DataMember(Name = "errorMessages")]
        public string ErrorMessages { get; set; }

        [DataMember(Name = "error")]
        public ErrorMessage Error { get; set; }

        public static explicit operator AwsResponseMessage<TResponse>(HttpResponseMessage message)
        {
            return JsonConvert.DeserializeObject<AwsResponseMessage<TResponse>>(message.Content.ReadAsStringAsync().Result);
        }
    }
}