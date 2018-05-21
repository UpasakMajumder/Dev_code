using Kadena.Dto.General;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.BusinessLogic.Services;
using Kadena.MicroserviceClients.Contracts;
using Moq;
using Xunit;
using System.Threading.Tasks;
using System;
using Kadena.Models.Product;
using System.Collections.Generic;
using Kadena.Dto.TemplatedProduct.MicroserviceResponses;
using Kadena.Models.Membership;
using Kadena.Models.SiteSettings;
using Kadena.Dto.TemplatedProduct.MicroserviceRequests;

namespace Kadena.Tests.WebApi
{
    public class TemplateServiceTest : KadenaUnitTest<TemplateService>
    {
        private readonly string newName = "newName";
        private readonly int newQuantity = 5;
        private readonly Guid templateId = Guid.Empty;

        [Fact(DisplayName = "TemplateService.UpdateTemplate() | Success")]
        public async Task TemplateUpdateSucceed()
        {
            Setup<ITemplatedClient, Task<BaseResponseDto<bool?>>>(c => c.UpdateTemplate(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())
                , Task.FromResult(new BaseResponseDto<bool?>
                {
                    Success = true,
                    Payload = true
                }));

            var result = await Sut.UpdateTemplate(templateId, newName, newQuantity);

            Verify<IKenticoLogger>(c => c.LogError(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            Assert.True(result);
        }

        [Fact(DisplayName = "TemplateService.UpdateTemplate() | Failed")]
        public async Task TemplateUpdateFailed()
        {
            Setup<ITemplatedClient, Task<BaseResponseDto<bool?>>>(c => c.UpdateTemplate(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())
                , Task.FromResult(new BaseResponseDto<bool?>
                {
                    Success = false,
                    Payload = null,
                    ErrorMessages = "Some error."
                }));

            var result = await Sut.UpdateTemplate(templateId, newName, newQuantity);

            Verify<IKenticoLogger>(c => c.LogError(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            Assert.False(result);
        }

        [Fact(DisplayName = "TemplateService.GetTemplatesByProduct() | Product not templated")]
        public async Task GetTemplatesByProduct_ReturnsEmptyModel_WhenDocumentIsNotTemplatedProduct()
        {
            var invalidDocumentId = 0;
            Setup<IKenticoProductsProvider, Product>(srv => srv.GetProductByDocumentId(invalidDocumentId), new Product { ProductType = ProductTypes.StaticProduct });

            var templates = await Sut.GetTemplatesByProduct(invalidDocumentId);

            Assert.NotNull(templates);
            Assert.NotNull(templates.Data);
            Assert.NotNull(templates.Header);
            Assert.True(templates.Data.Length == 0);
            Assert.True(templates.Header.Length > 0);
        }

        [Fact(DisplayName = "TemplateService.GetTemplatesByProduct() | Product has templates")]
        public async Task GetTemplatesByProduct_ReturnsData_WhenDocumentHasTemplates()
        {
            var documentId = 10;

            Setup<ITemplatedClient, Task<BaseResponseDto<List<TemplateServiceDocumentResponse>>>>(srv => srv.GetTemplates(It.IsAny<int>(), It.IsAny<Guid>())
                , Task.FromResult(new BaseResponseDto<List<TemplateServiceDocumentResponse>>()
                {
                    Success = true,
                    Payload = new List<TemplateServiceDocumentResponse>
                        {
                            CreateSavedTemplate(),
                            CreateSavedTemplate()
                        }
                }));
            Setup<IKenticoUserProvider, User>(prv => prv.GetCurrentUser(), new User { });
            Setup<IKenticoProductsProvider, Product>(srv => srv.GetProductByDocumentId(documentId), new Product { ProductType = ProductTypes.TemplatedProduct });

            var templates = await Sut.GetTemplatesByProduct(documentId);

            Assert.Equal(2, templates.Data.Length);
        }

        [Fact(DisplayName = "TemplateService.GetTemplatesByProduct() | Get only saved templates")]
        public async Task GetTemplatesByProduct_ReturnsOnlySavedTemplates()
        {
            var documentId = 10;
            Setup<ITemplatedClient, Task<BaseResponseDto<List<TemplateServiceDocumentResponse>>>>(srv => srv.GetTemplates(It.IsAny<int>(), It.IsAny<Guid>())
                , Task.FromResult(new BaseResponseDto<List<TemplateServiceDocumentResponse>>()
                {
                    Success = true,
                    Payload = new List<TemplateServiceDocumentResponse>
                        {
                            CreateSavedTemplate(),
                            CreateSavedTemplate(),
                            CreateSavedTemplate(),
                            CreateWorkingCopyTemplate()
                        }
                }));
            Setup<IKenticoUserProvider, User>(prv => prv.GetCurrentUser(), new User { });
            Setup<IKenticoProductsProvider, Product>(srv => srv.GetProductByDocumentId(documentId), new Product { ProductType = ProductTypes.TemplatedProduct });

            var templates = await Sut.GetTemplatesByProduct(documentId);

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

        [Fact(DisplayName = "TemplateService.GetPreviewUri() | Fail")]
        public async Task GetPreviewUriFail()
        {
            Setup<ITemplatedClient, Task<BaseResponseDto<string>>>((cl) => cl.GetPreview(It.IsAny<Guid>(), It.IsAny<Guid>())
                , Task.FromResult(new BaseResponseDto<string>
                    {
                        Success = false,
                        Error = new BaseErrorDto
                        {
                            Message = "Some error"
                        }
                    }));

            var actualResult = await Sut.GetPreviewUri(Guid.Empty, Guid.Empty);

            Assert.Null(actualResult);
        }

        [Fact(DisplayName = "TemplateService.GetPreviewUri() | Success")]
        public async Task GetPreviewUriSuccess()
        {
            var expectedResult = new Uri("http://example.com");
            Setup<ITemplatedClient, Task<BaseResponseDto<string>>>((cl) => cl.GetPreview(It.IsAny<Guid>(), It.IsAny<Guid>())
                , Task.FromResult(new BaseResponseDto<string>
                    {
                        Success = true,
                        Payload = expectedResult.AbsoluteUri
                    }));

            var actualResult = await Sut.GetPreviewUri(Guid.Empty, Guid.Empty);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData("KDA.TemplatedProduct", "/editorurl?documentId=1230&templateId=newtemplateid&workspaceid=80208ba7-99ea-6f19-3b07-35428e89aa4c&use3d=False")]
        [InlineData("KDA.TemplatedProduct|KDA.MailingProduct", "/selectlisturl?url=%2feditorurl%3fdocumentId%3d1230%26templateId%3dnewtemplateid%26workspaceid%3d80208ba7-99ea-6f19-3b07-35428e89aa4c%26use3d%3dFalse")]
        public async Task TemplatedProductEditorUrl_TemplatedTest(string productType, string expectedResult)
        {
            const int documentId = 1230;
            const int userId = 45;
            const string masterTemplateId = "70208ba7-58ea-4f19-8b07-87628e89113b";
            const string workspaceId = "80208ba7-99ea-6f19-3b07-35428e89aa4c";
            const bool use3d = false;

            Setup<IKenticoResourceService, string>(r => r.GetSiteSettingsKey(Settings.KDA_Templating_ProductEditorUrl), "editorurl");
            Setup<IKenticoResourceService, string>(r => r.GetSiteSettingsKey(Settings.KDA_Templating_SelectListPageUrl), "selectlisturl");
            Setup<IKenticoDocumentProvider, string, bool, string>(d => d.GetDocumentUrl(It.IsAny<string>(), use3d), (s,b) => "/"+s);

            Setup<ITemplatedClient, Task<BaseResponseDto<string>>>(t => t.CreateNewTemplate(It.IsAny<NewTemplateRequestDto>()),
                Task.FromResult(new BaseResponseDto<string> { Success = true, Payload = "http://chili.com/template?q=1&v=2&doc=newtemplateid" }));

            var result = await Sut.TemplatedProductEditorUrl(documentId, 
                                                             userId,
                                                             productType, 
                                                             Guid.Parse(masterTemplateId),
                                                             Guid.Parse(workspaceId),
                                                             false);

            Assert.Equal(expectedResult, result);
        }
    }
}
