using System.Linq;

namespace Kadena.Models.Product
{
    public class ProductValidator
    {
        public bool ValidateWeight(Product product)
        {
            var weightIsRequired = IsSKUWeightRequired(product);
            if (weightIsRequired)
            {
                var isValid = product.Weight > 0;
                return isValid;
            }

            return true;
        }

        public bool IsSKUWeightRequired(Product product)
        {
            var weightRequiredFor = Product.GetProductTypesRequiringWeight();
            var weightIsRequired = weightRequiredFor.Any(pt => ProductTypes.IsOfType(product.ProductType, pt));
            return weightIsRequired;
        }
    }
}
