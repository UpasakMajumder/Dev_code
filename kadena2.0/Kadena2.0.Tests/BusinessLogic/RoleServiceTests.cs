using Kadena.BusinessLogic.Services;
using Kadena.Models.Membership;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Kadena.Tests.BusinessLogic
{
    public class RoleServiceTests : KadenaUnitTest<RoleService>
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

        private void SetupRoleService()
        {
            Setup<IKenticoRoleProvider, IEnumerable<Role>>(r => r.GetUserRoles(userId), GetUserRoles());
            Setup<IKenticoRoleProvider, IEnumerable<Role>>(r => r.GetRoles(siteId), GetAllKenticoRoles());
        }

        [Fact(DisplayName = "RoleService.AssignSSORoles() | All assigned")]
        public void RoleServiceTest_AllOk()
        {
            // Arrange            
            var ssoRoles = new[] {
                "King",
                "Peasant"
            };
            var user = new User { UserId = userId, UserName = userName };
            SetupRoleService();

            // Act
            Sut.AssignSSORoles( user, siteId, ssoRoles );

            //Assert
            VerifyNoOtherCalls<IKenticoLogger>();
            Verify<IKenticoRoleProvider>(m => m.AssignUserRoles(userName, siteId, new[] { "King" }), Times.Once);
            Verify<IKenticoRoleProvider>(m => m.RemoveUserRoles(userName, siteId, new[] { "OtherKenticoRole", "ManuallyCreatedRole" }), Times.Once);
        }


        [Fact(DisplayName = "RoleService.AssignSSORoles() | Unknown role")]
        public void RoleServiceTest_UnknownRole()
        {
            // Arrange
            var ssoRoles = new[] {
                "King",
                "Peasant",
                "Archer",
                "RoleUnknownInKentico"
            };
            var user = new User { UserId = userId, UserName = userName };
            SetupRoleService();

            // Act
            Sut.AssignSSORoles(user, siteId, ssoRoles);

            //Assert
            Verify<IKenticoLogger>(l => l.LogError("SSO Update Roles", It.Is<string>(s => s.Contains("RoleUnknownInKentico"))), Times.Once);
            VerifyNoOtherCalls<IKenticoLogger>();
            Verify<IKenticoRoleProvider>(m => m.AssignUserRoles(userName, siteId, new[] { "King", "Archer" }), Times.Once);
            Verify<IKenticoRoleProvider>(m => m.RemoveUserRoles(userName, siteId, new[] { "OtherKenticoRole", "ManuallyCreatedRole" }), Times.Once);
        }
    }
}
