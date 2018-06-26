using Kadena.BusinessLogic.Services;
using Xunit;
using Moq;
using Kadena.AmazonFileSystemProvider;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System.Collections.Generic;
using Kadena2.MicroserviceClients.Contracts;
using Kadena.Dto.General;
using System.Threading.Tasks;
using Kadena2.MicroserviceClients;
using System;

namespace Kadena.Tests.BusinessLogic
{
    public class PathServiceTests : KadenaUnitTest<PathService>
    {
        public PathServiceTests()
        {
            Setup<IKenticoCustomItemProvider, Kadena.Models.Common.Environment>(s => s.GetItem<Kadena.Models.Common.Environment>(0, It.IsAny<string>()),
                new Kadena.Models.Common.Environment { AmazonS3Folder = "dev" });
        }
        
        [Theory(DisplayName = "PathService()")]
        [ClassData(typeof(PathServiceTests))]
        public void ConstructorNull(IS3PathService pathService, IKenticoResourceService resourceService, IKenticoCustomItemProvider customItemProvider,
            IFileClient fileClient, IKenticoSiteProvider siteProvider)
        {
            Assert.Throws<System.ArgumentNullException>(() => new PathService(pathService, resourceService, customItemProvider, fileClient, siteProvider));
        }

        [Fact(DisplayName = "PathService.CurrentDirectory")]
        public void CurrentDirectory()
        {
            var exc = Record.Exception(() => Sut.CurrentDirectory);

            Assert.Null(exc);
        }

        [Fact(DisplayName = "PathService.GetObjectKeyFromPath() | Environment setting not found")]
        public void GetObjectKeyFromPath_EnvironmentNotFound()
        {
            Setup<IKenticoCustomItemProvider, Kadena.Models.Common.Environment>(s => s.GetItem<Kadena.Models.Common.Environment>(0, It.IsAny<string>()), null);

            System.Action action = () => Sut.GetObjectKeyFromPath("", false);

            Assert.Throws<System.NullReferenceException>(action);
        }

        [Fact(DisplayName = "PathService.GetObjectKeyFromPath() | Environment folder is empty")]
        public void GetObjectKeyFromPath_EnvironmentFolderEmpty()
        {
            var argument = "\\folder1\\folder2\\file.ext";
            var expectedResult = "media/folder1/folder2/file.ext";

            Setup<IS3PathService, string>(s => s.GetObjectKeyFromPath(argument, It.IsAny<bool>()), "folder1/folder2/file.ext");
            Setup<IKenticoCustomItemProvider, Kadena.Models.Common.Environment>(
                s => s.GetItem<Kadena.Models.Common.Environment>(0, It.IsAny<string>()), 
                new Kadena.Models.Common.Environment());

            var actualResult = Sut.GetObjectKeyFromPath(argument, false);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult, actualResult);
            Verify<IS3PathService>(s => s.GetObjectKeyFromPath(It.IsAny<string>(), It.IsAny<bool>()), Times.AtLeastOnce);
        }

