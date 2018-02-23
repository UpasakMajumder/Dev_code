using AutoMapper;
using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using Kadena.Models.SubmitOrder;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.BusinessLogic.Contracts.Orders;
using Kadena2.MicroserviceClients.Contracts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Kadena2.BusinessLogic.Services.Orders
{
    public class SendSubmitOrder : ISendSubmitOrder
    {
        private readonly IMapper mapper;
        private readonly IOrderSubmitClient orderClient;
        private readonly IKenticoResourceService resources;
        private readonly IKenticoLogger log;

        public SendSubmitOrder(IMapper mapper, IOrderSubmitClient orderClient, IKenticoResourceService resources, IKenticoLogger log)
        {
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }
            if (orderClient == null)
            {
                throw new ArgumentNullException(nameof(orderClient));
            }
            if (resources == null)
            {
                throw new ArgumentNullException(nameof(resources));
            }
            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }

            this.mapper = mapper;
            this.orderClient = orderClient;
            this.resources = resources;
            this.log = log;
        }

        public async Task<SubmitOrderResult> SubmitOrderData(OrderDTO orderData)
        {
            if ((orderData?.Items?.Count() ?? 0) <= 0)
            {
                throw new ArgumentOutOfRangeException("Items", "Cannot submit order without items");
            }

            var serviceResultDto = await orderClient.SubmitOrder(orderData);

            if (serviceResultDto.Success)
            {
                log.LogInfo("Submit order", "INFORMATION", $"Order {serviceResultDto.Payload} successfully saved in microservice");             
            }
            else
            {
                log.LogError("Submit order", $"Order {serviceResultDto.Payload} failed to save in microservice. {serviceResultDto.ErrorMessages}");
            }

            return new SubmitOrderResult
            {
                Success = serviceResultDto.Success,
                OrderId = serviceResultDto.Payload,
                Error = serviceResultDto.ErrorMessages,
            };
        }
    }
}
