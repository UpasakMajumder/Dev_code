using System;
using System.Collections.Generic;
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

        public static string SetQueryParameter(string url, string parameterName, string value)
        {
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

        public static string GetPathFromUrl(string url)
        {
            var uri = new Uri(url, UriKind.RelativeOrAbsolute);
            if (uri.IsAbsoluteUri)
            {
                return uri.LocalPath;
            }

            var path = url
                .Split('?')
                .FirstOrDefault();

            if (path?.Length > 0)
            {
                return path;
            }

            return "/";
        }

        public static string GetQueryFromUrl(string url)
        {
            var queryStartIndex = url.IndexOf('?');
            if (queryStartIndex >= 0)
            {
                return url.Substring(queryStartIndex + 1);
            }

            return string.Empty;
        }
    }
}
