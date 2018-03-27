using Kadena.Models;
using Xunit;

namespace Kadena.Tests.CreditCard
{
    public class PaymentMethodTest
    {
        [Theory]
        [InlineData("KDA.PaymentMethod.CreditCard","CreditCard")]
        [InlineData("KDA.PaymentMethods.MonthlyPayment", "MonthlyPayment")]
        [InlineData("NoPaymentRequired", "NoPaymentRequired")]
        public void ShortClassNameTest(string fullName, string shortName)
        {
            // Arrange
            var payment = new PaymentMethod { ClassName = fullName };

            // Assert
            Assert.Equal(shortName, payment.ShortClassName);
        }
    }
}
