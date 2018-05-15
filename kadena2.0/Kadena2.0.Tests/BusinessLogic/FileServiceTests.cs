using Kadena.BusinessLogic.Services;
using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using Kadena2.MicroserviceClients.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.AmazonFileSystemProvider;
using System.Threading.Tasks;
using Kadena.Dto.General;

namespace Kadena.Tests.BusinessLogic
{
    public class FileServiceTests : KadenaUnitTest<FileService>
    {
        public static IEnumerable<object[]> GetDependencies()
        {
            yield return new object[]
            {
                null,
                new Mock<IKenticoResourceService>().Object,
                new Mock<IKenticoLogger>().Object,
                new Mock<IS3PathService>().Object,
                new Mock<IKenticoFileProvider>().Object,
                new Mock<IExportClient>().Object,
                new Mock<IKenticoSiteProvider>().Object,
            };
            yield return new object[]
            {
                new Mock<IFileClient>().Object,
                null,
                new Mock<IKenticoLogger>().Object,
                new Mock<IS3PathService>().Object,
                new Mock<IKenticoFileProvider>().Object,
                new Mock<IExportClient>().Object,
                new Mock<IKenticoSiteProvider>().Object,
            };
            yield return new object[]
            {
                new Mock<IFileClient>().Object,
                new Mock<IKenticoResourceService>().Object,
                null,
                new Mock<IS3PathService>().Object,
                new Mock<IKenticoFileProvider>().Object,
                new Mock<IExportClient>().Object,
                new Mock<IKenticoSiteProvider>().Object,
            };
            yield return new object[]
            {
                new Mock<IFileClient>().Object,
                new Mock<IKenticoResourceService>().Object,
                new Mock<IKenticoLogger>().Object,
                null,
                new Mock<IKenticoFileProvider>().Object,
                new Mock<IExportClient>().Object,
                new Mock<IKenticoSiteProvider>().Object,
            };
            yield return new object[]
            {
                new Mock<IFileClient>().Object,
                new Mock<IKenticoResourceService>().Object,
                new Mock<IKenticoLogger>().Object,
                new Mock<IS3PathService>().Object,
                null,
                new Mock<IExportClient>().Object,
                new Mock<IKenticoSiteProvider>().Object,
            };
            yield return new object[]
            {
                new Mock<IFileClient>().Object,
                new Mock<IKenticoResourceService>().Object,
                new Mock<IKenticoLogger>().Object,
                new Mock<IS3PathService>().Object,
                new Mock<IKenticoFileProvider>().Object,
                null,
                new Mock<IKenticoSiteProvider>().Object,
            };
            yield return new object[]
            {
                new Mock<IFileClient>().Object,
                new Mock<IKenticoResourceService>().Object,
                new Mock<IKenticoLogger>().Object,
                new Mock<IS3PathService>().Object,
                new Mock<IKenticoFileProvider>().Object,
                new Mock<IExportClient>().Object,
                null,
            };
        }

        [Theory(DisplayName = "FileService()")]
        [MemberData(nameof(GetDependencies))]
        public void FileService(IFileClient fileClient, IKenticoResourceService resources, IKenticoLogger logger, IS3PathService pathService,
            IKenticoFileProvider fileProvider, IExportClient exportClient, IKenticoSiteProvider siteProvider)
        {
            Assert.Throws<ArgumentNullException>(() => new FileService(fileClient, resources, logger, pathService,
                fileProvider, exportClient, siteProvider));
        }

        [Fact(DisplayName = "FileService.CreateMailingList()")]
        public void CreateMailing()
        {
            var exc = Record.Exception(() => Sut.CreateMailingList("", null));

            Assert.Null(exc);
        }

        [Theory(DisplayName = "FileService.GetContainerFileUrl() | Success")]
        [InlineData("https://example.com")]
        [InlineData("/relativeurl")]
        [InlineData("")]
        public async Task GetContainerFileUrl_Success(string url)
        {
            Setup<IExportClient, Task<BaseResponseDto<string>>>(s => s.ExportMailingList(It.IsAny<Guid>()),
                Task.FromResult(new BaseResponseDto<string> { Success = true, Payload = url }));

            var actualResult = await Sut.GetContainerFileUrl(Guid.Empty);

            Assert.NotNull(actualResult);
        }

        [Fact(DisplayName = "FileService.GetContainerFileUrl() | Success null value")]
        public async Task GetContainerFileUrl_SuccessNullValue()
        {
            Setup<IExportClient, Task<BaseResponseDto<string>>>(s => s.ExportMailingList(It.IsAny<Guid>()),
                Task.FromResult(new BaseResponseDto<string> { Success = true, Payload = null }));

            Task action() => Sut.GetContainerFileUrl(Guid.Empty);

            await Assert.ThrowsAsync<ArgumentNullException>(action);
        }

        [Fact(DisplayName = "FileService.GetContainerFileUrl() | Failed")]
        public async Task GetContainerFileUrl_Failed()
        {
            Setup<IExportClient, Task<BaseResponseDto<string>>>(s => s.ExportMailingList(It.IsAny<Guid>()),
                Task.FromResult(new BaseResponseDto<string> { Success = false }));

            var actualResult = await Sut.GetContainerFileUrl(Guid.Empty);

            Assert.Null(actualResult);
        }
    }
}
