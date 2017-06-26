using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;

namespace Kadena.Dto.General
{
    [DataContract]
    public class BaseResponse<TResponse>
    {
        [DataMember(Name = "success")]
        public bool Success { get; set; }

        [DataMember(Name = "payload")]
        public TResponse Payload { get; set; }

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
                Error = new BaseError { Message = value };
            }
        }

        [DataMember(Name = "error")]
        public BaseError Error { get; set; }

        public static explicit operator BaseResponse<TResponse>(HttpResponseMessage message)
        {
            return JsonConvert.DeserializeObject<BaseResponse<TResponse>>(message.Content.ReadAsStringAsync().Result);
        }

        public static explicit operator BaseResponse<TResponse>(HttpWebResponse message)
        {
            string resultString = string.Empty;

            using (var streamReader = new StreamReader(message.GetResponseStream()))
            {
                resultString = streamReader.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<BaseResponse<TResponse>>(resultString);
        }
    }
}