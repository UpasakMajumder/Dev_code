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
        private readonly IOrderManualUpdateService _updateService;
        private readonly IMapper _mapper;

        public OrderUpdateController(IOrderManualUpdateService updateService, IMapper mapper)
        {
            this._updateService = updateService ?? throw new ArgumentNullException(nameof(updateService));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost]
        [Route(Routes.Order.OrderUpdate)]
        public async Task<IHttpActionResult> UpdateOrder(OrderUpdateDto request)
        {
            var requestModel = _mapper.Map<OrderUpdate>(request);
            var result = await _updateService.UpdateOrder(requestModel);
            var resultDto = _mapper.Map<OrderUpdateResultDto>(result);
            return ResponseJson(resultDto);
        }

        [HttpPost]
        [Route(Routes.Order.OrderShippingUpdate)]
        public async Task<IHttpActionResult> UpdateOrdersShippings(UpdateShippingRow[] items)
        {
            var request = _mapper.Map<UpdateShippingRow[]>(items);
            var result = await _updateService.UpdateOrdersShippings(request);

            if (result.Success)
            {
                return this.ResponseJson(new {Message = result.Message});
            }

            return ErrorJson(result.Message);
        }
    }
}