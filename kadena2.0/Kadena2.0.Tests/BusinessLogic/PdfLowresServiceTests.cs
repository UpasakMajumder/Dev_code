using Kadena.BusinessLogic.Services;
using Kadena.Models.SiteSettings;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Kadena.Tests.BusinessLogic
{
    public class PdfLowresServiceTests : KadenaUnitTest<PdfService>
    {
        private const string _correctSalt = "secretsalt";
        private const string _correctHash = "058D1BC3D09A0FBBB1A57097B79C2BF8FBBF081A0B5F895D9A11F7DE7E604D79";
        private const string _notFoundUrl = "http://no.pdf/found";
        private const string _templateId = "211abf7f-3b51-49c3-b321-e5abf9cb5b9b";
        private const string _settingsId = "b312c262-f479-4bc8-886b-4dcee4eddb30";

        [Fact]
        public void HashComputationTest_Lowres()
        {
            Setup<IKenticoResourceService, string>(r => r.GetSiteSettingsKey(Settings.KDA_HiresPdfLinkHashSalt), _correctSalt);
            Setup<IKenticoSiteProvider, string, string>(s => s.GetAbsoluteUrl(It.IsAny<string>()), s => s);

            var result = Sut.GetLowresPdfUrl(Guid.Parse(_templateId), Guid.Parse(_settingsId));

            Assert.Equal($"/api/pdf/lowres/{_templateId}/{_settingsId}?hash={_correctHash}", result);
        }


        [Theory(DisplayName = "PdfLowresServiceTests - Passing changed data")]
        [InlineData("666ABF7F-3B51-49C3-B321-666666666666", _settingsId, _correctHash, _correctSalt)]
        [InlineData(_templateId, "666ABF7F-3B51-49C3-B321-66666666666B", _correctHash, _correctSalt)]
        [InlineData(_templateId, _settingsId, "wronghash", _correctSalt)]
        [InlineData(_templateId, _settingsId, _correctHash, "WRONGsalt")]
        public async Task HashFailedTest_Lowres(string templateId, string settingsid, string hash, string salt)
        {
            Setup<IKenticoResourceService, string>(r => r.GetSiteSettingsKey(Settings.KDA_HiresPdfLinkHashSalt), salt);
            Setup<IKenticoDocumentProvider, string>(d => d.GetDocumentAbsoluteUrl(It.IsAny<string>()), _notFoundUrl);

            var result = await Sut.GetLowresPdfRedirectLink(Guid.Parse(templateId), Guid.Parse(settingsid), hash);

            Assert.Equal(_notFoundUrl, result);
            Verify<IKenticoLogger>(l => l.LogError("GetLowresPdfLink", $"Failed to verify request hash"), Times.Once);
        }
    }
}
