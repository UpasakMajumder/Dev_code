using Kadena.Models.Dashboard;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IDashboardStatisticsService
    {
        DashboardStatistics GetDashboardStatistics();
    }
}
