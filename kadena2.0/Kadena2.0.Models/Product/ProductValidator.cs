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
            var weightNotRequired = ProductTypes.IsOfType(product.ProductType, ProductTypes.MailingProduct)
                && ProductTypes.IsOfType(product.ProductType, ProductTypes.TemplatedProduct);
            return !weightNotRequired;
        }
    }
}
