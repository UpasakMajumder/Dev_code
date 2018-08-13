using Kadena.BusinessLogic.Contracts;
using Kadena.BusinessLogic.Services;
using Kadena.Models.CampaignData;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using Xunit;

namespace Kadena.Tests.BusinessLogic
{
    public class PdfPreBuyCatalogServiceTests : KadenaUnitTest<PdfPreBuyCatalogService>
    {
        [Theory]
        [ClassData(typeof(PdfPreBuyCatalogServiceTests))]
        public void PreBuyPdfCatalogService(IByteConverter byteConverter, IKenticoResourceService resourceService, IKenticoCampaignsProvider campaignsProvider,
            IKenticoProgramsProvider programsProvider, IKenticoBrandsProvider brandsProvider, IKenticoProductsProvider productsProvider,
            IKenticoAddressBookProvider addressBookProvider, IImageService imageService, IKenticoSiteProvider siteProvider)
        {
            Assert.Throws<ArgumentNullException>(() => new PdfPreBuyCatalogService(byteConverter, resourceService, campaignsProvider, programsProvider, brandsProvider,
                productsProvider, addressBookProvider, imageService, siteProvider));
        }

        [Fact]
        public void Generate_NotExistingCampaign()
        {
            var actualResult = Sut.Generate(0);

            Assert.Null(actualResult);
        }

        [Fact]
        public void Generate()
        {
            Setup<IKenticoCampaignsProvider, CampaignData>(s => s.GetCampaign(0), new CampaignData());

            var actualResult = Sut.Generate(0);

            Assert.NotNull(actualResult);
        }
    }
}
