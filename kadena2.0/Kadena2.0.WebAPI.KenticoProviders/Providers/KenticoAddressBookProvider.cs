using AutoMapper;
using CMS.CustomTables;
using CMS.Ecommerce;
using Kadena.Models;
using Kadena.Models.Address;
using Kadena.Models.ShoppingCarts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.WebAPI.KenticoProviders.Contracts.KadenaSettings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoAddressBookProvider : IKenticoAddressBookProvider
    {
        private readonly string StateGroupTableName = "KDA.StatesGroup";

        public static string CustomerDefaultShippingAddresIDFieldName => "CustomerDefaultShippingAddresID";

        private readonly IMapper mapper;
        private readonly IShoppingCartProvider shoppingCartProvider;
        private readonly IKenticoCustomerProvider customers;

        public KenticoAddressBookProvider(IMapper mapper, IShoppingCartProvider shoppingCartProvider, IKenticoCustomerProvider customers)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.shoppingCartProvider = shoppingCartProvider ?? throw new ArgumentNullException(nameof(shoppingCartProvider));
            this.customers = customers ?? throw new ArgumentNullException(nameof(customers));
        }

        public void DeleteAddress(int addressID)
        {
            var address = AddressInfoProvider.GetAddressInfo(addressID);
            if (address != null)
            {
                AddressInfoProvider.DeleteAddressInfo(addressID);
            }
        }

        public Dictionary<int, string> GetAddressNames()
        {
            return AddressInfoProvider.GetAddresses().ToDictionary(x => x.AddressID, x => x.AddressPersonalName);
        }

        public List<DeliveryAddress> GetAddressesByAddressIds(List<int> addressIds)
        {
            var addresses = AddressInfoProvider.GetAddresses().WhereIn("AddressID", addressIds).ToList();
            return mapper.Map<List<DeliveryAddress>>(addresses);
        }

        public List<AddressData> GetAddressesList(int customerID)
        {
            var addressesList = AddressInfoProvider.GetAddresses(customerID).Columns("AddressID", "AddressPersonalName").WhereEquals("Status", true).ToList();
            return mapper.Map<List<AddressData>>(addressesList);
        }

        public DeliveryAddress[] GetCustomerAddresses(AddressType addressType)
        {
            var customer = ECommerceContext.CurrentCustomer;
            return GetCustomerAddresses(customer.CustomerID, addressType);
        }

        public DeliveryAddress[] GetCustomerAddresses(int customerId, AddressType addressType)
        {
            var query = AddressInfoProvider.GetAddresses(customerId);
            if (addressType != null)
            {
                query = query.Where($"AddressType ='{addressType}'");
            }
            return mapper.Map<DeliveryAddress[]>(query.ToArray());
        }

        public void SetDefaultShippingAddress(int addressId)
        {
            var customer = ECommerceContext.CurrentCustomer;

            if (customer != null)
            {
                customer.SetValue(CustomerDefaultShippingAddresIDFieldName, addressId);
                CustomerInfoProvider.SetCustomerInfo(customer);
            }
        }

        public void UnsetDefaultShippingAddress()
        {
            SetDefaultShippingAddress(0);
        }

        public void SaveShippingAddress(DeliveryAddress address)
        {
            var info = mapper.Map<AddressInfo>(address);
            if (info != null)
            {
                info.SetValue("AddressType", AddressType.Shipping.Code);
                AddressInfoProvider.SetAddressInfo(info);
                address.Id = info.AddressID;
            }
        }

        public List<AddressData> GetAddressesListByUserID(int userID, int inventoryType = 1, int campaignID = 0)
        {
            var myAddressList = new List<AddressData>();
            var customer = customers.GetCustomerByUser(userID);
            if (customer != null)
            {
                myAddressList = GetAddressesList(customer.Id)?.Select(x =>
                {
                    x.DistributorShoppingCartID = shoppingCartProvider.GetDistributorCartID(x.AddressID, (ShoppingCartTypes)inventoryType, campaignID);
                    return x;
                }).ToList();
            }
            return myAddressList;
        }

        public IEnumerable<StateGroup> GetStateGroups()
        {
            var stateGroups = CustomTableItemProvider.GetItems(StateGroupTableName).ToList();
            return mapper.Map<IEnumerable<StateGroup>>(stateGroups);
        }
    }
}
