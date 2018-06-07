using Kadena.Models;
using Kadena.Models.Checkout;
using System.Linq;
using Xunit;

namespace Kadena.Tests.Models
{
    public class PaymentMethodsTest : KadenaUnitTest<PaymentMethods>
    {
        [Fact(DisplayName = "PaymentMethods.CheckDefault()")]
        public void PaymentMethodsTest_CheckDefault()
        {
            // Arrange
            var sut = Sut;
            sut.Items = new System.Collections.Generic.List<PaymentMethod>()
                {
                    { new PaymentMethod() { Title = "pm1", Disabled = true, Checked = true} },
                    { new PaymentMethod() { Title = "pm2", Disabled = false, Checked = false} },
                    { new PaymentMethod() { Title = "pm3", Disabled = false, Checked = false} }
                };

            // Act
            sut.CheckDefault();

            // Assert
            Assert.True(sut.Items.Where(i => i.Checked).Count() == 1);
            Assert.Equal("pm2", sut.Items.First(i => i.Checked).Title);
        }
    }
}
