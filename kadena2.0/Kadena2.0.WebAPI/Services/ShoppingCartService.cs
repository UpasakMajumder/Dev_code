using Kadena.WebAPI.Contracts;
using CMS.Ecommerce;
using Kadena.WebAPI.Models;
using System;
using AutoMapper;
using System.Linq;

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

            return new CheckoutPage()
            {
                DeliveryAddresses = new DeliveryAddresses()
                {
                   AddAddressLabel = "New address",
                   Title = "Delivery",
                   Description = "Products will be delivered to selected address by",
                   items = addresses.ToList()
                }
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
    }
}