using Kadena.Helpers.Routes;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace Kadena.Helpers
{
    public static class UrlHelper
    {
        public static string GetUrlForTemplatePreview(Guid templateId, Guid settingId)
        {
            return $"/api/template/{templateId}/preview/{settingId}";
        }

        public static Uri GetACSUri()
        {
            return new Uri("/api/login/saml2", UriKind.Relative);
        }

        public static NameValueCollection ParseQueryStringFromUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return new NameValueCollection();
            }

            var parametersPart = url;
            var parametersPartStart = url.IndexOf('?');
            if (parametersPartStart > 0)
            {
                parametersPart = url.Substring(parametersPartStart);
            }

            return HttpUtility.ParseQueryString(parametersPart);
        }

        public static string SetQueryParameter(string url, string parameterName, string value)
        {
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }
            if (parameterName == null)
            {
                throw new ArgumentNullException(nameof(parameterName));
            }

            var urlParts = url.Split('?');
            var path = urlParts[0];
            var query = urlParts
                .Skip(1)
                .FirstOrDefault() ?? string.Empty;
            var parameters = HttpUtility.ParseQueryString(query);

            parameters[parameterName] = value;

            var newParameters = new List<string>(parameters.Count + 1);
            foreach (var key in parameters.AllKeys)
            {
                var parameter = $"{key}={HttpUtility.UrlEncode(parameters[key])}";
                newParameters.Add(parameter);
            }

            var newUrl = $"{path}?{string.Join("&", newParameters)}";
            return newUrl;
        }

        public static Uri GetMailingListExportUrl(Guid containerId)
        {
            return new Uri($"/{Klist.Export.Replace("{containerId}", containerId.ToString())}", UriKind.Relative);
        }
    }
}
