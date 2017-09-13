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

            var addressLines = new List<string> { ai.AddressLine1 };
            if (!string.IsNullOrWhiteSpace(ai.AddressLine2))
            {
                addressLines.Add(ai.AddressLine2);
            }

            var countryInfo = CountryInfoProvider.GetCountryInfo(ai.AddressCountryID);

            return new DeliveryAddress()
            { 
                Id = ai.AddressID,
                Checked = false,
                City = ai.AddressCity,
                State = ai.GetStateCode(),
                Country = countryInfo?.CountryDisplayName,
                CountryCode = ai.GetCountryTwoLetterCode(),
                StateId = ai.AddressStateID,
                CountryId = ai.AddressCountryID,
                Street = addressLines,
                Zip = ai.AddressZip
            };
        }

        public static DeliveryAddress[] CreateDeliveryAddresses(IAddress[] ais)
        {
            return ais.Select(a => CreateDeliveryAddress(a)).ToArray();
        }
    }
}
