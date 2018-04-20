using Kadena.BusinessLogic.Services;
using Kadena.Models.SiteSettings;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Kadena.Tests.BusinessLogic
{
    public class PdfServiceTests : KadenaUnitTest<PdfService>
    {
        private const string _correctOrderNo = "123-123-123-123";
        private const int _correctLineNo = 4;
        private const string _correctSalt = "secretsalt";
        private const string _correctHash = "705C4EF64558D816E08F5209463DCE24C4DDDB33E3136106736B4885A4795EBE";
        private const string _notFoundUrl = "http://no.pdf/found";

        [Fact]
        public void HashComputationTest()
        {
            Setup<IKenticoResourceService, string>(r => r.GetSiteSettingsKey(Settings.KDA_HiresPdfLinkHashSalt), _correctSalt);

            var result = Sut.GetHiresPdfUrl(_correctOrderNo, _correctLineNo);

            Assert.Equal($"/api/pdf/hires/{_correctOrderNo}/{_correctLineNo}?hash={_correctHash}", result);
        }


        [Theory(DisplayName = "PdfServiceTests - Passing changed data")]
        [InlineData("666-666-666-666", _correctLineNo, _correctHash, _correctSalt)]
        [InlineData(_correctOrderNo, 666, _correctHash, _correctSalt)]
        [InlineData(_correctOrderNo, _correctLineNo, "wronghash", _correctSalt)]
        [InlineData(_correctOrderNo, _correctLineNo, _correctHash, "WRONGsalt")]
        public async Task HashFailedTest_WrongSalt(string orderId, int lineNo, string hash, string salt)
        {
            Setup<IKenticoResourceService, string>(r => r.GetSiteSettingsKey(Settings.KDA_HiresPdfLinkHashSalt), salt);
            Setup<IKenticoDocumentProvider, string>(d => d.GetDocumentAbsoluteUrl(It.IsAny<string>()), _notFoundUrl);

            var result = await Sut.GetHiresPdfRedirectLink(orderId, lineNo, hash);

            Assert.Equal(_notFoundUrl, result);
            Verify<IKenticoLogger>(l => l.LogError("GetHiresPdfLink", $"Failed to verify request hash"), Times.Once);
        }
    }
}
