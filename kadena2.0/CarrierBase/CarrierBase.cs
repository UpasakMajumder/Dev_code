using CMS.DataEngine;
using CMS.Ecommerce;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Kadena2.CarrierBase
{
    public class CarrierBase
    {
        protected string ServiceUrl { get; set; }

        protected string ProviderApiKey { get; set; }

        public CarrierBase()
        {
            ServiceUrl = SettingsKeyInfoProvider.GetValue("KDA_EstimateDeliveryPriceServiceEndpoint");
        }

        protected EstimateDeliveryPriceResponse CallEstimationService(string requestBody)
        {
            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                var response = httpClient.PostAsync(ServiceUrl, content).Result;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string responseContent = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<EstimateDeliveryPriceResponse>(responseContent);
                }
                else
                {
                    return new EstimateDeliveryPriceResponse() { success = false, response = new Response() { serviceSuccess = false, cost = 0} };
                }
            }
        }

        protected EstimateDeliveryPriceRequest GetEstimatePriceRequest(Delivery delivery, string provider, string service )
        {
            return new EstimateDeliveryPriceRequest()
            {
                provider = provider,
                providerService = service,
                sourceAddress = new Address()
                {
                    city = delivery.DeliveryAddress.AddressCity,
                    country = delivery.DeliveryAddress.GetCountryTwoLetterCode(),
                    postal = delivery.DeliveryAddress.AddressZip,
                    state = delivery.DeliveryAddress.GetStateCode(),
                    streetLines = new List<string> { delivery.DeliveryAddress.AddressLine1, delivery.DeliveryAddress.AddressLine2 }
                },
                targetAddress = new Address()
                {
                    city = delivery.DeliveryAddress.AddressCity,
                    country = delivery.DeliveryAddress.GetCountryTwoLetterCode(),
                    postal = delivery.DeliveryAddress.AddressZip,
                    state = delivery.DeliveryAddress.GetStateCode(),
                    streetLines = new List<string> { delivery.DeliveryAddress.AddressLine1, delivery.DeliveryAddress.AddressLine2 }
                },
                weight = new Weight() { unit = "Lb", value = (double)delivery.Weight }
            };
        }
    }
}
