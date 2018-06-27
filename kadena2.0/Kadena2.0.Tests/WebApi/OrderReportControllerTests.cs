using Kadena.WebAPI.Controllers;
using Xunit;
using System;
using Kadena.BusinessLogic.Contracts.Orders;
using Kadena.BusinessLogic.Contracts;
using AutoMapper;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Kadena.WebAPI.Infrastructure.Communication;
using Kadena.Dto.Common;
using Kadena.Models.ModuleAccess;

namespace Kadena.Tests.WebApi
{
    public class OrderReportControllerTests : KadenaUnitTest<OrderReportController>
    {
        [Theory(DisplayName = "OrderReportController()")]
        [ClassData(typeof(OrderReportControllerTests))]
        public void OrderReportController(IOrderReportService orderReportService, IModuleAccessService moduleAccessService, IMapper mapper)
        {
            Assert.Throws<ArgumentNullException>(() => new OrderReportController(orderReportService, moduleAccessService, mapper));
        }

        [Fact(DisplayName = "OrderReportController.Get() | Success")]
        public async Task Get_Success()
        {
            Setup<IModuleAccessService, bool>(s => s.HasUserAccessToPageType(KnownPageTypes.OrdersReport), true);

            var actualResult = await Sut.Get();

            Assert.IsType<JsonResult<BaseResponse<TableViewDto>>>(actualResult);
        }

        [Fact(DisplayName = "OrderReportController.Get() | Unauthorized access")]
        public async Task Get_Unauthorized()
        {
            await Assert.ThrowsAsync<Exception>(() => Sut.Get());
        }
    }
}
