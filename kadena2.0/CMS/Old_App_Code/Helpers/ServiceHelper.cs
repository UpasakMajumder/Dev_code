using CMS.DataEngine;
using CMS.Helpers;
using System.IO;
using CMS.SiteProvider;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using CMS.Ecommerce;
using Kadena.Dto.General;
using Kadena.Dto.MailingList.MicroserviceResponses;

namespace Kadena.Old_App_Code.Helpers
{
    public static class ServiceHelper
    {
        private const string _moduleName = "Klist";
        private const string _getHeaderSettingKey = "KDA_GetHeadersUrl";
        private const string _createContainerSettingKey = "KDA_CreateContainerUrl";
        private const string _uploadMappingSettingKey = "KDA_UploadMappingUrl";
        private const string _validateAddressSettingKey = "KDA_ValidateAddressUrl";
        private const string _getMailingListsSettingKey = "KDA_GetMailingListsUrl";
        private const string _getMailingListByIdSettingKey = "KDA_GetMailingListByIdUrl";
        private const string _deleteAddressesSettingKey = "KDA_DeleteAddressesUrl";
        private const string _getAddressesSettingKey = "KDA_GetMailingAddressesUrl";

        private const string _customerNotSpecifiedMessage = "CustomerName not specified. Check settings for your site.";
        private const string _valueEmptyMessage = "Value can not be empty.";
        private const string _responseIncorrectMessage = "Response from microservice is not in correct format.";
        private const string _createContainerIncorrectMessage = "Url for creating container is not in correct format. Check settings for your site.";
        private const string _getHeadersIncorrectMessage = "Url for getting headers is not in correct format. Check settings for your site.";
        private const string _uploadMappingIncorrectMessage = "Url for uploading mapping is not in correct format. Check settings for your site.";
        private const string _validateAddressIncorrectMessage = "Url for validating addresses is not in correct format. Check settings for your site.";
        private const string _getMailingListByIdIncorrectMessage = "Url for getting mailing container by id is not in correct format. Check settings for your site.";
        private const string _deleteAddressesIncorrectMessage = "Url for deleting address from container is not in correct format. Check settings for your site.";
        private const string _getAddressesIncorrectMessage = "Url for getting addresses is not in correct format. Check settings for your site.";
        private const string _getOrderStatisticsIncorrectMessage = "Url of order statistics is not in correct format. Check settings for your site.";