        [Theory(DisplayName = "PathService.GetObjectKeyFromPath() | Key without special folder")]
        [InlineData("folder1/folder2/file.ext", "dev/media/folder1/folder2/file.ext")]
        [InlineData("folder1/file.ext", "dev/media/folder1/file.ext")]
        public void GetObjectKeyFromPath_NotInSpecialFolder(string path, string expectedResult)
        {
            Setup<IS3PathService, string, bool, string>(s => s.GetObjectKeyFromPath(path, It.IsAny<bool>()), (p, l) => p);

            var actualResult = Sut.GetObjectKeyFromPath(path, false);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult, actualResult);
            Verify<IS3PathService>(s => s.GetObjectKeyFromPath(It.IsAny<string>(), It.IsAny<bool>()), Times.AtLeastOnce);
        }

        [Fact(DisplayName = "PathService.GetObjectKeyFromPath() | Key with special folder")]
        public void GetObjectKeyFromPath_InSpecialFolder()
        {
            var argument = "\\folder1\\folder2\\file.ext";
            var expectedResult = "dev/media/folder1/folder2/file.ext";

            Setup<IS3PathService, string>(s => s.GetObjectKeyFromPath(argument, It.IsAny<bool>()), "dev/media/folder1/folder2/file.ext");

            var actualResult = Sut.GetObjectKeyFromPath(argument, false);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult, actualResult);
            Verify<IS3PathService>(s => s.GetObjectKeyFromPath(It.IsAny<string>(), It.IsAny<bool>()), Times.AtLeastOnce);
        }

        [Fact(DisplayName = "PathService.GetObjectKeyFromPath() | System key generated")]
        public void GetObjectKeyFromPath_SystemKeyGenerated()
        {
            var argument = $"{FileSystem.Mailing.SystemFolder}/subfolder1/file.ext1.ext";
            var expectedResult = "generated path";

            Setup<IS3PathService, string>(s => s.GetObjectKeyFromPath(argument, It.IsAny<bool>()), argument);
            Setup<IFileClient, Task<BaseResponseDto<string>>>(
                s => s.GetFileKey(It.IsAny<FileSystem>(), FileType.Original, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                Task.FromResult(new BaseResponseDto<string> { Success = true, Payload = expectedResult }));

            var actualResult = Sut.GetObjectKeyFromPath(argument, false);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult, actualResult);
            Verify<IFileClient>(s => s.GetFileKey(FileSystem.Mailing, FileType.Original, It.IsAny<string>(), "file.ext1", "ext"), Times.Once);
        }

        [Fact(DisplayName = "PathService.GetObjectKeyFromPath() | System key isn't generated")]
        public void GetObjectKeyFromPath_SystemKeyNoGenerated()
        {
            var argument = $"{FileSystem.Mailing.SystemFolder}/subfolder1/file.ext";
            var expectedResult = "generated path";

            Setup<IS3PathService, string>(s => s.GetObjectKeyFromPath(argument, It.IsAny<bool>()), argument);
            Setup<IFileClient, Task<BaseResponseDto<string>>>(
                s => s.GetFileKey(It.IsAny<FileSystem>(), FileType.Original, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                Task.FromResult(new BaseResponseDto<string> { Success = false, Payload = expectedResult }));

            Action action = () => Sut.GetObjectKeyFromPath(argument, false);

            Assert.Throws<InvalidOperationException>(action);
            Verify<IFileClient>(s => s.GetFileKey(It.IsAny<FileSystem>(), FileType.Original, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact(DisplayName = "PathService.GetPathFromObjectKey() | Success")]
        public void GetPathFromObjectKey()
        {
            var expectedResult = "\\folder1\\folder2\\file.ext";
            var argument = "dev/media/folder1/folder2/file.ext";

            Setup<IS3PathService, string>(s => s.GetPathFromObjectKey("folder1/folder2/file.ext", It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()), expectedResult);

            var actualResult = Sut.GetPathFromObjectKey(argument, false, false, false);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult, actualResult);
            Verify<IS3PathService>(s => s.GetPathFromObjectKey(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()), Times.AtLeastOnce);
        }

        [Fact(DisplayName = "PathService.GetPathFromObjectKey() | Null path")]
        public void GetPathFromObjectKey_Null()
        {
            var actualResult = Sut.GetPathFromObjectKey(null, false, false, false);

            Assert.Null(actualResult);
        }

        [Theory(DisplayName = "PathService.GetValidPath()")]
        [InlineData("", true)]
        [InlineData("asodas", true)]
        [InlineData(null, true)]
        [InlineData("", false)]
        [InlineData("asodas", false)]
        [InlineData(null, false)]
        public void GetValidPath(string path, bool lower)
        {
            var exc = Record.Exception(() => Sut.GetValidPath(path, lower));

            Assert.Null(exc);
            Verify<IS3PathService>(s => s.GetValidPath(It.IsAny<string>(), It.IsAny<bool>()), Times.AtLeastOnce);
        }
    }
}
