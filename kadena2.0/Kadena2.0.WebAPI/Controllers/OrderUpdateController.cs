using AutoMapper;
using Kadena.BusinessLogic.Contracts.Orders;
using Kadena.Dto.OrderManualUpdate.Requests;
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
            mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            updateService = updateService ?? throw new ArgumentNullException(nameof(updateService));
        }

        [HttpPost]
        [Route("api/orderupdate")]
        public async Task<IHttpActionResult> UpdateOrder(OrderItemUpdateDto[] request)
        {
            var items = mapper.Map<OrderItemUpdate[]>(request);
            await updateService.UpdateOrderItems(items);
            return ResponseJson((string)null);
        }
    }
}