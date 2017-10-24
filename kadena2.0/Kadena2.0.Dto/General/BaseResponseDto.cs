using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;

namespace Kadena.Dto.General
{
    [DataContract]
    public class BaseResponseDto<TResponse>
    {
        private string _errorMessage;
        private BaseErrorDto _error;

        [DataMember(Name = "success")]
        public bool Success { get; set; }

        [DataMember(Name = "payload")]
        public TResponse Payload { get; set; }

        [DataMember(Name = "errorMessages")]
        public string ErrorMessages
        {
            get
            {
                if (_errorMessage == null)
                {
                    var messages = new List<string>(4);
                    var error = Error;
                    while (error != null)
                    {
                        messages.Add($"ErrorMessage: {error.Message}");
                        error = error.InnerError;
                    }
                    _errorMessage = string.Join(Environment.NewLine, messages);
                }
                return _errorMessage;
            }
            set
            {
                Error = new BaseErrorDto { Message = value };
            }
        }

        [DataMember(Name = "error")]
        public BaseErrorDto Error
        {
            get
            {
                return _error;
            }
            set
            {
                _error = value;
                _errorMessage = null;
            }
        }

        public static explicit operator BaseResponseDto<TResponse>(HttpResponseMessage message)
        {
            return JsonConvert.DeserializeObject<BaseResponseDto<TResponse>>(message.Content.ReadAsStringAsync().Result);
        }

        public static explicit operator BaseResponseDto<TResponse>(HttpWebResponse message)
        {
            string resultString = string.Empty;

            using (var streamReader = new StreamReader(message.GetResponseStream()))
            {
                resultString = streamReader.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<BaseResponseDto<TResponse>>(resultString);
        }
    }
}