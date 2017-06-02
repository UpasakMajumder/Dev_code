using Kadena.Dto.SubmitOrder;
using Kadena.WebAPI.Infrastructure.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kadena.WebAPI.Contracts
{
    public interface IOrderServiceCaller
    {
        Task<OrderServiceResultDTO> SubmitOrder(string serviceEndpoint, OrderDTO orderData);
    }
}
