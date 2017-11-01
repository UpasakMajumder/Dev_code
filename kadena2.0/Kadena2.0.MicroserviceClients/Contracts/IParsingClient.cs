using Kadena.Dto.General;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface IParsingClient
    {
        /// <summary>
        /// Requests for headers of specified file.
        /// </summary>
        /// <param name="fileId">Id for file to get headers for.</param>
        /// <returns>List of header names.</returns>
        Task<BaseResponseDto<IEnumerable<string>>> GetHeaders(string endPoint, string fileId);
    }
}
