using Kadena.Helpers;
using System.Collections.Generic;

namespace Kadena.Models.TemplatedProduct
{
    public static class EditorUrl
    {
        public static string Create(string productEditorBaseUrl, int nodeId, string templateId, string workspaceid,
            int quantity = 0, bool use3d = false, string containerId = null, string customName = null)
        {
            var url = UrlHelper.SetQueryParameters(productEditorBaseUrl, new[] 
            {
                new KeyValuePair<string, string>("nodeId", nodeId.ToString()),
                new KeyValuePair<string, string>("templateId", templateId),
                new KeyValuePair<string, string>("workspaceid", workspaceid),
                new KeyValuePair<string, string>("use3d", use3d.ToString()),
            });

            if (quantity > 0)
            {
                url = UrlHelper.SetQueryParameter(url, "quantity", quantity.ToString());
            }

            if (containerId != null)
            {
                url = UrlHelper.SetQueryParameter(url, "containerId", containerId);
            }

            if (!string.IsNullOrWhiteSpace(customName))
            {
                url = UrlHelper.SetQueryParameter(url, "customName", customName);
            }

            return url.ToString();
        }
    }
}
