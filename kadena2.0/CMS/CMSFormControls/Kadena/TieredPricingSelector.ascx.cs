using CMS.FormEngine.Web.UI;
using Kadena.Models;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Kadena.CMSFormControls.Kadena
{
    public partial class TieredPricingSelector : FormEngineUserControl
    {
        private class TieredPricingRawData
        {
            public string quantity { get; set; }
            public string price { get; set; }
        }

        public override object Value
        {
            get
            {
                return inpValue.Value;
            }
            set
            {
                inpValue.Value = System.Convert.ToString(value);
            }
        }

        public override bool IsValid()
        {
            var rawData = new JavaScriptSerializer().Deserialize<List<TieredPricingRawData>>(inpValue.Value);

            if (rawData == null || rawData.Count == 0)
            {
                return true;
            }

            var cleanData = new List<TieredPricingRange>();

            foreach (var rawItem in rawData)
            {
                if (!int.TryParse(rawItem.quantity, out int quantity))
                {
                    return false;
                }
                if (!decimal.TryParse(rawItem.price, out decimal price))
                {
                    return false;
                }

                cleanData.Add(new TieredPricingRange { Quantity = quantity, Price = price });
            }

            var errors = new List<string>();

            var valid = TieredPricingRange.ValidateRanges(cleanData, errors, true);

            if (!valid)
            {
                ValidationError = string.Join(". ", errors);
            }

            return valid;
        }
    }
}