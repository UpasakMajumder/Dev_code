using Kadena.BusinessLogic.Services;
using Xunit;
using Moq;
using Kadena.AmazonFileSystemProvider;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Models.Common;

namespace Kadena.Tests.BusinessLogic
{
    public class PathServiceTests : KadenaUnitTest<PathService>
    {
        public PathServiceTests()
        {
            Setup<IKenticoCustomItemProvider, Environment>(s => s.GetItem<Environment>(0, It.IsAny<string>()), 
                new Environment { AmazonS3Folder = "dev" });
        }

        [Fact(DisplayName = "PathService.CurrentDirectory")]
        public void CurrentDirectory()
        {
            var exc = Record.Exception(() => Sut.CurrentDirectory);

            Assert.Null(exc);
        }

        [Fact(DisplayName = "PathService.GetObjectKeyFromPath()")]
        public void GetObjectKeyFromPath()
        {
            var argument = "\\folder1\\folder2\\file.ext";
            var expectedResult = "dev/media/folder1/folder2/file.ext";

            Setup<IS3PathService, string>(s => s.GetObjectKeyFromPath(argument, It.IsAny<bool>()), "folder1/folder2/file.ext");

            var actualResult = Sut.GetObjectKeyFromPath(argument, false);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact(DisplayName = "PathService.GetPathFromObjectKey()")]
        public void GetPathFromObjectKey()
        {
            var expectedResult = "\\folder1\\folder2\\file.ext";
            var argument = "dev/media/folder1/folder2/file.ext";

            Setup<IS3PathService, string>(s => s.GetPathFromObjectKey("folder1/folder2/file.ext", It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()), expectedResult);

            var actualResult = Sut.GetPathFromObjectKey(argument, false, false, false);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult, actualResult);
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
        }

        [Fact(DisplayName = "PathService.EnsureFullKey()")]
        public void EnsureFullKey()
        {
            var argument = "folder1/folder2/file.ext";
            var expectedResult = "dev/media/folder1/folder2/file.ext";

            var actualResult = Sut.EnsureFullKey(argument);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult, actualResult);
        }
    }
}
