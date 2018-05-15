using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kadena.Dto.MailingList.MicroserviceResponses;
using Kadena.BusinessLogic.Services;
using Kadena2.MicroserviceClients.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Dto.General;
using Kadena.Models.Site;
using Kadena.Container.Default;
using Moq;

namespace Kadena.Tests.BusinessLogic
{
    public class KListServiceTests : KadenaUnitTest<KListService>
    {
        private readonly Guid _containerId;
        private readonly List<MailingAddressDto> _addresses;

        public KListServiceTests()
        {
            _containerId = Guid.NewGuid();
            var count = new Random().Next(100);
            _addresses = new List<MailingAddressDto>();
            for (int i = 0; i < count; i++)
            {
                _addresses.Add(new MailingAddressDto
                {
                    Id = Guid.NewGuid(),
                    FirstName = $"Name{i}",
                    ContainerId = _containerId,
                    Address1 = $"Address 1, {i}",
                    Address2 = $"Address 2, {i}",
                    City = $"City {i}",
                    State = $"State {i}",
                    Zip = $"Zip {i}",
                    ErrorMessage = i % 2 == 0 ? $"Some error {i}" : null
                });
            }
        }

        private void SetupBase()
        {
            Use(MapperBuilder.MapperInstance);
            Setup<IKenticoSiteProvider, KenticoSite>(p => p.GetKenticoSite(), new KenticoSite());
        }

        private BaseResponseDto<IEnumerable<MailingAddressDto>> GetAddresses()
        {
            return new BaseResponseDto<IEnumerable<MailingAddressDto>>
            {
                Success = true,
                Payload = _addresses.Where(a => a.ContainerId == _containerId)
            };
        }

        private BaseResponseDto<IEnumerable<string>> UpdateSuccess()
        {
            return new BaseResponseDto<IEnumerable<string>>
            {
                Success = true,
                Payload = new string[0]
            };
        }

        private BaseResponseDto<IEnumerable<string>> UpdateFailed()
        {
            return new BaseResponseDto<IEnumerable<string>>
            {
                Success = false,
                Payload = new string[0],
                ErrorMessages = "Some error."
            };
        }

        [Fact(DisplayName = "KListService.UseOnlyCorrectAddresses() | Success")]
        public async Task UseOnlyCorrectTestSuccess()
        {
            Setup<IMailingListClient, Task<BaseResponseDto<IEnumerable<MailingAddressDto>>>>(c => c.GetAddresses(_containerId), Task.FromResult(GetAddresses()));
            Setup<IMailingListClient, Task<BaseResponseDto<object>>>(c => c.RemoveAddresses(_containerId, It.IsAny<IEnumerable<Guid>>()), Task.FromResult(new BaseResponseDto<object> { Success = true }));
            SetupBase();

            var actualResult = await Sut.UseOnlyCorrectAddresses(_containerId);

            Assert.True(actualResult);
        }

        [Fact(DisplayName = "KListService.UseOnlyCorrectAddresses() | Zero addresses in container")]
        public async Task UseOnlyCorrectTestEmptyAddresses()
        {
            Setup<IMailingListClient, Task<BaseResponseDto<IEnumerable<MailingAddressDto>>>>(c => c.GetAddresses(_containerId)
                , Task.FromResult((BaseResponseDto<IEnumerable<MailingAddressDto>>)null));
            SetupBase();

            Task action() => Sut.UseOnlyCorrectAddresses(_containerId);

            await Assert.ThrowsAsync<NullReferenceException>(action);
        }

        [Fact(DisplayName = "KListService.UpdateAddresses() | Success")]
        public async Task UpdateTestSuccess()
        {
            Setup<IMailingListClient, Task<BaseResponseDto<IEnumerable<string>>>>(c => c.UpdateAddresses(_containerId, null), Task.FromResult(UpdateSuccess()));
            SetupBase();

            var actualResult = await Sut.UpdateAddresses(_containerId, null);

            Assert.True(actualResult);
        }

        [Fact(DisplayName = "KListService.UpdateAddresses() | Update failed")]
        public async Task UpdateTestUpdateFailed()
        {
            Setup<IMailingListClient, Task<BaseResponseDto<IEnumerable<string>>>>(c => c.UpdateAddresses(_containerId, null), Task.FromResult(UpdateFailed()));
            SetupBase();

            var actualResult = await Sut.UpdateAddresses(_containerId, null);

            Assert.False(actualResult);
        }

        [Fact(DisplayName = "KListService.UpdateAddresses() | Empty changes")]
        public async Task UpdateTestEmptyChanges()
        {
            SetupBase();

            Task action() => Sut.UpdateAddresses(_containerId, null);

            await Assert.ThrowsAsync<NullReferenceException>(action);
        }
    }
}
