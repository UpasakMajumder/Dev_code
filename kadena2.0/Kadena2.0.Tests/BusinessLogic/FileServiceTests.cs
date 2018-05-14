using Kadena.BusinessLogic.Services;
using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using Kadena2.MicroserviceClients.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.AmazonFileSystemProvider;

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
            };
            yield return new object[]
            {
                new Mock<IFileClient>().Object,
                null,
                new Mock<IKenticoLogger>().Object,
                new Mock<IS3PathService>().Object,
                new Mock<IKenticoFileProvider>().Object,
            };
            yield return new object[]
            {
                new Mock<IFileClient>().Object,
                new Mock<IKenticoResourceService>().Object,
                null,
                new Mock<IS3PathService>().Object,
                new Mock<IKenticoFileProvider>().Object,
            };
            yield return new object[]
            {
                new Mock<IFileClient>().Object,
                new Mock<IKenticoResourceService>().Object,
                new Mock<IKenticoLogger>().Object,
                null,
                new Mock<IKenticoFileProvider>().Object,
            };
            yield return new object[]
            {
                new Mock<IFileClient>().Object,
                new Mock<IKenticoResourceService>().Object,
                new Mock<IKenticoLogger>().Object,
                new Mock<IS3PathService>().Object,
                null,
            };
        }

        [Theory(DisplayName = "FileService()")]
        [MemberData(nameof(GetDependencies))]
        public void FileService(IFileClient fileClient, IKenticoResourceService resources, IKenticoLogger logger, IS3PathService pathService, IKenticoFileProvider fileProvider)
        {
            Assert.Throws<ArgumentNullException>(() => new FileService(fileClient, resources, logger, pathService, fileProvider));
        }

        [Fact]
        public void CreateMailing()
        {
            var exc = Record.Exception(() => Sut.CreateMailingList("", null));

            Assert.Null(exc);
        }
    }
}
