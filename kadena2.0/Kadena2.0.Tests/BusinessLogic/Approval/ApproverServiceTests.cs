using Kadena.BusinessLogic.Services.Approval;
using Kadena.Models;
using Kadena.Models.Membership;
using Kadena.Models.Site;
using Kadena.Models.SiteSettings.Permissions;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using Moq;
using System;
using Xunit;

namespace Kadena.Tests.BusinessLogic.Approval
{
    public class ApproverServiceTests : KadenaUnitTest<ApproverService>
    {
        const int customerId = 56;
        const int approverUserId = 11;

        [Fact]
        public void GetApproversTest()
        {
            const int siteId = 1;

            Sut.GetApprovers(siteId);

            Verify<IKenticoPermissionsProvider>(p => p.GetUsersWithPermission("Kadena_Orders", "KDA_ApproveOrders", siteId), Times.Once);
        }

        [Fact]
        public void IsCustomersApproverTest_BadIds()
        {
            const int badApproverUserId = 0;
            const int badCustomerId = 0;

            var result = Sut.IsCustomersApprover(badApproverUserId, badCustomerId);

            Assert.False(result);
        }

        [Fact]
        public void IsCustomersApproverTest_NotApprover()
        {
            const int badApproverUserId = 1;
            const int badCustomerId = 2;
            Setup<IKenticoPermissionsProvider, bool>(p => p.UserHasPermission(badApproverUserId, ModulePermissions.KadenaOrdersModule, ModulePermissions.KadenaOrdersModule.ApproveOrders), false);

            var result = Sut.IsCustomersApprover(badApproverUserId, badCustomerId);

            Assert.False(result);
            Verify<IKenticoPermissionsProvider>(p => p.UserHasPermission(badApproverUserId, ModulePermissions.KadenaOrdersModule, ModulePermissions.KadenaOrdersModule.ApproveOrders), Times.Once);
        }

        [Fact]
        public void IsCustomersApproverTest_WrongApprover()
        {
            const int badApproverUserId = 666;
            Setup<IKenticoPermissionsProvider, bool>(p => p.UserHasPermission(approverUserId, ModulePermissions.KadenaOrdersModule, ModulePermissions.KadenaOrdersModule.ApproveOrders), true);
            Setup<IKenticoCustomerProvider, Customer>(p => p.GetCustomer(customerId), new Customer { ApproverUserId = badApproverUserId });

            var result = Sut.IsCustomersApprover(approverUserId, customerId);

            Assert.False(result);
            Verify<IKenticoPermissionsProvider>(p => p.UserHasPermission(approverUserId, ModulePermissions.KadenaOrdersModule, ModulePermissions.KadenaOrdersModule.ApproveOrders), Times.Once);
            Verify<IKenticoCustomerProvider>(p => p.GetCustomer(customerId), Times.Once);
        }

        [Fact]
        public void IsCustomersApproverTest()
        {         
            Setup<IKenticoPermissionsProvider, bool>(p => p.UserHasPermission(approverUserId, ModulePermissions.KadenaOrdersModule, ModulePermissions.KadenaOrdersModule.ApproveOrders), true);
            Setup<IKenticoCustomerProvider, Customer>(p => p.GetCustomer(customerId), new Customer { ApproverUserId = approverUserId, SiteId = 1 });
            Setup<IKenticoSiteProvider, KenticoSite>(p => p.GetKenticoSite(), new KenticoSite { Id = 1, Name = "KDA" });

            var result = Sut.IsCustomersApprover(approverUserId, customerId);

            Assert.True(result);
            Verify<IKenticoPermissionsProvider>(p => p.UserHasPermission(approverUserId, ModulePermissions.KadenaOrdersModule, ModulePermissions.KadenaOrdersModule.ApproveOrders), Times.Once);
            Verify<IKenticoCustomerProvider>(p => p.GetCustomer(customerId), Times.Once);
        }

