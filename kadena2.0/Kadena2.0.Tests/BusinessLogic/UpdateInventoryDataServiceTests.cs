using Kadena.BusinessLogic.Services;
using Kadena.Dto.General;
using Kadena.Dto.InventoryUpdate.MicroserviceResponses;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Kadena.Tests.BusinessLogic
{
    public class UpdateInventoryDataServiceTests : KadenaUnitTest<UpdateInventoryDataService>
    {
        [Theory]
        [ClassData(typeof(UpdateInventoryDataServiceTests))]
        public void UpdateInventoryDataService(IInventoryUpdateClient microserviceInventory,
                                          IKenticoSkuProvider skuProvider,
                                          IKenticoLogger kenticoLog,
                                          IKenticoResourceService kenticoResources)
        {
            Assert.Throws<ArgumentNullException>(() => new UpdateInventoryDataService(microserviceInventory, skuProvider,
                kenticoLog, kenticoResources));
        }

        [Fact(DisplayName = "UpdateInventoryData() | Microservice failed")]
        public async Task UpdateInventory_Failed()
        {
            var expectedResult = "some error";

            Setup<IInventoryUpdateClient, Task<BaseResponseDto<InventoryDataItemDto[]>>>(s => s.GetInventoryItems(It.IsAny<string>()),
                Task.FromResult(new BaseResponseDto<InventoryDataItemDto[]> { Success = false, ErrorMessages = expectedResult }));

            var actualResult = await Sut.UpdateInventoryData();

            Assert.False(string.IsNullOrEmpty(actualResult));
        }

        public static IEnumerable<object[]> GetTestInventory()
        {
            yield return new InventoryDataItemDto[][]
            {
                null
            };

            yield return new InventoryDataItemDto[][]
            {
                new InventoryDataItemDto[0]
            };
            yield return new InventoryDataItemDto[][]
            {
                new InventoryDataItemDto[]{
                    new InventoryDataItemDto()
                }
            };
        }

        [Theory(DisplayName = "UpdateInventoryData() | Microservice success")]
        [MemberData(nameof(GetTestInventory))]
        public async Task UpdateInventory_Success(InventoryDataItemDto[] testResponse)
        {
            Setup<IInventoryUpdateClient, Task<BaseResponseDto<InventoryDataItemDto[]>>>(s => s.GetInventoryItems(It.IsAny<string>()),
                Task.FromResult(new BaseResponseDto<InventoryDataItemDto[]> { Success = true, Payload = testResponse }));

            var actualResult = await Sut.UpdateInventoryData();

            Assert.False(string.IsNullOrEmpty(actualResult));
        }
    }
}
