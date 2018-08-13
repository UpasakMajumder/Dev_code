using Kadena.Models;
using Kadena.Models.Address;
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
        void SetDefaultShippingAddress(int addressId);
        void UnsetDefaultShippingAddress();
        void SaveShippingAddress(DeliveryAddress address);
        List<AddressData> GetAddressesListByUserID(int userID, int inventoryType = 1, int campaignID = 0);
        IEnumerable<StateGroup> GetStateGroups();
    }
}
