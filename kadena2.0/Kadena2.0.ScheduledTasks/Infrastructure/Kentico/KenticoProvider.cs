using CMS.SiteProvider;
using System.Linq;

namespace Kadena.ScheduledTasks.Infrastructure.Kentico
{
    public class KenticoProvider : IKenticoProvider
    {
        public string[] GetSites()
        {
            return SiteInfoProvider.GetSites()
                .ToList()
                .Select(s => s.SiteName)
                .ToArray();
        }
    }
}