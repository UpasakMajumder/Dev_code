using System.Linq;
using CMS.Membership;
using Kadena.Models.Dashboard;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoDashboardStatisticsProvider : IKenticoDashboardStatisticsProvider
    {
        public StatisticBlock GetSalespersonStatistics()
        {
            return new StatisticBlock()
            {
                Week = new StatisticsReading()
                {
                    Count = GetSalespersonStatistics(7)
                },
                Month = new StatisticsReading()
                {
                    Count = GetSalespersonStatistics(30)
                },
                Year = new StatisticsReading()
                {
                    Count = GetSalespersonStatistics(365)
                }
            };
        }

        private int GetSalespersonStatistics(int lastNDays)
        {
            return UserInfoProvider.GetUsers().Where(x => (DateTime.Now - x.UserCreated).Days <= lastNDays).ToList().Count;
        }
    }
}
