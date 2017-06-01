﻿using CMS.DataEngine;
using CMS.Helpers;
using System.IO;
using CMS.SiteProvider;
using Kadena.Old_App_Code.Kadena.MailingList;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Kadena.Old_App_Code.Helpers
{
    public static class ServiceHelper
    {
        private const string _bucketType = "original-mailing";
        private const string _moduleName = "Klist";
        private const string _loadFileSettingKey = "KDA_LoadFileUrl";
        private const string _getHeaderSettingKey = "KDA_GetHeadersUrl";
        private const string _customerNameSettingKey = "KDA_CustomerName";
        private const string _createContainerSettingKey = "KDA_CreateContainerUrl";
        private const string _uploadMappingSettingKey = "KDA_UploadMappingUrl";
        private const string _validateAddressSettingKey = "KDA_ValidateAddressUrl";
        private const string _getMailingListsSettingKey = "KDA_GetMailingListsUrl";
        private const string _getMailingListByIdSettingKey = "KDA_GetMailingListByIdUrl";
        private const string _deleteAddressesSettingKey = "KDA_DeleteAddressesUrl";

        private const string _customerNotSpecifiedMessage = "CustomerName not specified. Check settings for your site.";
        private const string _valueEmptyMessage = "Value can not be empty.";
        private const string _responseIncorrectMessage = "Response from microservice is not in correct format.";
        private const string _loadFileIncorrectMessage = "Url for file uploading is not in correct format. Check settings for your site.";
        private const string _createContainerIncorrectMessage = "Url for creating container is not in correct format. Check settings for your site.";
        private const string _getHeadersIncorrectMessage = "Url for getting headers is not in correct format. Check settings for your site.";
        private const string _uploadMappingIncorrectMessage = "Url for uploading mapping is not in correct format. Check settings for your site.";
        private const string _validateAddressIncorrectMessage = "Url for validating addresses is not in correct format. Check settings for your site.";
        private const string _getMailingListByIdIncorrectMessage = "Url for getting mailing container by id is not in correct format. Check settings for your site.";
        private const string _deleteAddressesIncorrectMessage = "Url for deleting address from container is not in correct format. Check settings for your site.";

        /// <summary>
        /// Sends request to microservice to create mailing container.
        /// </summary>
        /// <param name="name">Name for mailing container.</param>
        /// <param name="mailType">Mail type option for mailing container.</param>
        /// <param name="product">Product type option for mailing container.</param>
        /// <param name="validityDays">Validity option for mailing container.</param>
        /// <returns>Id of mailing container.</returns>
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
        public static string UploadFile(Stream fileStream, string fileName)
        {
            if (fileStream == null || fileStream.Length == 0)
            {
                throw new ArgumentException(_valueEmptyMessage, nameof(fileStream));
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException(_valueEmptyMessage, nameof(fileName));
            }

            string customerName = GetCustomerName();

            Uri postFileUrl;
            if (!Uri.TryCreate(SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.{_loadFileSettingKey}")
                , UriKind.Absolute
                , out postFileUrl))
            {
                throw new InvalidOperationException(_loadFileIncorrectMessage);
            }

            var fileId = string.Empty;
            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    fileStream.Seek(0, SeekOrigin.Begin);
                    content.Add(new StreamContent(fileStream), "file", fileName);
                    content.Add(new StringContent(_bucketType), "ConsumerDetails.BucketType");
                    content.Add(new StringContent(customerName), "ConsumerDetails.CustomerName");
                    content.Add(new StringContent(_moduleName), "ConsumerDetails.Module");
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
                            fileId = response?.Response?.ToString();
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

        /// <summary>
        /// Requests for headers of specified file.
        /// </summary>
        /// <param name="fileId">Id for file to get headers for.</param>
        /// <returns>List of header names.</returns>
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
                        result = (response.Response as JArray).ToObject<IEnumerable<string>>();
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
                        if (!(response?.Success ?? false))
                        {
                            throw new HttpRequestException(response?.ErrorMessages ?? message.Result.ReasonPhrase);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Forces microservices to start addresses validation for specified container.
        /// </summary>
        /// <param name="containerId">Id of container.</param>
        /// <returns>If of file with valid addresses.</returns>
        public static string ValidateAddresses(Guid containerId)
        {
            if (containerId == Guid.Empty)
            {
                throw new ArgumentException(_valueEmptyMessage, nameof(containerId));
            }

            Uri validateAddressUrl;
            if (!Uri.TryCreate(SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.{_validateAddressSettingKey}")
                , UriKind.Absolute
                , out validateAddressUrl))
            {
                throw new InvalidOperationException(_validateAddressIncorrectMessage);
            }

            using (var client = new HttpClient())
            {
                using (var content = new StringContent(JsonConvert.SerializeObject(new
                {
                    ContainerId = containerId
                }), System.Text.Encoding.UTF8, "application/json"))
                {
                    using (var message = client.PostAsync(validateAddressUrl, content))
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
                            return response?.Response?.ToString();
                        }
                        else
                        {
                            throw new HttpRequestException(response?.ErrorMessages ?? message.Result.ReasonPhrase);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets name of customer from settings for current site.
        /// </summary>
        /// <returns>Customer's name</returns>
        private static string GetCustomerName()
        {
            string customerName = SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.{_customerNameSettingKey}");
            if (string.IsNullOrWhiteSpace(customerName))
            {
                throw new InvalidOperationException(_customerNotSpecifiedMessage);
            }

            return customerName;
        }

        /// <summary>
        /// Get all mailing lists for particular customer (whole site)
        /// </summary>
        public static IEnumerable<MailingListData> GetMailingLists()
        {
            var customerName = SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.{_customerNameSettingKey}");

            using (var client = new HttpClient())
            {
                using (var message = client.GetAsync(SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.{_getMailingListsSettingKey}") + "/" + customerName))
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
                        return (response.Response as JArray).ToObject<IEnumerable<MailingListData>>();
                    }
                    else
                    {
                        throw new HttpRequestException(response?.ErrorMessages ?? message.Result.ReasonPhrase);
                    }
                }
            }
        }

        /// <summary>
        /// Get all mailing list for particular customer (whole site) by specified Id.
        /// </summary>
        /// <param name="containerId">Id of container to get.</param>
        public static MailingListData GetMailingList(Guid containerId)
        {
            var customerName = GetCustomerName();

            Uri getMailingListUrl;
            if (!Uri.TryCreate(
                    string.Format("{0}/{1}/{2}",
                    SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.{_getMailingListByIdSettingKey}"),
                    customerName,
                    containerId)
                , UriKind.Absolute
                , out getMailingListUrl))
            {
                throw new InvalidOperationException(_getMailingListByIdIncorrectMessage);
            }

            using (var client = new HttpClient())
            {
                using (var message = client.GetAsync(getMailingListUrl))
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
                        return JsonConvert.DeserializeObject<MailingListData>(response.Response.ToString());
                    }
                    else
                    {
                        throw new HttpRequestException(response?.ErrorMessages ?? message.Result.ReasonPhrase);
                    }
                }
            }
        }

        /// <summary>
        /// Removes all address from specified container.
        /// </summary>
        /// <param name="containerId">Id of container to be cleared.</param>
        public static void RemoveAddresses(Guid containerId)
        {
            if (containerId == Guid.Empty)
            {
                throw new ArgumentException(_valueEmptyMessage, nameof(containerId));
            }

            Uri deleteAddressesUrl;
            if (!Uri.TryCreate(SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.{_deleteAddressesSettingKey}")
                , UriKind.Absolute
                , out deleteAddressesUrl))
            {
                throw new InvalidOperationException(_deleteAddressesIncorrectMessage);
            }

            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(new
                    {
                        ContainerId = containerId
                    }), System.Text.Encoding.UTF8, "application/json"),
                    RequestUri = deleteAddressesUrl,
                    Method = HttpMethod.Delete
                })
                {
                    client.SendAsync(request).Wait();
                }
            }
        }
    }
}