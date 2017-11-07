using CMS.Ecommerce;
using Kadena.Models;
using System.Linq;

namespace Kadena2.WebAPI.KenticoProviders.Factories
{
    public static class DeliveryFactory
    {
        public static DeliveryCarrier CreateCarrier(CarrierInfo ci)
        {
            if (ci == null)
            {
                return null;
            }

            return new DeliveryCarrier()
            {
                Id = ci.CarrierID,
                Opened = false,
                Title = ci.CarrierDisplayName,
                Name = ci.CarrierName
            };
        }

        public static DeliveryCarrier[] CreateCarriers(CarrierInfo[] cis)
        {
            return cis.Select(c => CreateCarrier(c)).ToArray();
        }
    }
}
