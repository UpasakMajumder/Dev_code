using Amazon.SecurityToken.Model;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kadena.KOrder.PaymentService.Infrastucture.Helpers
{
    public interface IAwsV4Signer
    {
        void SetService(string service, string region);

        Task SignRequest(HttpRequestMessage request, AssumeRoleResponse assumedRole);

        Task SignRequest(HttpRequestMessage request, string gatewayApiRole);

        Task SignRequest(HttpRequestMessage request, string accessKey, string secretKey);
    }
}
