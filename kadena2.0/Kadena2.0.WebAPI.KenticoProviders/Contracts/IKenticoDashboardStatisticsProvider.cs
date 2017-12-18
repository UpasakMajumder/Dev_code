using Kadena.Models.Dashboard;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoDashboardStatisticsProvider
    {
        StatisticBlock GetSalespersonStatistics();
    }
}
