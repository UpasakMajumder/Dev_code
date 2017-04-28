
using CMS.FormEngine.Web.UI;
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

    private class DynamicPricingData
    {
      public int Min { get; set; }
      public int Max { get; set; }
      public decimal Price { get; set; }
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
      List<DynamicPricingData> data;
      if (!ConvertDynamicPricingData(rawData, out data))
      {
        return false;
      }
      return !IsDynamicPricingDataOverlap(data);
    }

    private bool ConvertDynamicPricingData(List<DynamicPricingRawData> rawData, out List<DynamicPricingData> cleanData)
    {
      cleanData = new List<DynamicPricingData>();

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
        cleanData.Add(new DynamicPricingData { Min = min, Max = max, Price = price });
      }
      return true;
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