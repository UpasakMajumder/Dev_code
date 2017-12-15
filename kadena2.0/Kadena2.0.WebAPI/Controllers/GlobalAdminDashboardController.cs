using System;
using System.Web.Http;
using Kadena.BusinessLogic.Contracts;
using AutoMapper;
using Kadena.DTO.Dashboard;
using Kadena.WebAPI.Infrastructure;

namespace Kadena.WebAPI.Controllers
{
    public class GlobalAdminDashboardController : ApiControllerBase
    {
        private readonly IDashboardStatisticsService dashboardStatisticsService;
        private readonly IMapper mapper;

        public GlobalAdminDashboardController(IDashboardStatisticsService dashboardStatisticsService, IMapper mapper)
        {
            if (dashboardStatisticsService == null)
            {
                throw new ArgumentNullException(nameof(dashboardStatisticsService));
            }

            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            this.mapper = mapper;
            this.dashboardStatisticsService = dashboardStatisticsService;
        }

        [HttpGet]
        [Route(("api/globaladmindashboard/getdashboardstatistics"))]
        public IHttpActionResult GetDashboardStatistics()
        {
            var statistics = dashboardStatisticsService.GetDashboardStatistics();
            var statisticsDto = mapper.Map<DashboardStatisticsDTO>(statistics);
            return ResponseJson(statisticsDto);
        }
    }
}