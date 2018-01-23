using AutoMapper;
using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using Kadena.Models.SubmitOrder;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.BusinessLogic.Contracts.OrderPayment;
using Kadena2.MicroserviceClients.Contracts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Kadena2.BusinessLogic.Services.OrderPayment
{
    public class PurchaseOrder : IPurchaseOrder
    {
        private readonly IShoppingCartProvider shoppingCart;
        private readonly IKenticoResourceService resources;
        private readonly IKenticoDocumentProvider documents;
        private readonly IKenticoUserProvider users;
        private readonly IOrderSubmitClient orderClient;
        private readonly IKenticoLogger log;
        private readonly IMapper mapper;

        public PurchaseOrder(IShoppingCartProvider shoppingCart, IKenticoResourceService resources, IKenticoDocumentProvider documents, IKenticoUserProvider users, IOrderSubmitClient orderClient, IKenticoLogger log, IMapper mapper)
        {
            if (shoppingCart == null)
            {
                throw new ArgumentNullException(nameof(shoppingCart));
            }
            if (resources == null)
            {
                throw new ArgumentNullException(nameof(resources));
            }
            if (documents == null)
            {
                throw new ArgumentNullException(nameof(documents));
            }
            if (users == null)
            {
                throw new ArgumentNullException(nameof(users));
            }
            if (orderClient == null)
            {
                throw new ArgumentNullException(nameof(orderClient));
            }
            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            this.shoppingCart = shoppingCart;
            this.resources = resources;
            this.documents = documents;
            this.orderClient = orderClient;
            this.users = users;
            this.log = log;
            this.mapper = mapper;
        }

        public async Task<SubmitOrderResult> SubmitPOOrder(OrderDTO orderData)
        {
            string serviceEndpoint = resources.GetSettingsKey("KDA_OrderServiceEndpoint");

            if ((orderData?.Items?.Count() ?? 0) <= 0)
            {
                throw new ArgumentOutOfRangeException("Items", "Cannot submit order without items");
            }

            var serviceResultDto = await orderClient.SubmitOrder(orderData);
            var serviceResult = mapper.Map<SubmitOrderResult>(serviceResultDto);

            var redirectUrlBase = resources.GetSettingsKey("KDA_OrderSubmittedUrl");
            var redirectUrlBaseLocalized = documents.GetDocumentUrl(redirectUrlBase);
            var redirectUrl = $"{redirectUrlBaseLocalized}?success={serviceResult.Success}".ToLower();
            if (serviceResult.Success)
            {
                redirectUrl += "&order_id=" + serviceResult.Payload;
            }
            serviceResult.RedirectURL = redirectUrl;

            if (serviceResult.Success)
            {
                log.LogInfo("Submit order", "INFORMATION", $"Order {serviceResult.Payload} successfully created");
                shoppingCart.RemoveCurrentItemsFromStock();
                shoppingCart.ClearCart();
            }
            else
            {
                log.LogError("Submit order", $"Order {serviceResult?.Payload} error. {serviceResult?.Error?.Message}");
            }

            return serviceResult;
        }
    }
}
