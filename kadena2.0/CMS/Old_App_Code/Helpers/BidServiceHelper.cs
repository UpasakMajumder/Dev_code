using CMS.DataEngine;
using CMS.SiteProvider;
using Kadena.Old_App_Code.Kadena.KSource;
using Kadena.Dto.General;
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

        private const string _customerNotSpecifiedMessage = "Workgroup's name not specified. Check settings for your site.";
        private const string _responseIncorrectMessage = "Response from microservice is not in correct format.";
        private const string _getProjectsIncorrectMessage = "Url for getting projects is not in correct format. Check settings for your site.";

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
        /// Get list of projects for workgroup.
        /// </summary>
        /// <returns>List of projects.</returns>
        public static IEnumerable<ProjectData> GetProjects()
        {
            var workgroupName = GetWorkgroupName();

            Uri url;
            if (!Uri.TryCreate(SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.{_getProjectsSettingKey}")
                , UriKind.Absolute
                , out url))
            {
                throw new InvalidOperationException(_getProjectsIncorrectMessage);
            }

            using (var client = new HttpClient())
            {
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
                        return response?.Payload;
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