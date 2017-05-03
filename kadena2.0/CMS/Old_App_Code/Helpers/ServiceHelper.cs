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

        public static Guid CreateMailingContainer(string mailType, string product, string validity)
        {
            if (string.IsNullOrWhiteSpace(mailType))
            {
                throw new ArgumentException("Value can not be empty.", "mailType");
            }
            if (string.IsNullOrWhiteSpace(product))
            {
                throw new ArgumentException("Value can not be empty.", "product");
            }
            if (string.IsNullOrWhiteSpace(validity))
            {
                throw new ArgumentException("Value can not be empty.", "validity");
            }

            string customerName = SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.{_customerNameSettingKey}");
            Uri createContainerUrl;
            if (string.IsNullOrWhiteSpace(customerName))
            {
                throw new InvalidOperationException("CustomerName not specified. Check settings for your site.");
            }
            if (!Uri.TryCreate(SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.{_createContainerSettingKey}")
                , UriKind.Absolute
                , out createContainerUrl))
            {
                throw new InvalidOperationException("Mailing service is not in correct format. Check settings for your site.");
            }
            
            var containerId = Guid.Empty;
            using (var client = new HttpClient())
            {
                using (var content = new StringContent(JsonConvert.SerializeObject(new
                {
                    name = $"Mailing container for {customerName}.",
                    customerName = customerName,
                    Validity = validity,
                    mailType = mailType,
                    productType = product
                }), System.Text.Encoding.UTF8, "application/json"))
                {
                    using (var message = client.PostAsync(createContainerUrl, content))
                    {
                        var awsResponse = message.Result;
                        if (awsResponse.IsSuccessStatusCode)
                        {
                            // success
                            var response = JsonConvert.DeserializeObject<AwsResponseMessage>(awsResponse.Content.ReadAsStringAsync().Result);
                            containerId = new Guid(response.Response.ToString());
                        }
                        else
                        {
                            // fail
                        }
                    }
                }
            }
            return containerId;
        }
    }
}