using Newtonsoft.Json;
using System.Net.Http;
using CMS.DataEngine;
using CMS.SiteProvider;

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
            string fileServiceAddress = SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.{_fileServiceSettingKey}");

            if (string.IsNullOrWhiteSpace(customerName))
            {
                return new ResponseMessage { IsSuccess = false, Message = "CustomerName not specified." };
            }
            if (string.IsNullOrWhiteSpace(mailingServiceAddress))
            {
                return new ResponseMessage { IsSuccess = false, Message = "Mailing service address not specified." };
            }
            if (string.IsNullOrWhiteSpace(fileServiceAddress))
            {
                return new ResponseMessage { IsSuccess = false, Message = "File service address not specified." };
            }


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
                            return new ResponseMessage
                            {
                                IsSuccess = true,
                                Message = $"Created container id is {response.Response}",
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
    }
}