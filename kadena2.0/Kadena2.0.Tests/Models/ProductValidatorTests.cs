using Kadena.Models.Product;
using System.Linq;
using Xunit;

namespace Kadena.Tests.Models
{
    public class ProductValidatorTests
    {
        private Product GetProductRequiringWeight()
        {
            return new Product
            {
                ProductType = ProductTypes.InventoryProduct
            };
        }

        private Product GetProductNotRequiringWeight()
        {
            return new Product
            {
                ProductType = ProductTypes.Combine(ProductTypes.MailingProduct, ProductTypes.TemplatedProduct)
            };
        }

        [Theory]
        [InlineData(0)]
        [InlineData(5)]
        public void ValidateWeight_ShouldBeTrue_WhenProductTypeDoesntRequireWeight(double weight)
        {
            var product = GetProductNotRequiringWeight();
            product.Weight = weight;

            var isValid = new ProductValidator().ValidateWeight(product);

            Assert.True(isValid);
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(5, true)]
        public void ValidateWeight_ShouldValidateCorrectly_WhenProductTypeRequiresWeight(double weight, bool expected)
        {
            var product = GetProductRequiringWeight();
            product.Weight = weight;

            var actual = new ProductValidator().ValidateWeight(product);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsSKUWeightRequired_ShouldBeTrue_WhenRequired()
        {
            var product = GetProductRequiringWeight();

            var isRequired = new ProductValidator().IsSKUWeightRequired(product);

            Assert.True(isRequired);
        }

        [Fact]
        public void IsSKUWeightRequired_ShouldBeFalse_WhenNotRequired()
        {
            var product = GetProductNotRequiringWeight();

            var isRequired = new ProductValidator().IsSKUWeightRequired(product);

            Assert.False(isRequired);
        }
    }
}
