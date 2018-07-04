using CMS.DocumentEngine;
using Kadena.Models.Product;
using Kadena.Old_App_Code.EventHandlers;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.WebAPI.KenticoProviders.Classes;
using Moq;
using System;
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

        [Fact(DisplayName = "ProductEventHandlerFake.CopyProductSKUFieldsToSKU_EventHandler()")]
        public void CopyProductSKUFieldsToSKU_ShouldCopyValues_WhenNodeIsProduct()
        {
            var product = new ProductClass
            {
                NodeSKUID = 1,
                SKUNeedsShipping = true,
                SKUWeight = 2
            };
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
            public ProductClass Product { get; set; }

            protected override ProductClass GetProductFromNode(TreeNode node)
            {
                return Product;
            }
        }
    }
}