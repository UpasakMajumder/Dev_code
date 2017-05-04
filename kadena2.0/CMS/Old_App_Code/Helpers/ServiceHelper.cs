using CMS.DataEngine;
using CMS.SiteProvider;
using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace Kadena.Old_App_Code.Helpers
{
    public static class ServiceHelper
    {
        private const string _bucketType = "original-mailing";
        private const string _loadFileSettingKey = "KDA_LoadFileUrl";
        private const string _getHeadersSettingKey = "KDA_GetHeadersUrl";
        private const string _customerNameSettingKey = "KDA_CustomerName";
        private const string _createContainerSettingKey = "KDA_CreateContainerUrl";

        private const string _customerNotSpecifiedMessage = "CustomerName not specified. Check settings for your site.";
        private const string _valueEmptyMessage = "Value can not be empty.";
        private const string _responseIncorrectMessage = "Response from microservice is not in correct format.";
        private const string _loadFileIncorrectMessage = "Url for file uploading is not in correct format. Check settings for your site.";
        private const string _createContainerIncorrectMessage = "Url for creating container is not in correct format. Check settings for your site.";

        /// <summary>
        /// Sends request to microservice to create mailing container.
        /// </summary>
        /// <param name="mailType">Mail type option for mailing container.</param>
        /// <param name="product">Product type option for mailing container.</param>
        /// <param name="validityDays">Validity option for mailing container.</param>
        /// <returns>Id of mailing container.</returns>
        public static Guid CreateMailingContainer(string mailType, string product, int validityDays)
        {
            if (string.IsNullOrWhiteSpace(mailType))
            {
                throw new ArgumentException(_valueEmptyMessage, "mailType");
            }
            if (string.IsNullOrWhiteSpace(product))
            {
                throw new ArgumentException(_valueEmptyMessage, "product");
            }

            string customerName = SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.{_customerNameSettingKey}");
            if (string.IsNullOrWhiteSpace(customerName))
            {
                throw new InvalidOperationException(_customerNotSpecifiedMessage);
            }

            Uri createContainerUrl;
            if (!Uri.TryCreate(SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.{_createContainerSettingKey}")
                , UriKind.Absolute
                , out createContainerUrl))
            {
                throw new InvalidOperationException(_createContainerIncorrectMessage);
            }

            var containerId = Guid.Empty;
            using (var client = new HttpClient())
            {
                using (var content = new StringContent(JsonConvert.SerializeObject(new
                {
                    name = $"Mailing container for {customerName}.",
                    customerName = customerName,
                    Validity = validityDays,
                    mailType = mailType,
                    productType = product
                }), System.Text.Encoding.UTF8, "application/json"))
                {
                    using (var message = client.PostAsync(createContainerUrl, content))
                    {
                        AwsResponseMessage response;
                        try
                        {
                            response = JsonConvert.DeserializeObject<AwsResponseMessage>(message.Result
                                .Content.ReadAsStringAsync()
                                .Result);
                        }
                        catch (JsonReaderException e)
                        {
                            throw new InvalidOperationException(_responseIncorrectMessage, e);
                        }
                        if (response?.Success ?? false)
                        {
                            containerId = new Guid(response?.Response?.ToString());
                        }
                        else
                        {
                            throw new HttpRequestException(response?.ErrorMessages ?? message.Result.ReasonPhrase);
                        }
                    }
                }
            }
            return containerId;
        }

        /// <summary>
        /// Uploads file with request to microservice.
        /// </summary>
        /// <param name="fileStream">Stream to upload.</param>
        /// <param name="fileName">Name of file to pass to microservice.</param>
        /// <returns>Id of uploaded file.</returns>
        public static Guid UploadFile(System.IO.Stream fileStream, string fileName)
        {
            if (fileStream == null || fileStream.Length == 0)
            {
                throw new ArgumentException(_valueEmptyMessage, "fileStream");
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException(_valueEmptyMessage, "fileName");
            }

            string customerName = SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.{_customerNameSettingKey}");
            if (string.IsNullOrWhiteSpace(customerName))
            {
                throw new InvalidOperationException(_customerNotSpecifiedMessage);
            }

            Uri postFileUrl;
            if (!Uri.TryCreate(SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.{_loadFileSettingKey}")
                , UriKind.Absolute
                , out postFileUrl))
            {
                throw new InvalidOperationException(_loadFileIncorrectMessage);
            }

            var fileId = Guid.Empty;
            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    fileStream.Seek(0, System.IO.SeekOrigin.Begin);
                    content.Add(new StreamContent(fileStream), "file", fileName);
                    content.Add(new StringContent(_bucketType), "bucketType");
                    content.Add(new StringContent(customerName), "customerName");
                    using (var message = client.PostAsync(postFileUrl, content))
                    {
                        AwsResponseMessage response;
                        try
                        {
                            response = JsonConvert.DeserializeObject<AwsResponseMessage>(message.Result
                                .Content.ReadAsStringAsync()
                                .Result);
                        }
                        catch (JsonReaderException e)
                        {
                            throw new InvalidOperationException(_responseIncorrectMessage, e);
                        }
                        if (response?.Success ?? false)
                        {
                            fileId = new Guid(response?.Response?.ToString());
                        }
                        else
                        {
                            throw new HttpRequestException(response?.ErrorMessages ?? message.Result.ReasonPhrase);
                        }
                    }
                }
            }
            return fileId;
        }
    }
}