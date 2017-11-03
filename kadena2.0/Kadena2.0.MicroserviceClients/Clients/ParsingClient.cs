using System.Collections.Generic;
using System.Threading.Tasks;
using Kadena.Dto.General;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;

namespace Kadena2.MicroserviceClients.Clients
{
    public class ParsingClient : ClientBase, IParsingClient
    {
        public async Task<BaseResponseDto<IEnumerable<string>>> GetHeaders(string endPoint, string fileId)
        {
            var url = $"{endPoint}/api/CsvParser/GetHeaders?FileId={fileId}&Module={FileModule.KList}";
            return await Get<IEnumerable<string>>(url).ConfigureAwait(false);
        }
    }
}