        [Fact]
        public void CheckIsCustomersApproverTest()
        {
            const int customerId = 2;
            const int userId = 6;
            const string customerName = "John";
            Setup<IKenticoUserProvider, User>(p => p.GetCurrentUser(), new User { UserId = 6 });
            Setup<IKenticoPermissionsProvider, bool>(p => p.UserHasPermission(userId, ModulePermissions.KadenaOrdersModule, ModulePermissions.KadenaOrdersModule.ApproveOrders), true);
            Setup<IKenticoCustomerProvider, Customer>(p => p.GetCustomer(customerId), new Customer { ApproverUserId = userId });

            Sut.CheckIsCustomersApprover(customerId, customerName);
        }

        [Fact]
        public void CheckIsCustomersApproverTest_NotApprover()
        {
            const int customerId = 2;
            const int userId = 6;
            const string customerName = "John";
            Setup<IKenticoUserProvider, User>(p => p.GetCurrentUser(), new User { UserId = 6 });
            Setup<IKenticoPermissionsProvider, bool>(p => p.UserHasPermission(userId, ModulePermissions.KadenaOrdersModule, ModulePermissions.KadenaOrdersModule.ApproveOrders), false);
            
            var exception = Assert.Throws<Exception>(() => Sut.CheckIsCustomersApprover(customerId, customerName));
            Assert.Contains(customerName, exception.Message);
        }

        [Fact]
        public void CheckIsCustomersApproverTest_NotAssignedToCustomer()
        {
            const int customerId = 2;
            const int userId = 6;
            const string customerName = "John";
            Setup<IKenticoUserProvider, User>(p => p.GetCurrentUser(), new User { UserId = 6 });
            Setup<IKenticoPermissionsProvider, bool>(p => p.UserHasPermission(userId, ModulePermissions.KadenaOrdersModule, ModulePermissions.KadenaOrdersModule.ApproveOrders), true);
            Setup<IKenticoCustomerProvider, Customer>(p => p.GetCustomer(customerId), new Customer { ApproverUserId = userId+1 });

            var exception = Assert.Throws<Exception>(() => Sut.CheckIsCustomersApprover(customerId, customerName));
            Assert.Contains(customerName, exception.Message);
        }

        [Fact]
        public void CheckIsCustomersEditorTest()
        {
            const int customerId = 2;
            const int userId = 6;
            Setup<IKenticoUserProvider, User>(p => p.GetCurrentUser(), new User { UserId = 6 });
            Setup<IKenticoPermissionsProvider, bool>(p => p.UserHasPermission(userId, ModulePermissions.KadenaOrdersModule, ModulePermissions.KadenaOrdersModule.ApproveOrders), true);
            Setup<IKenticoPermissionsProvider, bool>(p => p.UserHasPermission(userId, ModulePermissions.KadenaOrdersModule, ModulePermissions.KadenaOrdersModule.EditOrdersInApproval), true);
            Setup<IKenticoCustomerProvider, Customer>(p => p.GetCustomer(customerId), new Customer { ApproverUserId = userId });

            Sut.CheckIsCustomersEditor(customerId);
        }

        [Fact]
        public void CheckIsCustomersEditorTest_NotEditor()
        {
            const int customerId = 2;
            const int userId = 6;
            Setup<IKenticoUserProvider, User>(p => p.GetCurrentUser(), new User { UserId = 6 });
            Setup<IKenticoPermissionsProvider, bool>(p => p.UserHasPermission(userId, ModulePermissions.KadenaOrdersModule, ModulePermissions.KadenaOrdersModule.EditOrdersInApproval), false);

            Assert.Throws<Exception>(() => Sut.CheckIsCustomersEditor(customerId));
        }

        [Fact]
        public void CheckIsCustomersEditorTest_NotAssignedToCustomer()
        {
            const int customerId = 2;
            const int userId = 6;
            Setup<IKenticoUserProvider, User>(p => p.GetCurrentUser(), new User { UserId = 6 });
            Setup<IKenticoPermissionsProvider, bool>(p => p.UserHasPermission(userId, ModulePermissions.KadenaOrdersModule, ModulePermissions.KadenaOrdersModule.EditOrdersInApproval), true);
            Setup<IKenticoCustomerProvider, Customer>(p => p.GetCustomer(customerId), new Customer { ApproverUserId = userId + 1 });

            Assert.Throws<Exception>(() => Sut.CheckIsCustomersEditor(customerId));
        }
    }
}
