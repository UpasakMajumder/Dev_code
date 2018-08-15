using Kadena2.MicroserviceClients.Clients;
using Kadena2.MicroserviceClients.Contracts.Base;
using System;
using Xunit;
using Kadena.Models.Membership;
using AutoMapper;
using System.Threading.Tasks;

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
        public async Task Create()
        {
            var exception = await Record.ExceptionAsync(() => Sut.Create(null));

            Assert.Null(exception);
        }
    }
}
