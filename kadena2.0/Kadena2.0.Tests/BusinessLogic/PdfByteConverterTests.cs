using System;
using Xunit;
using Kadena.BusinessLogic.Services;
using Kadena.WebAPI.KenticoProviders.Contracts;
using NReco.PdfGenerator;
using Kadena.Models.SiteSettings;

namespace Kadena.Tests.BusinessLogic
{
    public class PdfByteConverterTests : KadenaUnitTest<PdfByteConverter>
    {
        public PdfByteConverterTests()
        {
            Setup<IKenticoResourceService, string>(s => s.GetSiteSettingsKey(Settings.KDA_NRecoOwner), "PDF_Generator_Src_Examples_Pack_206443366898");
            Setup<IKenticoResourceService, string>(s => s.GetSiteSettingsKey(Settings.KDA_NRecoKey), "ZY6RpF4uqHLQfQEcY8SetuBverdBLHGGFabv2xJL41o2d0XsORPQboV5rgl0fzMcdUnu5uH7cpCs3ThxZ8fdfsrYLQ5+Zq055UG3tbpQVDKSgnOf1QLDIoiddcbTKiqoe8VDIlrRMv69fyZXlt/T78hnVUDr/jQXfLarAu3iEHY=");
        }


        [Theory]
        [ClassData(typeof(PdfByteConverterTests))]
        public void PdfByteConverter(IKenticoResourceService kenticoResourceService, HtmlToPdfConverter htmlPdfConverter)
        {
            Assert.Throws<ArgumentNullException>(() => new PdfByteConverter(kenticoResourceService, htmlPdfConverter));
        }

        [Theory]
        [InlineData("whatever", null)]
        [InlineData("", "")]
        [InlineData("whatever", "")]
        [InlineData("", "whatever")]
        [InlineData("whatever", "whatever")]
        public void GetBytes(string content, string cover)
        {
            var actualResult = Sut.GetBytes(content, cover);

            Assert.NotNull(actualResult);
        }

        [Fact]
        public void GetBytes_NullContent()
        {
            Assert.Throws<ArgumentNullException>(() => Sut.GetBytes(null, "123123"));
        }
    }
}
