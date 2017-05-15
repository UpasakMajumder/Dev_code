using System.Collections.Generic;
using System.Linq;
using CMS;
using Kadena2.Carriers;

[assembly: RegisterCustomClass("USPSCarrier", typeof(USPSCarrier))]

namespace Kadena2.Carriers
{
    public class USPSCarrier : CarrierBase
    {
        public USPSCarrier() : base()
        {
            ProviderApiKey = "USPS";
            CarrierProviderName = "USPS";
        }

        public override List<KeyValuePair<string, string>> GetServices()
        {
            var services = new SortedDictionary<string, string>
            {
                { "First","1st Class" },
                { "First#","First Class" },
                { "First##","STANDARD MAIL" },
                { "FirstClassMailInternational","First-Class Mail International" },
                { "MediaMail","Media Mail" },
                { "Priority","Priority Mail" }
            };

            return services.ToList();
        }
    }
}
