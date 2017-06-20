using CMS.DataEngine;
using CMS.Helpers;
using CMS.SiteProvider;
using Kadena.Old_App_Code.Kadena.KSource;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Kadena.Old_App_Code.Helpers
{
    public static class BidServiceHelper
    {
        private const string _workgroupNameSettingKey = "KDA_WorkgroupName";
        private const string _getProjectsSettingKey = "KDA_GetProjectsUrl";
        private const string _nooshApiSettingKey = "KDA_NooshApi";
        private const string _nooshTokenSettingKey = "KDA_NooshToken";

        private const string _customerNotSpecifiedMessage = "Workgroup's name not specified. Check settings for your site.";
        private const string _nooshTokenNotSpecifiedMessage = "Noosh access token not specified. Check settings for your site.";
        private const string _responseIncorrectMessage = "Response from microservice is not in correct format.";
        private const string _getProjectsIncorrectMessage = "Url for getting projects is not in correct format. Check settings for your site.";
        private const string _nooshApiIncorrectMessage = "Url for Noosh API is not in correct format. Check settings for your site.";

        /// <summary>
        /// Gets name of workgroup from K-Source settings for current site.
        /// </summary>
        /// <returns>Workgroup's name</returns>
        private static string GetWorkgroupName()
        {
            string customerName = SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.{_workgroupNameSettingKey}");
            if (string.IsNullOrWhiteSpace(customerName))
            {
                throw new InvalidOperationException(_customerNotSpecifiedMessage);
            }

            return customerName;
        }

        /// <summary>
        /// Gets access token for Noosh API from K-Source settings for current site.
        /// </summary>
        /// <returns>Noosh access token.</returns>
        private static string GetNooshToken()
        {
            string token = SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.{_nooshTokenSettingKey}");
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new InvalidOperationException(_nooshTokenNotSpecifiedMessage);
            }

            return token;
        }

        /// <summary>
        /// Gets URL for Noosh API from K-Source settings for current site.
        /// </summary>
        /// <returns>Noosh API URL.</returns>
        private static string GetNooshUrl()
        {
            Uri url;
            if (!Uri.TryCreate(SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.{_nooshApiSettingKey}")
                , UriKind.Absolute
                , out url))
            {
                throw new InvalidOperationException(_nooshApiIncorrectMessage);
            }

            return url.AbsoluteUri;
        }

        /// <summary>
        /// Get list of projects for workgroup.
        /// </summary>
        /// <returns>List of projects.</returns>
        public static IEnumerable<ProjectData> GetProjects()
        {
            var workgroupName = GetWorkgroupName();
            var nooshApi = GetNooshUrl();
            var nooshToken = GetNooshToken();

            Uri url;
            if (!Uri.TryCreate(SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.{_getProjectsSettingKey}")
                , UriKind.Absolute
                , out url))
            {
                throw new InvalidOperationException(_getProjectsIncorrectMessage);
            }

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Noosh.Url", nooshApi);
                client.DefaultRequestHeaders.Add("Noosh.Token", nooshToken);
                using (var message = client.GetAsync($"{url.AbsoluteUri}/{workgroupName}"))
                {
                    AwsResponseMessage<IEnumerable<ProjectData>> response;
                    try
                    {
                        response = (AwsResponseMessage<IEnumerable<ProjectData>>)message.Result;
                    }
                    catch (JsonReaderException e)
                    {
                        throw new InvalidOperationException(_responseIncorrectMessage, e);
                    }
                    if (response?.Success ?? false)
                    {
                        return response?.Response;
                    }
                    else
                    {
                        throw new HttpRequestException(response?.Error?.Message ?? message.Result.ReasonPhrase);
                    }
                }
            }
        }
    }
}