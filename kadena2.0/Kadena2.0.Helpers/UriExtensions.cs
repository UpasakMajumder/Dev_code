using System;
using System.Web;

namespace Kadena.Helpers
{
    public static class UriExtensions
    {
        public static Uri AddParameter(this Uri url, string paramName, string paramValue)
        {
            var urlString = url.ToString();
            var newUrl = UrlHelper.SetQueryParameter(urlString, paramName, paramValue);
            return new Uri(newUrl, UriKind.RelativeOrAbsolute);
        }

        public static string GetParameter(this Uri uri, string parameterName)
        {
            var query = HttpUtility.ParseQueryString(uri.Query);
            return query[parameterName];
        }
    }
}
