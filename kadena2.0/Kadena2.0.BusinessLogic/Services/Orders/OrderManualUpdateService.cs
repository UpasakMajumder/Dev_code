using Kadena.BusinessLogic.Contracts.Orders;
using Kadena.Dto.OrderManualUpdate.MicroserviceRequests;
using Kadena.Models.Orders;
using Kadena2.MicroserviceClients.Contracts;
using System;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Services.Orders
{
    public class OrderManualUpdateService : IOrderManualUpdateService
    {
        private readonly IOrderManualUpdateClient updateService;

        public OrderManualUpdateService(IOrderManualUpdateClient updateService)
        {
            this.updateService = updateService ?? throw new ArgumentNullException(nameof(updateService));
        }

        public async Task UpdateOrder(OrderUpdate request)
        {

            var requestDto = new OrderManualUpdateRequestDto
            {

            };


            /*updateService.UpdateOrder()*/

        }
    }
}
