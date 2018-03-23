using Kadena.BusinessLogic.Services;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace Kadena.Tests.BusinessLogic
{
    public class UserServiceTest
    {
        [Fact(DisplayName = "UserService.AcceptTaC()")]
        public void AcceptTaC()
        {
            var autoMock = new AutoMocker();
            var sut = autoMock.CreateInstance<UserService>();
            var userProvider = autoMock.GetMock<IKenticoUserProvider>();

            sut.AcceptTaC();

            userProvider.Verify(s => s.AcceptTaC(), Times.AtLeastOnce);
        }
    }
}
