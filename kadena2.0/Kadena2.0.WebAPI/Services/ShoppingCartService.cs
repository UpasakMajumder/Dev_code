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
                DeliveryAddresses = addresses.ToArray()
            };
        }

        public Address[] GetCustomerAddresses()
        {
            var customer = ECommerceContext.CurrentCustomer;
            if (customer == null)
                throw new Exception("Unknown customer");

            var addresses = AddressInfoProvider.GetAddresses(customer.CustomerID).ToArray();

            return mapper.Map<Address[]>(addresses);
        }
    }
}