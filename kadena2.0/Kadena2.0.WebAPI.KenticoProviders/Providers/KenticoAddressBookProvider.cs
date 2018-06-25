using AutoMapper;
using CMS.Ecommerce;
using Kadena.Models;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.WebAPI.KenticoProviders.Contracts.KadenaSettings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoAddressBookProvider : IKenticoAddressBookProvider
    {
        public static string CustomerDefaultShippingAddresIDFieldName => "CustomerDefaultShippingAddresID";

        private readonly IMapper mapper;
        private readonly IShoppingCartProvider shoppingCartProvider;
        private readonly IKadenaSettings kadenaSettings;
        private readonly IKenticoLocalizationProvider localizationProvider;
        private readonly IKenticoCustomerProvider customers;

        public KenticoAddressBookProvider(IMapper mapper, IShoppingCartProvider shoppingCartProvider, IKadenaSettings kadenaSettings, IKenticoLocalizationProvider localizationProvider, IKenticoCustomerProvider customers)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.shoppingCartProvider = shoppingCartProvider ?? throw new ArgumentNullException(nameof(shoppingCartProvider));
            this.kadenaSettings = kadenaSettings ?? throw new ArgumentNullException(nameof(kadenaSettings));
            this.localizationProvider = localizationProvider ?? throw new ArgumentNullException(nameof(localizationProvider));
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

        public void SaveShippingAddress(DeliveryAddress address, int customerId = 0)
        {
            if (address != null)
            {
                if (address.State != null && address.State.Id == 0)
                {
                    address.State = localizationProvider.GetStates().FirstOrDefault(c => c.StateCode == address.State.StateCode);
                }

                if (address.Country != null && address.Country.Id == 0)
                {
                    address.Country = localizationProvider.GetCountries().FirstOrDefault(c => c.Code == address.Country.Code);
                }

                if (address.Country != null)
                {
                    CustomerInfo customer = customerId > 0
                        ? CustomerInfoProvider.GetCustomerInfo(customerId)
                        : ECommerceContext.CurrentCustomer;

                    if (string.IsNullOrWhiteSpace(address.AddressPersonalName))
                    {
                        address.AddressPersonalName = $"{customer.CustomerFirstName} {customer.CustomerLastName}";
                    }

                    if (string.IsNullOrWhiteSpace(address.CompanyName))
                    {
                        if (customer.CustomerHasCompanyInfo)
                        {
                            address.CompanyName = customer.CustomerCompany;
                        }
                        else
                        {
                            address.CompanyName = kadenaSettings.DefaultCustomerCompanyName;
                        }
                    }

                    if (string.IsNullOrWhiteSpace(address.AddressName))
                    {
                        address.AddressName = $"{address.AddressPersonalName}, {address.Address1}, {address.City}";
                    }

                    var info = mapper.Map<AddressInfo>(address);
                    info.SetValue("AddressType", AddressType.Shipping.Code);
                    info.AddressCustomerID = customer.CustomerID;

                    AddressInfoProvider.SetAddressInfo(info);
                    address.Id = info.AddressID;
                }
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
                    x.DistributorShoppingCartID = shoppingCartProvider.GetDistributorCartID(x.AddressID, inventoryType, campaignID);
                    return x;
                }).ToList();
            }
            return myAddressList;
        }
    }
}
