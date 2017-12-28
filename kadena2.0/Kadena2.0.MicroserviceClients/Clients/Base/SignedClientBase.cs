using Kadena2.MicroserviceClients.Helpers;
using System.Net.Http;
using System.Threading.Tasks;
using Kadena.Dto.General;
using Kadena2.MicroserviceClients.Contracts.Base;

namespace Kadena2.MicroserviceClients.Clients.Base
{
    public class SignedClientBase : ClientBase
    {
        public SignedClientBase() : base()
        { }

        protected SignedClientBase(ISuppliantDomainClient suppliantDomain) : base(suppliantDomain)
        { }

        protected sealed override async Task<BaseResponseDto<TResult>> SendRequest<TResult>(HttpRequestMessage request)
        {
            await SignRequestMessage(request).ConfigureAwait(false);
            return await base.SendRequest<TResult>(request).ConfigureAwait(false);
        }

        private async Task SignRequestMessage(HttpRequestMessage request)
        {
            await AwsSignerHelper.Sign(request).ConfigureAwait(false);
        }
    }
}
