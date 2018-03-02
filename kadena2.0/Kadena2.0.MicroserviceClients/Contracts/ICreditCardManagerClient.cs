using Kadena.Dto.General;
using Kadena.Dto.Payment.CreditCard.MicroserviceRequests;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface ICreditCardManagerClient
    {
        Task<BaseResponseDto<object>> CreateCustomerContainer(CreateCustomerContainerRequestDto request);
        Task<BaseResponseDto<object>> UpdateCustomerContainer(UpdateCustomerContainerRequestDto request);
    }
}
