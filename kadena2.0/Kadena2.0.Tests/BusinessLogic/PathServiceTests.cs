using Kadena.BusinessLogic.Services;
using Xunit;
using Moq;
using Kadena.AmazonFileSystemProvider;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Models.Common;
using System.Collections.Generic;

namespace Kadena.Tests.BusinessLogic
{
    public class PathServiceTests : KadenaUnitTest<PathService>
    {
        public PathServiceTests()
        {
            Setup<IKenticoCustomItemProvider, Environment>(s => s.GetItem<Environment>(0, It.IsAny<string>()), 
                new Environment { AmazonS3Folder = "dev" });
        }

        public static IEnumerable<object[]> GetDependencies()
        {
            yield return new object[] {
                null,
                new Mock<IKenticoResourceService>().Object,
                new Mock<IKenticoCustomItemProvider>().Object
            };
            yield return new object[] {
                new Mock<IS3PathService>().Object,
                null,
                new Mock<IKenticoCustomItemProvider>().Object
            };
            yield return new object[] {
                new Mock<IS3PathService>().Object,
                new Mock<IKenticoResourceService>().Object,
                null
            };
        }

        [Theory(DisplayName = "new PathService()")]
        [MemberData(nameof(GetDependencies))]
        public void ConstructorNull(IS3PathService pathService, IKenticoResourceService resourceService, IKenticoCustomItemProvider customItemProvider)
        {
            Assert.Throws<System.ArgumentNullException>(() => new PathService(pathService, resourceService, customItemProvider));
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
            Setup<IKenticoCustomItemProvider, Environment>(s => s.GetItem<Environment>(0, It.IsAny<string>()), null);

            System.Action action = () => Sut.GetObjectKeyFromPath("", false);

            Assert.Throws<System.NullReferenceException>(action);
        }

        [Fact(DisplayName = "PathService.GetObjectKeyFromPath() | Environment folder is empty")]
        public void GetObjectKeyFromPath_EnvironmentFolderEmpty()
        {
            var argument = "\\folder1\\folder2\\file.ext";
            var expectedResult = "media/folder1/folder2/file.ext";

            Setup<IS3PathService, string>(s => s.GetObjectKeyFromPath(argument, It.IsAny<bool>()), "folder1/folder2/file.ext");
            Setup<IKenticoCustomItemProvider, Environment>(s => s.GetItem<Environment>(0, It.IsAny<string>()), new Environment());

            var actualResult = Sut.GetObjectKeyFromPath(argument, false);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult, actualResult);
            Verify<IS3PathService>(s => s.GetObjectKeyFromPath(It.IsAny<string>(), It.IsAny<bool>()), Times.AtLeastOnce);
        }

        [Fact(DisplayName = "PathService.GetObjectKeyFromPath() | Key without special folder")]
        public void GetObjectKeyFromPath_NotInSpecialFolder()
        {
            var argument = "\\folder1\\folder2\\file.ext";
            var expectedResult = "dev/media/folder1/folder2/file.ext";

            Setup<IS3PathService, string>(s => s.GetObjectKeyFromPath(argument, It.IsAny<bool>()), "folder1/folder2/file.ext");

            var actualResult = Sut.GetObjectKeyFromPath(argument, false);

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
