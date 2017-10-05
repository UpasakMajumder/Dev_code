using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Kadena.Dto.Attributes
{
    public class StringListValidationAttribute : ValidationAttribute
    {
        private readonly int minElements;
        private readonly int maxElements;
        private readonly int minimalElementLength;
        private readonly int maximalElementLength;

        public StringListValidationAttribute(int minElements, int maxElements, int minimalElementLength, int maximalElementLength)
        {
            this.minElements = minElements;
            this.maxElements = maxElements;
            this.minimalElementLength = minimalElementLength;
            this.maximalElementLength = maximalElementLength;
        }

        public override bool IsValid(object value)
        {
            var list = value as IList<string>;

            if (list != null)
            {
                return list.Count >= minElements &&
                       list.Count <= maxElements &&
                       list.All(e => (e?.Length ?? 0) >= minimalElementLength &&
                                     (e?.Length ?? 0) <= maximalElementLength);
            }
            else if (minElements == 0)
            {
                return true;
            }

            return false;
        }
    }
}