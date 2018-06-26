using Xunit;
using Moq;
using Kadena.BusinessLogic.Services;
using System.Collections.Generic;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.AmazonFileSystemProvider;
using System;

namespace Kadena.Tests.BusinessLogic
{
    public class ArtworkServiceTests : KadenaUnitTest<ArtworkService>
    {
        [Theory(DisplayName = "ArtworkService()")]
        [ClassData(typeof(ArtworkServiceTests))]
        public void ConstructorNull(IKenticoProductsProvider productsProvider, IS3PathService pathService, IKenticoSiteProvider siteProvider)
        {
            Assert.Throws<ArgumentNullException>(() => new ArtworkService(productsProvider, pathService, siteProvider));
        }

        [Fact(DisplayName = "ArtworkService.GetLocation() | Artwork doesn't exists")]
        public void GetLocation_NullArtwork()
        {
            Setup<IKenticoProductsProvider, Uri>(ip => ip.GetProductArtworkUri(It.IsAny<int>()), null);
            Setup<IKenticoSiteProvider, string, string>(ip => ip.GetAbsoluteUrl(Kadena.Helpers.Routes.File.Get), s => $"https://local/{s}");

            // Act
            var actualResult = Sut.GetLocation(0);

            // Assert
            Assert.Null(actualResult);
        }

        [Fact(DisplayName = "ArtworkService.GetLocation() | Artwork is in local storage")]
        public void GetLocation_Local()
        {
            // Arrange 
            var expectedResult = "https://local/files/file.jpg";

            Setup<IKenticoProductsProvider, Uri>(ip => ip.GetProductArtworkUri(It.IsAny<int>()), new Uri(expectedResult));
            Setup<IKenticoSiteProvider, string, string>(ip => ip.GetAbsoluteUrl(Kadena.Helpers.Routes.File.Get), s => $"https://local/{s}");

            // Act
            var actualResult = Sut.GetLocation(0);

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory(DisplayName = "ArtworkService.GetLocation() | Artwork is in S3 storage")]
        [InlineData("")]
        [InlineData("files/file.jpg")]
        [InlineData("files1/files2/file.jpg")]
        public void GetLocation_S3(string path)
        {
            // Arrange 
            var expectedResult = $"environment/{path}";

            Setup<IKenticoProductsProvider, Uri>(ip => ip.GetProductArtworkUri(It.IsAny<int>()), new Uri($"https://local/{Kadena.Helpers.Routes.File.Get}?path={path}"));
            Setup<IS3PathService, string>(ip => ip.GetObjectKeyFromPath(path, true), expectedResult);
            Setup<IKenticoSiteProvider, string, string>(ip => ip.GetAbsoluteUrl(Kadena.Helpers.Routes.File.Get), s => $"https://local/{s}");

            // Act
            var actualResult = Sut.GetLocation(0);

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }
    }
}
