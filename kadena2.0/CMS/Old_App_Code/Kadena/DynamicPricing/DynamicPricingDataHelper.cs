using System.Collections.Generic;

namespace Kadena.Old_App_Code.Kadena.DynamicPricing
{
  public class DynamicPricingDataHelper
  {
    public bool ConvertDynamicPricingData(List<DynamicPricingRawData> rawData, out List<DynamicPricingData> cleanData)
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
  }
}