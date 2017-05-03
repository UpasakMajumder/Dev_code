
using CMS.FormEngine.Web.UI;
using Kadena.Old_App_Code.Kadena.DynamicPricing;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Kadena.CMSFormControls.Kadena
{
  public partial class DynamicPricingSelector : FormEngineUserControl
  {
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
      List<DynamicPricingData> data;
      if (!new DynamicPricingDataHelper().ConvertDynamicPricingData(rawData, out data))
      {
        return false;
      }
      return !IsDynamicPricingDataOverlap(data);
    }

    private bool IsDynamicPricingDataOverlap(List<DynamicPricingData> data)
    {
      foreach (var item1 in data)
      {
        foreach (var item2 in data)
        {
          if (item1.GetHashCode() != item2.GetHashCode())
          {
            if (item1.Min < item2.Max && item2.Min < item1.Max)
            {
              return true;
            }
          }         
        }
      }
      return false;
    }
  }
}