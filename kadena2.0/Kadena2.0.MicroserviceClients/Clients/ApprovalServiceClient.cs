using Kadena.Dto.Approval.MicroserviceRequests;
using Kadena.Dto.General;
using Kadena.Models.SiteSettings;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.MicroserviceClients.Contracts.Base;
using System;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Clients
{
    public class ApprovalServiceClient : SignedClientBase, IApprovalServiceClient
    {
        public ApprovalServiceClient(IMicroProperties properties)
        {
            _properties = properties ?? throw new ArgumentNullException(nameof(properties));
            _serviceVersionSettingKey = Settings.KDA_ApprovalServiceVersion;
        }

        public async Task<BaseResponseDto<string>> Approval(ApprovalRequestDto approval)
        {
            var url = $"{BaseUrl}/api/v{Version}/approval";
            return await Patch<string>(url, approval).ConfigureAwait(false);
        }
    }
}
