using AutoMapper;
using Kadena.BusinessLogic.Contracts.Orders;
using Kadena.Dto.OrderManualUpdate.Requests;
using Kadena.Dto.OrderManualUpdate.Responses;
using Kadena.Helpers.Routes;
using Kadena.Models.Orders;
using Kadena.WebAPI.Infrastructure;
using Kadena.WebAPI.Infrastructure.Filters;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace Kadena.WebAPI.Controllers
{
    [CustomerAuthorizationFilter]
    public class OrderUpdateController : ApiControllerBase
    {
        private readonly IOrderManualUpdateService updateService;
        private readonly IMapper mapper;

        public OrderUpdateController(IOrderManualUpdateService updateService, IMapper mapper)
        {
            this.updateService = updateService ?? throw new ArgumentNullException(nameof(updateService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost]
        [Route(Routes.Order.OrderUpdate)]
        public async Task<IHttpActionResult> UpdateOrder(OrderUpdateDto request)
        {
            var requestModel = mapper.Map<OrderUpdate>(request);
            var result = await updateService.UpdateOrder(requestModel);
            var resultDto = mapper.Map<OrderUpdateResultDto>(result);
            return ResponseJson(resultDto);
        }

        [HttpPost]
        [Route(Routes.Order.OrderShippingUpdate)]
        public async Task<IHttpActionResult> UpdateOrdersShippings(object[] items)
        {
            var request = mapper.Map<UpdateShippingRow[]>(items);
            var result = await updateService.UpdateOrdersShippings(request);

            if (result.Item1)
            {
                return SuccessJson();
            }

            return ErrorJson(result.Item2);
        }
    }
}