using Kadena.Models;
using Xunit;

namespace Kadena.Tests.CreditCard
{
    public class PaymentMethodTest : KadenaUnitTest<PaymentMethod>
    {
        [Theory(DisplayName = "PaymentMethod.ShortClassName")]
        [InlineData("KDA.PaymentMethod.CreditCard","CreditCard")]
        [InlineData("KDA.PaymentMethods.MonthlyPayment", "MonthlyPayment")]
        [InlineData("NoPaymentRequired", "NoPaymentRequired")]
        public void ShortClassNameTest(string fullName, string shortName)
        {
            // Arrange
            var sut = Sut;
            sut.ClassName = fullName;

            var actualShortName = sut.ShortClassName;

            // Assert
            Assert.Equal(shortName, actualShortName);
        }
    }
}
