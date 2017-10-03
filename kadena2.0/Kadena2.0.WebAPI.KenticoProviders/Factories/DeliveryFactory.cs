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


        public static DeliveryOption CreateOption(ShippingOptionInfo s)
        {
            if (s == null)
            {
                return null;
            }

            return new DeliveryOption()
            {
                Id = s.ShippingOptionID,
                CarrierId = s.ShippingOptionCarrierID,
                Title = s.ShippingOptionDisplayName,
                Service = s.ShippingOptionCarrierServiceName,
            };
        }

        public static DeliveryOption[] CreateOptions(ShippingOptionInfo[] options)
        {
            return options.Select(s => CreateOption(s)).ToArray();
        }
    }
}
