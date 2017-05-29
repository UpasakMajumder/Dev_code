using Kadena.WebAPI.Contracts;
using CMS.Ecommerce;
using Kadena.WebAPI.Models;
using System;
using AutoMapper;
using System.Linq;
using CMS.SiteProvider;

namespace Kadena.WebAPI.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        IMapper mapper;
        public ShoppingCartService(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public CheckoutPage GetCheckoutPage()
        {
            var addresses = GetCustomerAddresses();
            var carriers = GetShippingCarriers();

            return new CheckoutPage()
            {
                DeliveryAddresses = new DeliveryAddresses()
                {
                    AddAddressLabel = "New address",
                    Title = "Delivery",
                    Description = "Products will be delivered to selected address by",
                    items = addresses.ToList()
                },

                DeliveryMethods = new DeliveryMethods()
                {
                    Title = "Delivery",
                    Description = "Select delivery carrier and option",
                    items = carriers.ToList(),
                },

                SubmitLabel = "Place order"
            };
        }

        public DeliveryAddress[] GetCustomerAddresses()
        {
            var customer = ECommerceContext.CurrentCustomer;
            if (customer == null)
                throw new Exception("Unknown customer");

            var addresses = AddressInfoProvider.GetAddresses(customer.CustomerID).ToArray();

            return mapper.Map<DeliveryAddress[]>(addresses);
        }

        public DeliveryMethod[] GetShippingCarriers()
        {
            var carriers = CarrierInfoProvider.GetCarriers(SiteContext.CurrentSiteID).ToArray();
            return mapper.Map<DeliveryMethod[]>(carriers);
        }

        public DeliveryService[] GetShippingOptions()
        {
            //ShippingOptionInfo i;
            return null;
        }
    }
}