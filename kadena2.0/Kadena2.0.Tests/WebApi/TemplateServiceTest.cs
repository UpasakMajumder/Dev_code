using Kadena.Dto.General;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.BusinessLogic.Services;
using Kadena2.MicroserviceClients.Contracts;
using Moq;
using Xunit;
using System.Threading.Tasks;
using System;
using Kadena.Models;
using Kadena.Models.Product;
using System.Collections.Generic;
using Kadena.Dto.TemplatedProduct.MicroserviceResponses;

namespace Kadena.Tests.WebApi
{
    public class TemplateServiceTest
    {
        private readonly string newName = "newName";
        private readonly int newQuantity = 5;
        private readonly Guid templateId = Guid.Empty;

        private TemplateService Create(Mock<ITemplatedClient> templateClient)
        {
            return new TemplateService(
                Mock.Of<IKenticoResourceService>(),
                Mock.Of<IKenticoLogger>(),
                templateClient.Object,
                Mock.Of<IKenticoProviderService>(),
                Mock.Of<IKenticoUserProvider>(),
                Mock.Of<IKenticoDocumentProvider>(),
                Mock.Of<IShoppingCartProvider>()
                );
        }
        
        [Fact(DisplayName = "TemplateUpdateSucceed")]
        public async Task TemplateUpdateSucceed()
        {
            var templateClient = new Mock<ITemplatedClient>();
            templateClient.Setup(c => c.UpdateTemplate(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResponseDto<bool?>
                {
                    Success = true,
                    Payload = true
                }));
            var logger = new Mock<IKenticoLogger>();
            var service = new TemplateService(Mock.Of<IKenticoResourceService>(),
                logger.Object, templateClient.Object,
                Mock.Of<IKenticoProviderService>(),
                Mock.Of<IKenticoUserProvider>(),
                Mock.Of<IKenticoDocumentProvider>(),
                Mock.Of<IShoppingCartProvider>());

            var result = await service.UpdateTemplate(templateId, newName, newQuantity);

            templateClient.Verify(c => c.UpdateTemplate(templateId, newName, newQuantity), Times.Once);
            logger.Verify(c => c.LogError(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            Assert.True(result);
        }

        [Fact(DisplayName = "TemplateUpdateFailed")]
        public async Task TemplateUpdateFailed()
        {
            var templateClient = new Mock<ITemplatedClient>();
            templateClient.Setup(c => c.UpdateTemplate(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResponseDto<bool?>
                {
                    Success = false,
                    Payload = null,
                    ErrorMessages = "Some error."
                }));
            var logger = new Mock<IKenticoLogger>();
            var service = new TemplateService(Mock.Of<IKenticoResourceService>(),
                logger.Object, templateClient.Object,
                Mock.Of<IKenticoProviderService>(),
                Mock.Of<IKenticoUserProvider>(),
                Mock.Of<IKenticoDocumentProvider>(),
                Mock.Of<IShoppingCartProvider>());

            var result = await service.UpdateTemplate(templateId, newName, newQuantity);

            templateClient.Verify(c => c.UpdateTemplate(templateId, newName, newQuantity), Times.Once);
            logger.Verify(c => c.LogError(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            Assert.False(result);
        }

        [Fact]
        public async Task GetTemplatesByProduct_ReturnsEmptyModel_WhenDocumentIsNotTemplatedProduct()
        {
            var invalidNodeId = 0;

            var sut = new TemplateService(
                Mock.Of<IKenticoResourceService>(),
                Mock.Of<IKenticoLogger>(),
                Mock.Of<ITemplatedClient>(),
                Mock.Of<IKenticoProviderService>(),
                Mock.Of<IKenticoUserProvider>(),
                Mock.Of<IKenticoDocumentProvider>(),
                Mock.Of<IShoppingCartProvider>(srv => srv.GetProductByNodeId(invalidNodeId) == new Product { ProductType = ProductTypes.StaticProduct })
            );

            var templates = await sut.GetTemplatesByProduct(invalidNodeId);

            Assert.NotNull(templates);
            Assert.NotNull(templates.Data);
            Assert.NotNull(templates.Header);
            Assert.True(templates.Data.Length == 0);
            Assert.True(templates.Header.Length > 0);
        }

        [Fact]
        public async Task GetTemplatesByProduct_ReturnsData_WhenDocumentHasTemplates()
        {
            var nodeId = 10;
            var fakeDatetime = new DateTime().ToString();

            var templatesResponse = new BaseResponseDto<List<TemplateServiceDocumentResponse>>()
            {
                Success = true,
                Payload = new List<TemplateServiceDocumentResponse>
                {
                    new TemplateServiceDocumentResponse { Created = fakeDatetime, Updated = fakeDatetime },
                    new TemplateServiceDocumentResponse { Created = fakeDatetime, Updated = fakeDatetime }
                }
            };

            var sut = new TemplateService(
                Mock.Of<IKenticoResourceService>(),
                Mock.Of<IKenticoLogger>(),
                Mock.Of<ITemplatedClient>(srv => srv.GetTemplates(It.IsAny<int>(), It.IsAny<Guid>()) == Task.FromResult(templatesResponse)),
                Mock.Of<IKenticoProviderService>(),
                Mock.Of<IKenticoUserProvider>(prv => prv.GetCurrentUser() == new User { }),
                Mock.Of<IKenticoDocumentProvider>(),
                Mock.Of<IShoppingCartProvider>(srv => srv.GetProductByNodeId(nodeId) == new Product { ProductType = ProductTypes.TemplatedProduct })
            );

            var templates = await sut.GetTemplatesByProduct(nodeId);

            Assert.True(templates.Data.Length == 2);
        }
    }
}
