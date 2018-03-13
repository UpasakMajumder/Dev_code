using Kadena.BusinessLogic.Services;
using Kadena.Models;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Moq;
using Moq.AutoMock;
using System.Collections.Generic;
using Xunit;

namespace Kadena.Tests.BusinessLogic
{
    public class RoleServiceTests
    {
        private const int userId = 123;
        private const int siteId = 5113;
        private const string userName = "lord@craft.com";

        private IEnumerable<Role> GetAllKenticoRoles()
        {
            return new[] {
                new Role { CodeName = "OtherKenticoRole" },
                new Role { CodeName = "Mage" },
                new Role { CodeName = "Knight" },
                new Role { CodeName = "King" },
                new Role { CodeName = "Archer" },
                new Role { CodeName = "Peasant" },
                new Role { CodeName = "ManuallyCreatedRole" }
            };
        }

        private IEnumerable<Role> GetUserRoles()
        {
            return new[] {
                new Role { CodeName = "Peasant" },
                new Role { CodeName = "OtherKenticoRole" },
                new Role { CodeName = "ManuallyCreatedRole" }
            };
        }

        private AutoMocker GetRoleServiceAutomocker()
        {
            var automocker = new AutoMocker();
            var rolesMock = automocker.GetMock<IKenticoRoleProvider>();
            rolesMock.Setup(r => r.GetUserRoles(userId))
                .Returns(GetUserRoles());
            rolesMock.Setup(r => r.GetRoles(siteId))
                .Returns(GetAllKenticoRoles());
            return automocker;
        }

        [Fact]
        public void RoleServiceTest_AllOk()
        {
            // Arrange            
            var ssoRoles = new[] {
                "King",
                "Peasant"
            };

            var automocker = GetRoleServiceAutomocker();
            var loggerMock = automocker.GetMock<IKenticoLogger>();
            var user = new User { UserId = userId, UserName = userName };
            var rolesMock = automocker.GetMock<IKenticoRoleProvider>();
            var sut = automocker.CreateInstance<RoleService>();

            // Act
            sut.AssignSSORoles( user, siteId, ssoRoles );

            //Assert
            loggerMock.VerifyNoOtherCalls();
            rolesMock.Verify(m => m.AssignUserRoles(userName, siteId, new[] { "King" }), Times.Once);
            rolesMock.Verify(m => m.RemoveUserRoles(userName, siteId, new[] { "OtherKenticoRole", "ManuallyCreatedRole" }), Times.Once);
        }


        [Fact]
        public void RoleServiceTest_UnknownRole()
        {
            // Arrange
            var ssoRoles = new[] {
                "King",
                "Peasant",
                "Archer",
                "RoleUnknownInKentico"
            };

            var automocker = GetRoleServiceAutomocker();
            var loggerMock = automocker.GetMock<IKenticoLogger>();
            var user = new User { UserId = userId, UserName = userName };
            var rolesMock = automocker.GetMock<IKenticoRoleProvider>();
            var sut = automocker.CreateInstance<RoleService>();

            // Act
            sut.AssignSSORoles(user, siteId, ssoRoles);

            //Assert
            loggerMock.Verify(l => l.LogError("SSO Update Roles",
                                              It.Is<string>(s => s.Contains("RoleUnknownInKentico"))), Times.Once);
            loggerMock.VerifyNoOtherCalls();
            rolesMock.Verify(m => m.AssignUserRoles(userName, siteId, new[] { "King", "Archer" }), Times.Once);
            rolesMock.Verify(m => m.RemoveUserRoles(userName, siteId, new[] { "OtherKenticoRole", "ManuallyCreatedRole" }), Times.Once);
        }
    }
}
