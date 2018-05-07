using Kadena.BusinessLogic.Services;
using Kadena.Models.Membership;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Kadena.Tests.BusinessLogic
{
    public class RoleServiceTests : KadenaUnitTest<RoleService>
    {
        private const int userId = 123;
        private const int siteId = 5113;
        private const string userName = "lord@craft.com";

        public static IEnumerable<object[]> GetRoles()
        {
            yield return new object[]
            {
                new string[] {
                    "Role1",
                    "Role2"
                }
            };
            yield return new object[]
            {
                new string[] {
                }
            };
        }

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

        [Theory(DisplayName = "RoleService.AssignRole() | Success")]
        [MemberData(nameof(GetRoles))]
        public void AssginRole_Success(IEnumerable<string> roles)
        {
            var exc = Record.Exception(() => Sut.AssignRoles(new User(), 0, roles));

            Assert.Null(exc);
        }

        [Fact(DisplayName = "RoleService.AssignRole() | Null roles")]
        public void AssginRole_Null()
        {
            Action act = () => Sut.AssignRoles(new User(), 0, null);

            var exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("roles", exception.ParamName);
        }
    }
}
