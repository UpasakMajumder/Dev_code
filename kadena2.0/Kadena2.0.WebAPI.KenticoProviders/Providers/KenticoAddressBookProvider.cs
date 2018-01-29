using AutoMapper;
using CMS.Ecommerce;
using CMS.SiteProvider;
using Kadena.Models;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoAddressBookProvider : IKenticoAddressBookProvider
    {

        private readonly IMapper mapper;
        public KenticoAddressBookProvider(IMapper mapper)
        {
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }
            this.mapper = mapper;
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
    }
}
