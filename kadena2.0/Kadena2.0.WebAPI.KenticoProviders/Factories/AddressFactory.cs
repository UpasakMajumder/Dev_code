using CMS.Ecommerce;
using CMS.Globalization;
using Kadena.Models;
using System.Collections.Generic;
using System.Linq;

namespace Kadena2.WebAPI.KenticoProviders.Factories
{
    public static class AddressFactory
    {
        public static DeliveryAddress CreateDeliveryAddress(IAddress ai)
        {
            if (ai == null)
            {
                return null;
            }

            var countryInfo = CountryInfoProvider.GetCountryInfo(ai.AddressCountryID);

            return new DeliveryAddress()
            { 
                Id = ai.AddressID,
                Checked = false,
                City = ai.AddressCity,
                State = ai.GetStateCode(),
                Country = countryInfo?.CountryDisplayName,
                StateId = ai.AddressStateID,
                CountryId = ai.AddressCountryID,
                Street1 = ai.AddressLine1,
                Street2 = ai.AddressLine2,
                Zip = ai.AddressZip
            };
        }

        public static DeliveryAddress[] CreateDeliveryAddresses(IAddress[] ais)
        {
            return ais.Select(a => CreateDeliveryAddress(a)).ToArray();
        }
    }
}
