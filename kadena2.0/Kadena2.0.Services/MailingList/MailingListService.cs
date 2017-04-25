using Newtonsoft.Json;
using System.Net.Http;
using CMS.DataEngine;
using CMS.SiteProvider;
using CMS.Helpers;
using CMS.IO;
using System;

namespace Kadena.Services.MailingList
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MailingListService" in both code and config file together.
    public class MailingListService : IMailingListService
    {
        private readonly string _bucketType = "original-mailing";
        private readonly string _customerNameSettingKey = "KDA_CustomerName";
        private readonly string _mailingServiceSettingKey = "KDA_MailingServiceUrl";
        private readonly string _fileServiceSettingKey = "KDA_FileServiceUrl";

        public ResponseMessage UploadFile(UploadFileData data)
        {
            if (string.IsNullOrWhiteSpace(data.FileName))
            {
                return new ResponseMessage { IsSuccess = false, Message = "File name can not be empty." };
            }
            if (string.IsNullOrWhiteSpace(data.MailType))
            {
                return new ResponseMessage { IsSuccess = false, Message = "Mail type can not be empty." };
            }
            if (string.IsNullOrWhiteSpace(data.Product))
            {
                return new ResponseMessage { IsSuccess = false, Message = "Product can not be empty." };
            }
            if (string.IsNullOrWhiteSpace(data.Validity))
            {
                return new ResponseMessage { IsSuccess = false, Message = "Validity can not be empty." };
            }
            if (string.IsNullOrWhiteSpace(data.FileUrl))
            {
                return new ResponseMessage { IsSuccess = false, Message = "File url can not be empty." };
            }

            string customerName = SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.{_customerNameSettingKey}");
            string mailingServiceAddress = SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.{_mailingServiceSettingKey}");

            if (string.IsNullOrWhiteSpace(customerName))
            {
                return new ResponseMessage { IsSuccess = false, Message = "CustomerName not specified." };
            }
            if (string.IsNullOrWhiteSpace(mailingServiceAddress))
            {
                return new ResponseMessage { IsSuccess = false, Message = "Mailing service address not specified." };
            }

            var containerId = Guid.Empty;
            // Create container
            using (var client = new HttpClient())
            {
                using (var content = new StringContent(JsonConvert.SerializeObject(new
                {
                    name = $"Container for {data.FileName} file",
                    customerName = customerName,
                    Validity = data.Validity,
                    mailType = data.MailType,
                    productType = data.Product
                }), System.Text.Encoding.UTF8, "application/json"))
                {
                    using (var message = client.PostAsync(mailingServiceAddress, content))
                    {
                        var awsResponse = message.Result;
                        if (awsResponse.IsSuccessStatusCode)
                        {
                            var response = JsonConvert.DeserializeObject<AWSResponseMessage>(awsResponse.Content.ReadAsStringAsync().Result);
                            containerId = new Guid(response.Response);
                            return new ResponseMessage
                            {
                                IsSuccess = true,
                                Message = $"Created container id is {containerId}",
                                AWSStatusCode = awsResponse.StatusCode,
                                AWSResponse = awsResponse.ReasonPhrase
                            };
                        }
                        else
                        {
                            return new ResponseMessage
                            {
                                IsSuccess = false,
                                Message = "Failed to create mailing container.",
                                AWSStatusCode = awsResponse.StatusCode,
                                AWSResponse = awsResponse.ReasonPhrase
                            };
                        }
                    }
                }
            }
            // Upload file
            // Return headers
            // ef0c7c36-4934-4118-b317-2604ac69138c


            return new ResponseMessage { IsSuccess = true, Message = "Method called" };
        }

        private Guid SendToService(System.IO.Stream fileStream, string fileName)
        {
            string fileServiceAddress = SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.{_fileServiceSettingKey}");
            if (string.IsNullOrWhiteSpace(fileServiceAddress))
            {
                return Guid.Empty;
            }

            string customerName = SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.{_customerNameSettingKey}");
            if (string.IsNullOrWhiteSpace(customerName))
            {
                return Guid.Empty;
            }

            var fileId = Guid.Empty;
            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    content.Add(new StreamContent(fileStream), "file", fileName);
                    content.Add(new StringContent(_bucketType), "bucketType");
                    content.Add(new StringContent(customerName), "customerName");
                    using (var message = client.PostAsync(fileServiceAddress, content))
                    {
                        var awsResponse = message.Result;
                        if (awsResponse.IsSuccessStatusCode)
                        {
                            var response = JsonConvert.DeserializeObject<AWSResponseMessage>(awsResponse.Content.ReadAsStringAsync().Result);
                            fileId = new Guid(response.Response);
                        }
                        return fileId;
                    }
                }
            }
        }

        private void GetHeaders()
        {
            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    using (var message = client.GetAsync(""))
                    {
                        var awsResponse = message.Result;
                        if (awsResponse.IsSuccessStatusCode)
                        {
                            var response = JsonConvert.DeserializeObject<AWSResponseMessage>(awsResponse.Content.ReadAsStringAsync().Result);
                        }
                    }
                }
            }
        }

        public ResponseMessage UploadFilePath()
        {
            var path = @"C:\Projects\MailingListTest.csv";
            using (var fs = FileStream.New(path, FileMode.Open))
            {
                var fileId = SendToService(fs, System.IO.Path.GetFileName(path));
                if (fileId == Guid.Empty)
                    return new ResponseMessage { IsSuccess = false, Message = "Failed to upload file." };
                else
                    return new ResponseMessage { IsSuccess = true, Message = $"File id is {fileId}" };
            }
        }
    }
}