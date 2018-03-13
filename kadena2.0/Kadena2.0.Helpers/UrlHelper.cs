using System;

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
    }
}
