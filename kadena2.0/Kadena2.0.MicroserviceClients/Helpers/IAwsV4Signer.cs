using Amazon.SecurityToken.Model;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kadena.KOrder.PaymentService.Infrastucture.Helpers
{
    interface IAwsV4Signer
    {
        Task SignRequest(HttpRequestMessage request);

        Task SignRequest(HttpRequestMessage request, AssumeRoleResponse assumedRole);

        Task SignRequest(HttpRequestMessage request, string gatewayApiRole);

        Task SignRequest(HttpRequestMessage request, string accessKey, string secretKey);
    }
}
