using System;
using System.Collections.Specialized;
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
            var parametersPart = url;
            var parametersPartStart = url.IndexOf('?');
            if (parametersPartStart > 0)
            {
                parametersPart = url.Substring(parametersPartStart);
            }

            return HttpUtility.ParseQueryString(parametersPart);
        }
    }
}
