using Kadena.Dto.General;
using System;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface IAddressValidationClient
    {
        /// <summary>
        /// Forces microservices to start addresses validation for specified container.
        /// </summary>
        /// <param name="containerId">Id of container.</param>
        /// <returns>ID of file with valid addresses.</returns>
        Task<BaseResponseDto<string>> Validate(Guid containerId);
    }
}
