using Kadena.Dto.General;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.WebAPI.Services;
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
        private string _currentName = "currentName";

        private TemplateService Create(Mock<ITemplatedProductService> templateClient)
        {
            return new TemplateService(
                Mock.Of<IKenticoResourceService>(), 
                Mock.Of<IKenticoLogger>(),
                templateClient.Object,
                Mock.Of<IKenticoProviderService>(),
                Mock.Of<IKenticoUserProvider>()
                );
        }

        private BaseResponseDto<bool?> SetNameSuccess(string name)
        {
            _currentName = name;
            return new BaseResponseDto<bool?>
            {
                Success = true,
                Payload = true
            };
        }

        private BaseResponseDto<bool?> SetNameFailed()
        {
            return new BaseResponseDto<bool?>
            {
                Success = false,
                Payload = null,
                ErrorMessages = "Some error."
            };
        }

        [Fact(DisplayName = "SetNameSucceed")]
        public async Task SetNameSucceed()
        {
            var newName = "newName";
            Assert.NotEqual(newName, _currentName);

            var client = new Mock<ITemplatedProductService>();
            client.Setup(c => c.SetName(null, Guid.Empty, newName))
                .Returns(Task.FromResult(SetNameSuccess(newName)));
            var service = Create(client);

            var result = await service.SetName(Guid.Empty, newName);
            Assert.True(result);
            Assert.Equal(newName, _currentName);
        }

        [Fact(DisplayName = "SetNameFail")]
        public async Task SetNameFail()
        {
            var newName = "newName";

            var client = new Mock<ITemplatedProductService>();
            client.Setup(c => c.SetName(null, Guid.Empty, newName))
                .Returns(Task.FromResult(SetNameFailed()));
            var service = Create(client);
            var result = await service.SetName(Guid.Empty, newName);

            Assert.False(result);
        }

        [Fact]
        public async Task GetTemplatesByProduct_ReturnsEmptyModel_WhenDocumentIsNotTemplatedProduct()
        {
            var invalidDocumentId = 0;

            var sut = new TemplateService(
                Mock.Of<IKenticoResourceService>(),
                Mock.Of<IKenticoLogger>(),
                Mock.Of<ITemplatedProductService>(),
                Mock.Of<IKenticoProviderService>(srv => srv.GetProductByDocumentId(invalidDocumentId) == new Product { ProductType = ProductTypes.StaticProduct }),
                Mock.Of<IKenticoUserProvider>()
            );

            var templates = await sut.GetTemplatesByProduct(invalidDocumentId);

            Assert.NotNull(templates);
            Assert.NotNull(templates.Data);
            Assert.NotNull(templates.Header);
            Assert.True(templates.Data.Length == 0);
            Assert.True(templates.Header.Length > 0);
        }

        [Fact]
        public async Task GetTemplatesByProduct_ReturnsData_WhenDocumentHasTemplates()
        {
            var documentId = 10;
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
                Mock.Of<ITemplatedProductService>(srv => srv.GetTemplates(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<Guid>()) == Task.FromResult(templatesResponse)),
                Mock.Of<IKenticoProviderService>(srv => srv.GetProductByDocumentId(documentId) == new Product { ProductType = ProductTypes.TemplatedProduct }),
                Mock.Of<IKenticoUserProvider>(prv => prv.GetCurrentUser() == new User { })
            );

            var templates = await sut.GetTemplatesByProduct(documentId);

            Assert.True(templates.Data.Length == 2);
        }
    }
}
