using Xunit;
using Moq;
using Kadena.BusinessLogic.Services;
using Kadena.WebAPI.KenticoProviders.Contracts;

namespace Kadena.Tests.BusinessLogic
{
    public class ImageServiceTests : KadenaUnitTest<ImageService>
    {
        [Theory(DisplayName = "ImageService.GetThumbnailLink() | Empty parameter")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void GetThumbnailLink_EmptyParameter(string parameter)
        {
            var expectedResult = string.Empty;

            var actualResult = Sut.GetThumbnailLink(parameter);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact(DisplayName = "ImageService.GetThumbnailLink() | External image")]
        public void GetThumbnailLink_ExternalImage()
        {
            var expectedResult = @"http://example.com/image.jpg";
            var originalImageLink = expectedResult;
            Setup<IKenticoSiteProvider, string>(s => s.GetFullUrl(), @"http://otherexample.com");

            var actualResult = Sut.GetThumbnailLink(originalImageLink);

            Assert.Equal(expectedResult, actualResult);
        }

        [Theory(DisplayName = "ImageService.GetThumbnailLink() | S3 image")]
        [InlineData(@"http://example.com/api/file/get?path=kda/media/images/image.jpg")]
        [InlineData(@"http://example.com/api/file/get?path=kda/media/images/image.jpg&path1=blablabla.jpg")]
        [InlineData(@"/api/file/get?path=kda/media/images/image.jpg&path1=blablabla.jpg")]
        [InlineData(@"~/api/file/get?path=kda/media/images/image.jpg&path1=blablabla.jpg")]
        public void GetThumbnailLink_S3Image(string originalImageLink)
        {
            var expectedResult = @"/api/file/get?path=kda/media/images/thumbnails/thumbnail.jpg";

            Setup<IKenticoSiteProvider, string>(s => s.GetFullUrl(), @"http://example.com");
            Setup<IKenticoResourceService, string>(s => s.GetMediaLibrariesLocation(), @"/kda/media");
            Setup<IKenticoMediaProvider, string>(s => s.GetThumbnailPath(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), @"/thumbnails/thumbnail.jpg");

            var actualResult = Sut.GetThumbnailLink(originalImageLink);

            Assert.Equal(expectedResult, actualResult);
        }

        [Theory(DisplayName = "ImageService.GetThumbnailLink() | Local image")]
        [InlineData(@"http://example.com/kda/media/images/image.jpg", @"http://example.com", @"/kda/media", @"thumbnails/thumbnail.jpg")]
        [InlineData(@"http://example.com/kda/media/images/image.jpg", @"http://example.com", @"KDA/media", @"/thumbnails/thumbnail.jpg")]
        [InlineData(@"http://example.com/kda/media/images/image.jpg", @"http://example.com", @"/KDA/media", @"/thumbnails/thumbnail.jpg")]
        [InlineData(@"/kda/media/images/image.jpg", @"http://example.com", @"/KDA/media", @"/thumbnails/thumbnail.jpg")]
        [InlineData(@"kda/media/images/image.jpg", @"http://example.com", @"/KDA/media", @"/thumbnails/thumbnail.jpg")]
        [InlineData(@"~/kda/media/images/image.jpg", @"http://example.com", @"/KDA/media", @"/thumbnails/thumbnail.jpg")]
        public void GetThumbnailLink_LocalImage(string originalImageLink, string baseLink, string mediaLibrariesLink, string thumbnailLink)
        {
            var expectedResult = @"/kda/media/images/thumbnails/thumbnail.jpg";

            Setup<IKenticoSiteProvider, string>(s => s.GetFullUrl(), baseLink);
            Setup<IKenticoResourceService, string>(s => s.GetMediaLibrariesLocation(), mediaLibrariesLink);
            Setup<IKenticoMediaProvider, string>(s => s.GetThumbnailPath(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), thumbnailLink);

            var actualResult = Sut.GetThumbnailLink(originalImageLink);

            Assert.Equal(expectedResult, actualResult);
        }

        [Theory(DisplayName = "ImageService.GetThumbnailLink() | Image isn't in media library")]
        [InlineData(@"http://example.com/kda/media/images/image.jpg", @"http://example.com", @"/media", @"thumbnails/thumbnail.jpg")]
        [InlineData(@"/kda/media/images/image.jpg", @"http://example.com", @"/media", @"/thumbnails/thumbnail.jpg")]
        [InlineData(@"~/kda/media/images/image.jpg", @"http://example.com", @"/media", @"/thumbnails/thumbnail.jpg")]
        [InlineData(@"kda/media/images/image.jpg", @"http://example.com", @"/media", @"/thumbnails/thumbnail.jpg")]
        public void GetThumbnailLink_NotMediaImage(string originalImageLink, string baseLink, string mediaLibrariesLink, string thumbnailLink)
        {
            var expectedResult = originalImageLink;

            Setup<IKenticoSiteProvider, string>(s => s.GetFullUrl(), baseLink);
            Setup<IKenticoResourceService, string>(s => s.GetMediaLibrariesLocation(), mediaLibrariesLink);
            Setup<IKenticoMediaProvider, string>(s => s.GetThumbnailPath(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), thumbnailLink);

            var actualResult = Sut.GetThumbnailLink(originalImageLink);

            Assert.Equal(expectedResult, actualResult);
        }
    }
}
