using CMS.DataEngine;
using CMS.SiteProvider;
using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace Kadena.Old_App_Code.Helpers
{
    public static class ServiceHelper
    {
        private const string _customerNameSettingKey = "KDA_CustomerName";
        private const string _createContainerSettingKey = "KDA_CreateContainerUrl";

        private const string _customerNotSpecifiedMessage = "CustomerName not specified. Check settings for your site.";
        private const string _valueEmptyMessage = "Value can not be empty.";
        private const string _responseIncorrectMessage = "Response from microservice is not in correct format.";
        private const string _loadFileIncorrectMessage = "Url for file uploading is not in correct format. Check settings for your site.";
        private const string _createContainerIncorrectMessage = "Url for creating container is not in correct format. Check settings for your site.";
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
                        if (response.Success)
                        {
                            containerId = new Guid(response?.Response?.ToString());
                        }
                        else
                        {
                            throw new HttpRequestException(response.ErrorMessages);
                        }
                    }
                }
            }
            return containerId;
        }
    }
}