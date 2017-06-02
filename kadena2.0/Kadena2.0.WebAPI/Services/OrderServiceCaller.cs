using System;
using Kadena.Dto.SubmitOrder;
using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.Infrastructure.Responses;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Web.Http;
using System.Threading.Tasks;

namespace Kadena.WebAPI.Services
{
    public class OrderServiceCaller : IOrderServiceCaller
    {
        public async Task<OrderServiceResultDTO> SubmitOrder(string serviceEndpoint, OrderDTO orderData)
        {
            using (var httpClient = new HttpClient())
            {
                var requestBody = JsonConvert.SerializeObject(orderData, GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings);
                var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(serviceEndpoint, content);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string responseContent = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<OrderServiceResultDTO>(responseContent);
                }
                else
                {
                    return new OrderServiceResultDTO()
                    {
                        Success = false,
                        ErrorMessage = $"HTTP error - {response.StatusCode}"
                    };
                }
            }
        }
    }
}