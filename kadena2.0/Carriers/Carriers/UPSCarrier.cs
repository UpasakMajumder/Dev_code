using System.Collections.Generic;
using System.Linq;
using CMS;
using Kadena2.Carriers;

[assembly: RegisterCustomClass("UPSCarrier", typeof(UPSCarrier))]

namespace Kadena2.Carriers
{
    public class UPSCarrier : CarrierBase
    {
        public UPSCarrier() : base()
        {
            ProviderApiKey = "UPS";
            CarrierProviderName = "UPS";
        }

        public override List<KeyValuePair<string, string>> GetServices()
        {
            var services = new SortedDictionary<string, string>
            {
                { "03","Ground" },
                { "03#","UPS Ground" },
                { "02","2DayAir" },
                { "12","3DaySelect" },
                { "14","NextDayAm" },
                { "07","International" },
                { "01","NextDayStd" },
                { "01#","Overnight" },
                { "08","International Express" }
            };

            return services.ToList();
        }
    }
}
