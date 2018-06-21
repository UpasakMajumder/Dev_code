using Kadena.BusinessLogic.Services.Approval;
using Kadena.Models;
using Kadena.Models.SiteSettings.Permissions;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using Moq;
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
            Setup<IKenticoCustomerProvider, Customer>(p => p.GetCustomer(customerId), new Customer { ApproverUserId = approverUserId });

            var result = Sut.IsCustomersApprover(approverUserId, customerId);

            Assert.True(result);
            Verify<IKenticoPermissionsProvider>(p => p.UserHasPermission(approverUserId, ModulePermissions.KadenaOrdersModule, ModulePermissions.KadenaOrdersModule.ApproveOrders), Times.Once);
            Verify<IKenticoCustomerProvider>(p => p.GetCustomer(customerId), Times.Once);
        }

        public void CheckIsCustomersApproverTest()
        {
            // Sut.CheckIsCustomersApprover(); TODO
        }

        public void CheckIsCustomersEditorTest()
        {
             // Sut.CheckIsCustomersEditor(); TODO
        }
    }
}
