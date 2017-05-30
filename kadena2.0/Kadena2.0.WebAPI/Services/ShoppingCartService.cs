using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.Models;
using AutoMapper;
using System.Linq;

namespace Kadena.WebAPI.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IMapper mapper;
        private readonly IKenticoProviderService kenticoProvider;

        public ShoppingCartService(IMapper mapper, IKenticoProviderService kenticoProvider)
        {
            this.mapper = mapper;
            this.kenticoProvider = kenticoProvider;
        }

        public CheckoutPage GetCheckoutPage()
        {
            var addresses = kenticoProvider.GetCustomerAddresses();
            var carriers = kenticoProvider.GetShippingCarriers();
            var totals = kenticoProvider.GetShoppingCartTotals();

            return new CheckoutPage()
            {
                DeliveryAddresses = new DeliveryAddresses()
                {
                    AddAddressLabel = "New address",
                    Title = "Delivery",
                    Description = "Products will be delivered to selected address by",
                    items = addresses.ToList()
                },

                DeliveryMethod = new DeliveryMethods()
                {
                    Title = "Delivery",
                    Description = "Select delivery carrier and option",
                    items = carriers.ToList(),
                },

                Totals = new Totals()
                {
                    Title = "Total",
                    Description = null,
                    Items = totals.ToList()
                },

                SubmitLabel = "Place order"
            };
        }
    }
}