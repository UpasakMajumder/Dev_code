using Kadena.WebAPI.Contracts;
using CMS.Ecommerce;
using Kadena.WebAPI.Models;
using AutoMapper;
using System.Linq;
using CMS.SiteProvider;

namespace Kadena.WebAPI.Services
{
    public class KenticoProviderService : IKenticoProviderService
    {
        private readonly IMapper mapper;
        public KenticoProviderService(IMapper mapper)
        {
            this.mapper = mapper;
        }
        
        public DeliveryAddress[] GetCustomerAddresses()
        {
            var customer = ECommerceContext.CurrentCustomer;
            var addresses = AddressInfoProvider.GetAddresses(customer.CustomerID).ToArray();
            return mapper.Map<DeliveryAddress[]>(addresses);
        }

        public DeliveryMethod[] GetShippingCarriers()
        {
            var shippingOptions = GetShippingOptions();
            var carriers = CarrierInfoProvider.GetCarriers(SiteContext.CurrentSiteID).ToArray();

            var deliveryMethods = mapper.Map<DeliveryMethod[]>(carriers);

            foreach (DeliveryMethod dm in deliveryMethods)
                dm.SetShippingOptions(shippingOptions);

            return deliveryMethods;
        }

        public DeliveryService[] GetShippingOptions()
        {
            var services = ShippingOptionInfoProvider.GetShippingOptions(SiteContext.CurrentSiteID).ToArray();
            return mapper.Map<DeliveryService[]>(services);
        }

        public Total[] GetShoppingCartTotals()
        {
            return new Total[]
            {
                new Total()
                {
                    Title = "Summary",
                    Value = ECommerceContext.CurrentShoppingCart.TotalItemsPrice.ToString()
                },
                new Total()
                {
                    Title = "Shipping",
                    Value = ECommerceContext.CurrentShoppingCart.TotalShipping.ToString()
                },
                new Total()
                {
                    Title = "Subtotal",
                    Value = "0"
                },
                new Total()
                {
                    Title = "Tax 8%",
                    Value = "0"
                },
                new Total()
                {
                    Title = "Totals",
                    Value = ECommerceContext.CurrentShoppingCart.TotalPrice.ToString()
                }
            };
        }
    }
}