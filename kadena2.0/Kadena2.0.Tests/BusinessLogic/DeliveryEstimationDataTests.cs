using Kadena.BusinessLogic.Services.Orders;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Moq;
using System.Linq;
using Xunit;

namespace Kadena.Tests.BusinessLogic
{
    public class DeliveryEstimationDataTests : KadenaUnitTest<DeliveryEstimationDataService>
    {
        [Fact]
        public void GetWeightInSiteUnitTest()
        {
            // Arrange
            const decimal weight = 3.14m;
            const string mass = "lb";
            Setup<IKenticoResourceService, string>(r => r.GetMassUnit(), mass);

            // Act
            var result = Sut.GetDeliveryEstimationRequestData("", "", weight, null).FirstOrDefault();

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Weight);
            Assert.Equal(mass, result.Weight.Unit);
            Assert.Equal(weight, result.Weight.Value);
            Verify<IKenticoResourceService>(r => r.GetMassUnit(), Times.Once);
        }

        [Fact]
        public void GetSourceAddressTest_LoadsProperSettinKeys()
        {
            // Arrange
            Setup<IKenticoResourceService, string>(r => r.GetSiteSettingsKey("KDA_EstimateDeliveryPrice_SenderAddressLine1"), "Line1");
            Setup<IKenticoResourceService, string>(r => r.GetSiteSettingsKey("KDA_EstimateDeliveryPrice_SenderAddressLine2"), "Line2");
            Setup<IKenticoResourceService, string>(r => r.GetSiteSettingsKey("KDA_EstimateDeliveryPrice_SenderCity"), "City");
            Setup<IKenticoResourceService, string>(r => r.GetSiteSettingsKey("KDA_EstimateDeliveryPrice_SenderCountry"), "Country");
            Setup<IKenticoResourceService, string>(r => r.GetSiteSettingsKey("KDA_EstimateDeliveryPrice_SenderPostal"), "Postal");
            Setup<IKenticoResourceService, string>(r => r.GetSiteSettingsKey("KDA_EstimateDeliveryPrice_SenderState"), "State");

            // Act
            var result = Sut.GetSourceAddress();

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.StreetLines);
            Assert.Equal(2, result.StreetLines.Count);
            Assert.Equal("Line1", result.StreetLines[0]);
            Assert.Equal("Line2", result.StreetLines[1]);
            Assert.Equal("City", result.City);
            Assert.Equal("Country", result.Country);
            Assert.Equal("Postal", result.Postal);
            Assert.Equal("State", result.State);

            Verify<IKenticoResourceService>(r => r.GetSiteSettingsKey("KDA_EstimateDeliveryPrice_SenderAddressLine1"), Times.Once);
            Verify<IKenticoResourceService>(r => r.GetSiteSettingsKey("KDA_EstimateDeliveryPrice_SenderAddressLine2"), Times.Once);
            Verify<IKenticoResourceService>(r => r.GetSiteSettingsKey("KDA_EstimateDeliveryPrice_SenderCity"), Times.Once);
            Verify<IKenticoResourceService>(r => r.GetSiteSettingsKey("KDA_EstimateDeliveryPrice_SenderCountry"), Times.Once);
            Verify<IKenticoResourceService>(r => r.GetSiteSettingsKey("KDA_EstimateDeliveryPrice_SenderPostal"), Times.Once);
            Verify<IKenticoResourceService>(r => r.GetSiteSettingsKey("KDA_EstimateDeliveryPrice_SenderState"), Times.Once);
        }

        [Fact]
        public void GetSourceAddress_OneLineAddressTest()
        {
            // Arrange
            Setup<IKenticoResourceService, string>(r => r.GetSiteSettingsKey("KDA_EstimateDeliveryPrice_SenderAddressLine1"), "Line1");
            Setup<IKenticoResourceService, string>(r => r.GetSiteSettingsKey("KDA_EstimateDeliveryPrice_SenderAddressLine2"), "");

            // Act
            var result = Sut.GetSourceAddress();

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.StreetLines);
            Assert.Single(result.StreetLines);
            Assert.Equal("Line1", result.StreetLines[0]);
            Verify<IKenticoResourceService>(r => r.GetSiteSettingsKey("KDA_EstimateDeliveryPrice_SenderAddressLine1"), Times.Once);
            Verify<IKenticoResourceService>(r => r.GetSiteSettingsKey("KDA_EstimateDeliveryPrice_SenderAddressLine2"), Times.Once);
        }
    }
}
