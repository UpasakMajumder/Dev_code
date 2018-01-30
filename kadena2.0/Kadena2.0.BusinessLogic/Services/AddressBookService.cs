using Kadena.BusinessLogic.Contracts;
using Kadena.Models;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Collections.Generic;

namespace Kadena.BusinessLogic.Services
{
    public class AddressBookService : IAddressBookService
    {
        private readonly IKenticoAddressBookProvider kenticoAddress;

        public AddressBookService(IKenticoAddressBookProvider kenticoAddress)
        {
            if (kenticoAddress == null)
            {
                throw new ArgumentNullException(nameof(kenticoAddress));
            }
            this.kenticoAddress = kenticoAddress;
        }

        public void DeleteAddress(int addressID)
        {
            kenticoAddress.DeleteAddress(addressID);
        }
        public List<DeliveryAddress> GetAddressesByAddressIds(List<int> addresssIds)
        {
           return kenticoAddress.GetAddressesByAddressIds(addresssIds);
        }
    }
}
