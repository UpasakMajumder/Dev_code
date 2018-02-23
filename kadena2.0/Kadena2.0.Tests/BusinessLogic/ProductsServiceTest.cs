using Kadena.BusinessLogic.Services;
using Kadena.Models.Product;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using Xunit;

namespace Kadena.Tests.BusinessLogic
{
    public class ProductsServiceTest
    {
        public static IEnumerable<object[]> GetEmptyOptions()
        {
            yield return new object[]
            {
                new Dictionary<string, int> ()
            };
            yield return new object[]
            {
                new Dictionary<string, int> {
                    { "Option", 0 }
                }
            };
            yield return new object[]
            {
                null
            };
        }

        public static IEnumerable<object[]> GetOptions()
        {
            yield return new object[]
            {
                new Dictionary<string, int> {
                    { "Option", 0 }
                }
            };
        }

        [Theory(DisplayName = "ProductsService.GetPrice()")]
        [MemberData(nameof(GetEmptyOptions))]
        [MemberData(nameof(GetOptions))]
        public void GetPrice(Dictionary<string, int> options)
        {
            var autoMocker = new AutoMocker();
            var service = autoMocker.CreateInstance<ProductsService>();
            var productsService = autoMocker.GetMock<IKenticoProductsProvider>();
            productsService.Setup(p => p.GetVariant(It.IsAny<int>(), It.IsAny<IEnumerable<int>>()))
                .Returns(new Sku());

            var result = service.GetPrice(0, options);

            Assert.Null(result);
        }

        [Theory(DisplayName = "ProductsService.GetPrice() | Variant not found")]
        [MemberData(nameof(GetOptions))]
        public void GetPrice_NonExistingVariant(Dictionary<string, int> options)
        {
            var autoMocker = new AutoMocker();
            var service = autoMocker.CreateInstance<ProductsService>();

            Assert.Throws<ArgumentException>(() => service.GetPrice(0, options));
        }
    }
}
