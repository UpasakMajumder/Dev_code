using CMS.DocumentEngine;
using Kadena.Models.Product;
using Kadena.Old_App_Code.EventHandlers;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.WebAPI.KenticoProviders.Classes;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Kadena.Tests.EventHandlers
{
    public class ProductEventHandlerTests : KadenaUnitTest<ProductEventHandler>
    {
        [Fact(DisplayName = "ProductEventHandler.CopyProductSKUFieldsToSKU_EventHandler()")]
        public void CopyProductSKUFieldsToSKU_ShouldDoNothing_WhenNodeIsNotProduct()
        {
            var sut = Sut;

            sut.CopyProductSKUFieldsToSKU_EventHandler(sut, new DocumentEventArgs());

            Verify<IKenticoSkuProvider>(p => p.UpdateSkuMandatoryFields(It.IsAny<Sku>()), Times.Never);
        }

        public static IEnumerable<object[]> GetProducts() => new[]
        {
            new object []
            {
                new ProductClass
                {
                    NodeSKUID = 1,
                    SKUNeedsShipping = true,
                    SKUWeight = 2
                }
            },
            new object []
            {
                new CampaignsProductClass
                {
                    NodeSKUID = 3,
                    SKUWeight = 4
                }
            },
        };

        [Theory(DisplayName = "ProductEventHandlerFake.CopyProductSKUFieldsToSKU_EventHandler()")]
        [MemberData(nameof(GetProducts))]
        public void CopyProductSKUFieldsToSKU_ShouldCopyValues_WhenNodeIsProduct(IProductClass product)
        {
            var sut = new ProductEventHandlerFake() { Product = product };
            var skuProviderMock = new Mock<IKenticoSkuProvider>();
            sut.SkuProvider = skuProviderMock.Object;

            sut.CopyProductSKUFieldsToSKU_EventHandler(sut, new DocumentEventArgs());

            skuProviderMock.Verify(p => p.UpdateSkuMandatoryFields(It.Is<Sku>(s
                => s.SkuId == product.NodeSKUID
                    && s.NeedsShipping == product.SKUNeedsShipping
                    && s.Weight == product.SKUWeight)));
        }

        [Fact(DisplayName = "ProductEventHandlerFake.CopyProductSKUFieldsToSKU_EventHandler()")]
        public void CopyProductSKUFieldsToSKU_ShouldLogException_WhenProductProviderFails()
        {
            var sut = new ProductEventHandlerFake() { Product = new ProductClass() };
            var skuProviderStub = new Mock<IKenticoSkuProvider>();
            skuProviderStub.Setup(p => p.UpdateSkuMandatoryFields(It.IsAny<Sku>())).Throws(new Exception());
            sut.SkuProvider = skuProviderStub.Object;
            var loggerMock = new Mock<IKenticoLogger>();
            sut.Logger = loggerMock.Object;

            sut.CopyProductSKUFieldsToSKU_EventHandler(sut, new DocumentEventArgs());

            loggerMock.Verify(l => l.LogException(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once());
        }

        private class ProductEventHandlerFake : ProductEventHandler
        {
            public IProductClass Product { get; set; }

            protected override IProductClass GetProductFromNode(TreeNode node)
            {
                return Product;
            }

            protected override IProductClass GetCampaignsProductFromNode(TreeNode node)
            {
                return Product;
            }
        }
    }
}