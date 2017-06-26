using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
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
        [Obsolete("Will be removed after all microservices will use Error property.")]
        public string ErrorMessages
        {
            get
            {
                return Error?.Message ?? string.Empty;
            }
            set
            {
                Error = new ErrorMessage { Message = value };
            }
        }

        [DataMember(Name = "error")]
        public ErrorMessage Error { get; set; }

        public static explicit operator AwsResponseMessage<TResponse>(HttpResponseMessage message)
        {
            return JsonConvert.DeserializeObject<AwsResponseMessage<TResponse>>(message.Content.ReadAsStringAsync().Result);
        }

        public static explicit operator AwsResponseMessage<TResponse>(HttpWebResponse message)
        {
            string resultString = string.Empty;

            using (var streamReader = new StreamReader(message.GetResponseStream()))
            {
                resultString = streamReader.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<AwsResponseMessage<TResponse>>(resultString);
        }
    }
}