        /// <summary>
        /// Sends request to microservice to create mailing container.
        /// </summary>
        /// <param name="name">Name for mailing container.</param>
        /// <param name="mailType">Mail type option for mailing container.</param>
        /// <param name="product">Product type option for mailing container.</param>
        /// <param name="validityDays">Validity option for mailing container.</param>
        /// <returns>Id of mailing container.</returns>
        // Mailing Service
        public static Guid CreateMailingContainer(string name, string mailType, string product, int validityDays)
        {
            if (string.IsNullOrWhiteSpace(mailType))
            {
                throw new ArgumentException(_valueEmptyMessage, nameof(mailType));
            }
            if (string.IsNullOrWhiteSpace(product))
            {
                throw new ArgumentException(_valueEmptyMessage, nameof(product));
            }

            string customerName = GetCustomerName();

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
                    name = name,
                    customerName = customerName,
                    Validity = validityDays,
                    mailType = mailType,
                    productType = product,
                    customerId = ECommerceContext.CurrentCustomer?.CustomerID.ToString()
                }), System.Text.Encoding.UTF8, "application/json"))
                {
                    using (var message = client.PostAsync(createContainerUrl, content))
                    {
                        BaseResponseDto<Guid> response;
                        try
                        {
                            response = (BaseResponseDto<Guid>)message.Result;
                        }
                        catch (JsonReaderException e)
                        {
                            throw new InvalidOperationException(_responseIncorrectMessage, e);
                        }
                        if (response?.Success ?? false)
                        {
                            containerId = response.Payload;
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
        /// Requests for headers of specified file.
        /// </summary>
        /// <param name="fileId">Id for file to get headers for.</param>
        /// <returns>List of header names.</returns>
        // Parsing Service
        public static IEnumerable<string> GetHeaders(string fileId)
        {
            if (string.IsNullOrWhiteSpace(fileId))
            {
                throw new ArgumentException(_valueEmptyMessage, nameof(fileId));
            }

            string customerName = GetCustomerName();

            Uri getHeaderUrl;
            if (!Uri.TryCreate(SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.{_getHeaderSettingKey}")
                , UriKind.Absolute
                , out getHeaderUrl))
            {
                throw new InvalidOperationException(_getHeadersIncorrectMessage);
            }

            string parametrizeUrl = URLHelper.AddParameterToUrl(getHeaderUrl.AbsoluteUri, "fileid", fileId.ToString());
            parametrizeUrl = URLHelper.AddParameterToUrl(parametrizeUrl, "module", _moduleName);

            IEnumerable<string> result;
            using (var client = new HttpClient())
            {
                using (var message = client.GetAsync(parametrizeUrl))
                {
                    BaseResponseDto<IEnumerable<string>> response;
                    try
                    {
                        response = (BaseResponseDto<IEnumerable<string>>)message.Result;
                    }
                    catch (JsonReaderException e)
                    {
                        throw new InvalidOperationException(_responseIncorrectMessage, e);
                    }
                    if (response?.Success ?? false)
                    {
                        result = response.Payload;
                    }
                    else
                    {
                        throw new HttpRequestException(response?.ErrorMessages ?? message.Result.ReasonPhrase);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Uploads specified mapping to microservice with binding to specified file and container.
        /// </summary>
        /// <param name="fileId">Id of file.</param>
        /// <param name="containerId">Id of mailing container.</param>
        /// <param name="mapping">Dictionary with mapping field names to index of column.</param>
        // Mailing Service
        public static void UploadMapping(string fileId, Guid containerId, Dictionary<string, int> mapping)
        {
            if (string.IsNullOrWhiteSpace(fileId))
            {
                throw new ArgumentException(_valueEmptyMessage, nameof(fileId));
            }

            if (containerId == Guid.Empty)
            {
                throw new ArgumentException(_valueEmptyMessage, nameof(containerId));
            }

            if ((mapping?.Count ?? 0) == 0)
            {
                throw new ArgumentException(_valueEmptyMessage, nameof(mapping));
            }

            string customerName = GetCustomerName();

            Uri uploadMappingUrl;
            if (!Uri.TryCreate(SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.{_uploadMappingSettingKey}")
                , UriKind.Absolute
                , out uploadMappingUrl))
            {
                throw new InvalidOperationException(_uploadMappingIncorrectMessage);
            }

            string jsonMapping = string.Empty;
            using (var sw = new StringWriter())
            {
                using (var writer = new JsonTextWriter(sw))
                {
                    writer.WriteStartObject();
                    foreach (var map in mapping)
                    {
                        writer.WritePropertyName(map.Key);
                        writer.WriteValue(map.Value);
                    }
                    writer.WriteEndObject();
                    jsonMapping = sw.ToString();
                }
            }
            using (var client = new HttpClient())
            {
                using (var content = new StringContent(JsonConvert.SerializeObject(new
                {
                    mapping = jsonMapping,
                    fileId = fileId,
                    customerName = customerName,
                    containerId = containerId
                }), System.Text.Encoding.UTF8, "application/json"))
                {
                    using (var message = client.PostAsync(uploadMappingUrl, content))
                    {
                        BaseResponseDto<object> response;
                        try
                        {
                            response = (BaseResponseDto<object>)message.Result;
                        }
                        catch (JsonReaderException e)
                        {
                            throw new InvalidOperationException(_responseIncorrectMessage, e);
                        }
                        if (!(response?.Success ?? false))
                        {
                            throw new HttpRequestException(response?.ErrorMessages ?? message.Result.ReasonPhrase);
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// Gets name of customer from site settings (Site application).
        /// </summary>
        /// <returns>Customer's name</returns>
        private static string GetCustomerName()
        {
            return SiteContext.CurrentSiteName;
        }
    }
}