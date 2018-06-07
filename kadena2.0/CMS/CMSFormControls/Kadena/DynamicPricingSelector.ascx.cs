using CMS.FormEngine.Web.UI;
using Kadena.Models;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Kadena.CMSFormControls.Kadena
{
  public partial class DynamicPricingSelector : FormEngineUserControl
  {
    private class DynamicPricingRawData
    {
        public string minVal { get; set; }
        public string maxVal { get; set; }
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
      var rawData = new JavaScriptSerializer().Deserialize<List<DynamicPricingRawData>>(inpValue.Value);

      if (rawData == null || rawData.Count == 0)
      {
        return true;
      }
      
      var cleanData = new List<DynamicPricingRange>();

        foreach (var rawItem in rawData)
        {
            int min, max;
            decimal price;

            if (!int.TryParse(rawItem.minVal, out min))
            {
                return false;
            }
            if (!int.TryParse(rawItem.maxVal, out max))
            {
                return false;
            }
            if (!decimal.TryParse(rawItem.price, out price))
            {
                return false;
            }

            if (min > max)
            {
                return false;
            }

            if (price < 0)
            {
                return false;
            }

            cleanData.Add(new DynamicPricingRange { MinVal = min, MaxVal = max, Price = price });
        }

        var errors = new List<string>();

        var valid = DynamicPricingRange.ValidateRanges(cleanData, errors, true);

        if (!valid)
        {
             ValidationError = string.Join(". ", errors);
        }

        return valid;
    }
  }
}