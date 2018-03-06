using Kadena.Models;
using System.Collections.Generic;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoAddressBookProvider
    {
        void DeleteAddress(int addressID);
        Dictionary<int, string> GetAddressNames();
        List<DeliveryAddress> GetAddressesByAddressIds(List<int> addresssIds);
        List<AddressData> GetAddressesList(int customerID);
        DeliveryAddress[] GetCustomerAddresses(AddressType addressType);
        DeliveryAddress[] GetCustomerAddresses(int customerId, AddressType addressType);
        DeliveryAddress[] GetCustomerShippingAddresses(int customerId);
        void SetDefaultShippingAddress(int addressId);
        void UnsetDefaultShippingAddress();
        void SaveShippingAddress(DeliveryAddress address, int customerId = 0);
    }
}
