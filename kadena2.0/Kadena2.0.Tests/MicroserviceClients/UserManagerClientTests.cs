using Kadena2.MicroserviceClients.Clients;
using Kadena2.MicroserviceClients.Contracts.Base;
using System;
using Xunit;
using Kadena.Models.Membership;

namespace Kadena.Tests.MicroserviceClients
{
    public class UserManagerClientTests : KadenaUnitTest<UserManagerClient>
    {
        [Theory]
        [ClassData(typeof(UserManagerClientTests))]
        public void UserManagerClient(IMicroProperties properties)
        {
            Assert.Throws<ArgumentNullException>(() => new UserManagerClient(properties));
        }

        [Fact]
        public void Create()
        {
            var exception = Record.Exception(() => Sut.Create(new User()));

            Assert.Null(exception);
        }
    }
}
