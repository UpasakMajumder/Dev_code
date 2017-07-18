using CMS.Ecommerce;
using Kadena.Models;
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

            return new DeliveryAddress()
            { 
                Id = ai.AddressID,
                Checked = false,
                City = ai.AddressCity,
                State = ai.GetStateCode(),
                Country = ai.GetCountryTwoLetterCode(),
                StateId = ai.AddressStateID,
                CountryId = ai.AddressCountryID,
                Street = new[] { ai.AddressLine1 }.ToList(),
                Zip = ai.AddressZip
            };
        }

        public static DeliveryAddress[] CreateDeliveryAddresses(IAddress[] ais)
        {
            return ais.Select(a => CreateDeliveryAddress(a)).ToArray();
        }
    }
}
