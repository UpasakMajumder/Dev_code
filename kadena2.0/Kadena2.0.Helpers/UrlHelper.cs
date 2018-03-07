using System;

namespace Kadena.Helpers
{
    public static class UrlHelper
    {
        public static string GetUrlForTemplatePreview(Guid templateId, Guid settingId)
        {
            return $"/api/template/{templateId}/preview/{settingId}";
        }
    }
}
