using System;
using Kadena.Dto.SubmitOrder;
using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.Infrastructure.Responses;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Web.Http;
using System.Threading.Tasks;
using Kadena.WebAPI.Models.SubmitOrder;

namespace Kadena.WebAPI.Services
{
    public class OrderServiceCaller : IOrderServiceCaller
    {
        public async Task<SubmitOrderResult> SubmitOrder(string serviceEndpoint, OrderDTO orderData)
        {
            using (var httpClient = new HttpClient())
            {
                var requestBody = JsonConvert.SerializeObject(orderData, GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings);
                var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(serviceEndpoint, content);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string responseContent = response.Content.ReadAsStringAsync().Result;
                    var submitResponse = JsonConvert.DeserializeObject<SubmitOrderResult>(responseContent);
                    return submitResponse;
                }
                else
                {
                    return new SubmitOrderResult()
                    {
                        
                        Success = false,
                        Payload = null,
                        Error = $"HTTP error - {response.StatusCode}"
                    };
                }
            }
        }
    }
}