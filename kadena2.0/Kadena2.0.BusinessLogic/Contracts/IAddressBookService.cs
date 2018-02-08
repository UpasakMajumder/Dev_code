using Kadena.Models;
using System.Collections.Generic;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IAddressBookService
    {
        void DeleteAddress(int addressID);
        List<DeliveryAddress> GetAddressesByAddressIds(List<int> addresssIds);
    }
}
