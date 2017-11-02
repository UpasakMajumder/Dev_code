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
    public class ProductEventHandlerTests
    {
        [Fact]
        public void CopyProductSKUFieldsToSKU_ShouldDoNothing_WhenNodeIsNotProduct()
        {
            var productProviderMock = new Mock<IKenticoProductsProvider>();
            var sut = new ProductEventHandler();
            sut.ProductsProvider = productProviderMock.Object;

            sut.CopyProductSKUFieldsToSKU_EventHandler(sut, new DocumentEventArgs());

            productProviderMock.Verify(p => p.UpdateSku(It.IsAny<Sku>()), Times.Never());
        }

        [Fact]
        public void CopyProductSKUFieldsToSKU_ShouldCopyValues_WhenNodeIsProduct()
        {
            var product = new ProductClass
            {
                NodeSKUID = 1,
                SKUNeedsShipping = true,
                SKUWeight = 2
            };
            var sut = new ProductEventHandlerFake() { Product = product };
            var productProviderMock = new Mock<IKenticoProductsProvider>();
            sut.ProductsProvider = productProviderMock.Object;

            sut.CopyProductSKUFieldsToSKU_EventHandler(sut, new DocumentEventArgs());

            productProviderMock.Verify(p => p.UpdateSku(It.Is<Sku>(s
                => s.SkuId == product.NodeSKUID
                    && s.NeedsShipping == product.SKUNeedsShipping
                    && s.Weight == product.SKUWeight)));
        }

        [Fact]
        public void CopyProductSKUFieldsToSKU_ShouldLogException_WhenProductProviderFails()
        {
            var sut = new ProductEventHandlerFake() { Product = new ProductClass() };
            var productProviderStub = new Mock<IKenticoProductsProvider>();
            productProviderStub.Setup(p => p.UpdateSku(It.IsAny<Sku>())).Throws(new Exception());
            sut.ProductsProvider = productProviderStub.Object;
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