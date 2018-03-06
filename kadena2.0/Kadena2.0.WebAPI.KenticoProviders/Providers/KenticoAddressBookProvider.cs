using AutoMapper;
using CMS.Ecommerce;
using CMS.SiteProvider;
using Kadena.Models;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.WebAPI.KenticoProviders.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoAddressBookProvider : IKenticoAddressBookProvider
    {

        private readonly IMapper mapper;
        private readonly IShoppingCartProvider shoppingCartProvider;
        public KenticoAddressBookProvider(IMapper mapper, IShoppingCartProvider shoppingCartProvider)
        {
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }
            if (shoppingCartProvider == null)
            {
                throw new ArgumentNullException(nameof(shoppingCartProvider));
            }
            this.mapper = mapper;
            this.shoppingCartProvider = shoppingCartProvider;
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
            return AddressInfoProvider.GetAddresses().ToDictionary(x => x.AddressID, x => x.AddressName);
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

        public List<AddressData> GetAddressesListByUserID(int userID, int inventoryType = 1)
        {
            List<AddressData> myAddressList = new List<AddressData>();
            int currentCustomerId = new KenticoCustomerProvider().GetCustomerIDByUserID(userID);
            if (currentCustomerId != default(int))
            {
                myAddressList = GetAddressesList(currentCustomerId)?.Select(x =>
                {
                    x.DistributorShoppingCartID = shoppingCartProvider.GetDistributorCartID(x.AddressID, inventoryType);
                    return x;
                }).ToList();
            }
            return myAddressList;
        }
    }
}
