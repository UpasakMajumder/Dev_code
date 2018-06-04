using System.Collections.Generic;
using System.Linq;

namespace Kadena.Models
{
    public class DynamicPricingRange
    {
        public int MinVal { get; set; }
        public int MaxVal { get; set; }
        public decimal Price { get; set; }

        public bool Validate(List<string> errors)
        {
            bool isValid = true;
            if (MinVal < 0 || MaxVal < 0)
            {
                isValid = false;
                errors.Add("Min and Max value must be >= 0");
            }
            if (MinVal > MaxVal)
            {
                isValid = false;
                errors.Add("Min must be <= Max");
            }
            return isValid;
        }   

        public static bool ValidateRanges(List<DynamicPricingRange> ranges, List<string> errors, bool allowEmptyRanges = false, bool requireAscendentOrder = false)
        {
            if ((ranges == null || ranges.Count == 0) && !allowEmptyRanges)
            {
                errors.Add("No Dynamic pricing ranges specified");
                return false;
            }

            if (!requireAscendentOrder)
            {
                ranges = ranges.OrderBy(x => x.MinVal).ToList();
            }

            bool isValid = true;

            for (int i = 0; i < ranges.Count; i++)
            {
                if (!ranges[i].Validate(errors))
                {
                    return false;
                }

                if (i < ranges.Count - 1)
                {
                    if (ranges[i].MaxVal >= ranges[i + 1].MinVal)
                    {
                        errors.Add($"Specified range intervals interfere between themselves (intrerval #{i})");
                        return false;
                    }
                }
            }

            return isValid;
        }
    }    
}