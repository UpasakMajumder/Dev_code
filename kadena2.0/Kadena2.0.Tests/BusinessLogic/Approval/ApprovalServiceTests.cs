using Kadena.BusinessLogic.Contracts.Approval;
using Kadena.BusinessLogic.Services.Approval;
using Kadena.Dto.Approval.MicroserviceRequests;
using Kadena.Dto.General;
using Kadena.Models.Membership;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Kadena.Tests.BusinessLogic.Approval
{
    public class ApprovalServiceTests : KadenaUnitTest<ApprovalService>
    {
        const string orderId = "ord123";
        const int customerId = 56;
        const string customerName = "John Customer";
        const int approverUserId = 11;

        [Fact]
        public async Task ApproveOrderTest()

        {
            Setup<IKenticoUserProvider, User>(u => u.GetCurrentUser(), new User { UserId = approverUserId });
            Setup<IApproverService, bool>(a => a.IsCustomersApprover(approverUserId, customerId), true);
            Setup<IApprovalServiceClient, Task<BaseResponseDto<string>>>(s => s.Approval(It.IsAny<ApprovalRequestDto>()),
                Task.FromResult(new BaseResponseDto<string>() { Success = true, Payload = ApprovalState.Approved.ToString() }));

            await Sut.ApproveOrder(orderId, customerId, customerName);

            Verify<IApprovalServiceClient>(s => s.Approval(It.Is<ApprovalRequestDto>(req => req.ApproversCount == 1 &&
                                                                                            req.OrderId == orderId &&
                                                                                            req.Approvals.Length == 1 &&
                                                                                            req.Approvals[0].State == (int)ApprovalState.Approved &&
                                                                                            req.Approvals[0].Customer.Id == customerId &&
                                                                                            req.Approvals[0].Customer.Name == customerName
                                                                                            )), Times.Once);
            
            Verify<IKenticoLogger>(l => l.LogInfo("ApproveOrder", "Info", It.Is<string>(s => s.Contains(orderId) && 
                                                                                             s.Contains(ApprovalState.Approved.ToString()))), Times.Once);
            VerifyNoOtherCalls<IKenticoLogger>();
        }

        [Fact]
        public async Task ApproveOrderTest_ErrorLogged()
        {
            Setup<IKenticoUserProvider, User>(u => u.GetCurrentUser(), new User { UserId = approverUserId });
            Setup<IApproverService, bool>(a => a.IsCustomersApprover(approverUserId, customerId), true);
            Setup<IApprovalServiceClient, Task<BaseResponseDto<string>>>(s => s.Approval(It.IsAny<ApprovalRequestDto>()),
                Task.FromResult(new BaseResponseDto<string>() { Success = false, Payload = null }));

            try
            {
                await Sut.ApproveOrder(orderId, customerId, customerName);
            }
            catch (ApprovalServiceException) { }
            
            Verify<IKenticoLogger>(l => l.LogError("ApproveOrder", It.Is<string>(s => s.Contains(orderId))), Times.Once);
        }

        [Fact]
        public async Task ApproveOrderTest_StatusLogged()
        {
            const string badStatus = "SomeUnknownStatus";
            Setup<IKenticoUserProvider, User>(u => u.GetCurrentUser(), new User { UserId = approverUserId });
            Setup<IApproverService, bool>(a => a.IsCustomersApprover(approverUserId, customerId), true);
            Setup<IApprovalServiceClient, Task<BaseResponseDto<string>>>(s => s.Approval(It.IsAny<ApprovalRequestDto>()),
                Task.FromResult(new BaseResponseDto<string>() { Success = true, Payload = badStatus }));

            try
            {
                await Sut.ApproveOrder(orderId, customerId, customerName);
            }
            catch (ApprovalServiceException) { }

            Verify<IKenticoLogger>(l => l.LogError("ApproveOrder", It.Is<string>(s => s.Contains(badStatus) && 
                                                                                      s.Contains(orderId))), Times.Once);
        }

        [Fact]
        public async Task ApproveOrderTest_Exception()
        {
            Setup<IApproverService, bool>(a => a.IsCustomersApprover(approverUserId, customerId), false);

            await Assert.ThrowsAsync<Exception>(async () => await Sut.ApproveOrder(orderId, customerId, customerName));
        }


        [Fact]
        public async Task RejectOrderTest()
        {
            const string rejectNote = "because";

            Setup<IKenticoUserProvider, User>(u => u.GetCurrentUser(), new User { UserId = approverUserId });
            Setup<IApproverService, bool>(a => a.IsCustomersApprover(approverUserId, customerId), true);
            Setup<IApprovalServiceClient, Task<BaseResponseDto<string>>>(s => s.Approval(It.IsAny<ApprovalRequestDto>()),
                Task.FromResult(new BaseResponseDto<string>() { Success = true, Payload = ApprovalState.ApprovalRejected.ToString() }));

            await Sut.RejectOrder(orderId, customerId, customerName, rejectNote);

            Verify<IApprovalServiceClient>(s => s.Approval(It.Is<ApprovalRequestDto>(req => req.ApproversCount == 1 &&
                                                                                            req.OrderId == orderId &&
                                                                                            req.Approvals.Length == 1 &&
                                                                                            req.Approvals[0].State == (int)ApprovalState.ApprovalRejected &&
                                                                                            req.Approvals[0].Customer.Id == customerId &&
                                                                                            req.Approvals[0].Customer.Name == customerName &&
                                                                                            req.Approvals[0].Note == rejectNote
                                                                                            )), Times.Once);
            
            Verify<IKenticoLogger>(l => l.LogInfo("RejectOrder", "Info", It.Is<string>(s => s.Contains(orderId) &&
                                                                                            s.Contains(ApprovalState.ApprovalRejected.ToString()) &&
                                                                                            s.Contains(rejectNote))), Times.Once);
            VerifyNoOtherCalls<IKenticoLogger>();
        }

        [Fact]
        public async Task RejectOrderTest_ErrorLogged()
        {
            Setup<IKenticoUserProvider, User>(u => u.GetCurrentUser(), new User { UserId = approverUserId });
            Setup<IApproverService, bool>(a => a.IsCustomersApprover(approverUserId, customerId), true);
            Setup<IApprovalServiceClient, Task<BaseResponseDto<string>>>(s => s.Approval(It.IsAny<ApprovalRequestDto>()),
                Task.FromResult(new BaseResponseDto<string>() { Success = false, Payload = null }));

            try
            {
                await Sut.RejectOrder(orderId, customerId, customerName);
            }
            catch (ApprovalServiceException) { }

            Verify<IKenticoLogger>(l => l.LogError("RejectOrder", It.Is<string>(s => s.Contains(orderId))), Times.Once);
        }

        [Fact]
        public async Task RejectOrderTest_StatusLogged()
        {
            const string badStatus = "SomeUnknownStatus";
            Setup<IKenticoUserProvider, User>(u => u.GetCurrentUser(), new User { UserId = approverUserId });
            Setup<IApproverService, bool>(a => a.IsCustomersApprover(approverUserId, customerId), true);
            Setup<IApprovalServiceClient, Task<BaseResponseDto<string>>>(s => s.Approval(It.IsAny<ApprovalRequestDto>()),
                Task.FromResult(new BaseResponseDto<string>() { Success = true, Payload = badStatus }));

            try
            {
                await Sut.RejectOrder(orderId, customerId, customerName);
            }
            catch (ApprovalServiceException) { }

            Verify<IKenticoLogger>(l => l.LogError("RejectOrder", It.Is<string>(s => s.Contains(orderId) && s.Contains(badStatus))), Times.Once);
        }
        
        [Fact]
        public async Task RejectOrderTest_Exception()
        {
            Setup<IApproverService, bool>(a => a.IsCustomersApprover(approverUserId, customerId), false);

            await Assert.ThrowsAsync<Exception>(async () => await Sut.RejectOrder(orderId, customerId, customerName));
        }
    }
}
