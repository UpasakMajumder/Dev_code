using Kadena.WebAPI.Models;
using Kadena.WebAPI.Models.Settings;

namespace Kadena.WebAPI.Contracts
{
    public interface ISettingsService
    {
        SettingsAddresses GetAddresses();
        void SaveShippingAddress(DeliveryAddress address);
    }
}