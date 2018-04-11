using System;
using System.Web;

namespace Kadena.Helpers
{
    public static class UriExtensions
    {
        public static Uri AddParameter(this Uri url, string paramName, string paramValue)
        {
            var uriBuilder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query[paramName] = paramValue;
            uriBuilder.Query = HttpUtility.UrlDecode(query.ToString());

            return uriBuilder.Uri;
        }
        
    }
}
