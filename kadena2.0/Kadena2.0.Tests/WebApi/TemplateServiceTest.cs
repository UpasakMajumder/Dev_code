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
                Mock.Of<IKenticoUserProvider>(),
                Mock.Of<IKenticoDocumentProvider>(),
                Mock.Of<IKenticoProductsProvider>()
                );
        }
        
        [Fact(DisplayName = "TemplateUpdateSucceed")]
        public async Task TemplateUpdateSucceed()
        {
            var templateClient = new Mock<ITemplatedClient>();
            templateClient
                .Setup(c => c.UpdateTemplate(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()))
                .Returns(Task.FromResult(new BaseResponseDto<bool?>
                {
                    Success = true,
                    Payload = true
                }));
            var logger = new Mock<IKenticoLogger>();
            var service = new TemplateService(Mock.Of<IKenticoResourceService>(),
                logger.Object, templateClient.Object,
                Mock.Of<IKenticoUserProvider>(),
                Mock.Of<IKenticoDocumentProvider>(),
                Mock.Of<IKenticoProductsProvider>());

            var result = await service.UpdateTemplate(templateId, newName, newQuantity);

            logger.Verify(c => c.LogError(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            Assert.True(result);
        }

        [Fact(DisplayName = "TemplateUpdateFailed")]
        public async Task TemplateUpdateFailed()
        {
            var templateClient = new Mock<ITemplatedClient>();
            templateClient
                .Setup(c => c.UpdateTemplate(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()))
                .Returns(Task.FromResult(new BaseResponseDto<bool?>
                {
                    Success = false,
                    Payload = null,
                    ErrorMessages = "Some error."
                }));
            var logger = new Mock<IKenticoLogger>();
            var service = new TemplateService(Mock.Of<IKenticoResourceService>(),
                logger.Object, templateClient.Object,
                Mock.Of<IKenticoUserProvider>(),
                Mock.Of<IKenticoDocumentProvider>(),
                Mock.Of<IKenticoProductsProvider>());

            var result = await service.UpdateTemplate(templateId, newName, newQuantity);

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
                Mock.Of<IKenticoUserProvider>(),
                Mock.Of<IKenticoDocumentProvider>(),
                Mock.Of<IKenticoProductsProvider>(srv => srv.GetProductByNodeId(invalidNodeId) == new Product { ProductType = ProductTypes.StaticProduct })
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
            var templatesResponse = new BaseResponseDto<List<TemplateServiceDocumentResponse>>()
            {
                Success = true,
                Payload = new List<TemplateServiceDocumentResponse>
                {
                    CreateSavedTemplate(),
                    CreateSavedTemplate()
                }
            };

            var sut = new TemplateService(
                Mock.Of<IKenticoResourceService>(),
                Mock.Of<IKenticoLogger>(),
                Mock.Of<ITemplatedClient>(srv => srv.GetTemplates(It.IsAny<int>(), It.IsAny<Guid>()) == Task.FromResult(templatesResponse)),
                Mock.Of<IKenticoUserProvider>(prv => prv.GetCurrentUser() == new User { }),
                Mock.Of<IKenticoDocumentProvider>(),
                Mock.Of<IKenticoProductsProvider>(srv => srv.GetProductByNodeId(nodeId) == new Product { ProductType = ProductTypes.TemplatedProduct })
            );

            var templates = await sut.GetTemplatesByProduct(nodeId);

            Assert.Equal(2, templates.Data.Length);
        }

        [Fact]
        public async Task GetTemplatesByProduct_ReturnsOnlySavedTemplates()
        {
            var nodeId = 10;
            var templatesResponse = new BaseResponseDto<List<TemplateServiceDocumentResponse>>()
            {
                Success = true,
                Payload = new List<TemplateServiceDocumentResponse>
                {
                    CreateSavedTemplate(),
                    CreateSavedTemplate(),
                    CreateSavedTemplate(),
                    CreateWorkingCopyTemplate()
                }
            };

            var sut = new TemplateService(
                Mock.Of<IKenticoResourceService>(),
                Mock.Of<IKenticoLogger>(),
                Mock.Of<ITemplatedClient>(srv => srv.GetTemplates(It.IsAny<int>(), It.IsAny<Guid>()) == Task.FromResult(templatesResponse)),
                Mock.Of<IKenticoUserProvider>(prv => prv.GetCurrentUser() == new User { }),
                Mock.Of<IKenticoDocumentProvider>(),
                Mock.Of<IKenticoProductsProvider>(srv => srv.GetProductByNodeId(nodeId) == new Product { ProductType = ProductTypes.TemplatedProduct })
            );

            var templates = await sut.GetTemplatesByProduct(nodeId);

            Assert.Equal(3, templates.Data.Length);
        }

        private TemplateServiceDocumentResponse CreateSavedTemplate()
        {
            var createdDatetime = DateTime.Now;
            var updatedDatetime = DateTime.Now.AddSeconds(10);

            return new TemplateServiceDocumentResponse
            {
                Created = createdDatetime,
                Updated = updatedDatetime,
                MetaData = new TemplateMetaData()
            };
        }

        private TemplateServiceDocumentResponse CreateWorkingCopyTemplate()
        {
            var createdDatetime = DateTime.Now;
            var updatedDatetime = createdDatetime;

            return new TemplateServiceDocumentResponse
            {
                Created = createdDatetime,
                Updated = updatedDatetime,
                MetaData = new TemplateMetaData()
            };
        }
    }
}
