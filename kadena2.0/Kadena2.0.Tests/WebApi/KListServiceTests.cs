using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kadena.Dto.MailingList.MicroserviceResponses;
using Kadena.WebAPI;
using Kadena.BusinessLogic.Services;
using AutoMapper;
using Kadena2.MicroserviceClients.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Dto.General;
using Kadena.Models.Site;

namespace Kadena.Tests.WebApi
{
    public class KListServiceTests
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

        private KListService Create(Mock<IMailingListClient> mailingClient = null, Mock<IAddressValidationClient> validationClient = null)
        {
            MapperBuilder.InitializeAll();
            var mapper = Mapper.Instance;


            var kenticoClient = new Mock<IKenticoResourceService>();
            kenticoClient.Setup(p => p.GetKenticoSite())
                .Returns(new KenticoSite());

            return new KListService(mailingClient?.Object ?? new Mock<IMailingListClient>().Object,
                kenticoClient.Object,
                validationClient?.Object ?? new Mock<IAddressValidationClient>().Object,
                mapper);
        }

        private BaseResponseDto<IEnumerable<MailingAddressDto>> GetAddresses()
        {
            return new BaseResponseDto<IEnumerable<MailingAddressDto>>
            {
                Success = true,
                Payload = _addresses.Where(a => a.ContainerId == _containerId)
            };
        }

        private BaseResponseDto<string> ValidateSuccess()
        {
            return new BaseResponseDto<string>
            {
                Success = true,
                Payload = string.Empty
            };
        }

        private BaseResponseDto<string> ValidateFailed()
        {
            return new BaseResponseDto<string>
            {
                Success = false,
                Payload = string.Empty,
                ErrorMessages = "Some error."
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

        [Fact(DisplayName = "UseOnlyCorrectTestSuccess")]
        public async Task UseOnlyCorrectTestSuccess()
        {
            var mailingClient = new Mock<IMailingListClient>();
            var validationClient = new Mock<IAddressValidationClient>();
            mailingClient
                .Setup(c => c.GetAddresses(_containerId))
                .Returns(Task.FromResult(GetAddresses()));
            validationClient
                .Setup(c => c.Validate(_containerId))
                .Returns(Task.FromResult(ValidateSuccess()));
            var srvs = Create(mailingClient, validationClient);
            var result = await srvs.UseOnlyCorrectAddresses(_containerId);

            Assert.True(result);
        }

        [Fact(DisplayName = "UseOnlyCorrectTestValidationFailed")]
        public async Task UseOnlyCorrectTestValidationFailed()
        {
            var mailingClient = new Mock<IMailingListClient>();
            var validationClient = new Mock<IAddressValidationClient>();
            mailingClient
                .Setup(c => c.GetAddresses(_containerId))
                .Returns(Task.FromResult(GetAddresses()));
            validationClient
                .Setup(c => c.Validate(_containerId))
                .Returns(Task.FromResult(ValidateFailed()));
            var srvs = Create(mailingClient, validationClient);
            var result = await srvs.UseOnlyCorrectAddresses(_containerId);

            Assert.False(result);
        }

        [Fact(DisplayName = "UseOnlyCorrectTestEmptyAddresses")]
        public void UseOnlyCorrectTestEmptyAddresses()
        {
            var mailingClient = new Mock<IMailingListClient>();
            mailingClient
                .Setup(c => c.GetAddresses(_containerId))
                .Returns(Task.FromResult((BaseResponseDto<IEnumerable<MailingAddressDto>>)null));
            var srvs = Create(mailingClient);
            Assert.ThrowsAsync<NullReferenceException>(() => srvs.UseOnlyCorrectAddresses(_containerId));

        }

        [Fact(DisplayName = "UpdateTestSuccess")]
        public async Task UpdateTestSuccess()
        {
            var mailingClient = new Mock<IMailingListClient>();
            var validationClient = new Mock<IAddressValidationClient>();
            mailingClient
                .Setup(c => c.UpdateAddresses(_containerId, null))
                .Returns(Task.FromResult(UpdateSuccess()));
            validationClient
                .Setup(c => c.Validate(_containerId))
                .Returns(Task.FromResult(ValidateSuccess()));
            var srvs = Create(mailingClient, validationClient);
            var result = await srvs.UpdateAddresses(_containerId, null);

            Assert.True(result);
        }

        [Fact(DisplayName = "UpdateTestValidationFailed")]
        public async Task UpdateTestValidationFailed()
        {
            var mailingClient = new Mock<IMailingListClient>();
            var validationClient = new Mock<IAddressValidationClient>();
            mailingClient
                .Setup(c => c.UpdateAddresses(_containerId, null))
                .Returns(Task.FromResult(UpdateSuccess()));
            validationClient
                .Setup(c => c.Validate(_containerId))
                .Returns(Task.FromResult(ValidateFailed()));
            var srvs = Create(mailingClient, validationClient);
            var result = await srvs.UpdateAddresses(_containerId, null);

            Assert.False(result);
        }

        [Fact(DisplayName = "UpdateTestUpdateFailed")]
        public async Task UpdateTestUpdateFailed()
        {
            var mailingClient = new Mock<IMailingListClient>();
            mailingClient
                .Setup(c => c.UpdateAddresses(_containerId, null))
                .Returns(Task.FromResult(UpdateFailed()));
            var srvs = Create(mailingClient);
            var result = await srvs.UpdateAddresses(_containerId, null);

            Assert.False(result);
        }

        [Fact(DisplayName = "UpdateTestEmptyChanges")]
        public void UpdateTestEmptyChanges()
        {
            var srvs = Create();
            Assert.ThrowsAsync<NullReferenceException>(() => srvs.UpdateAddresses(_containerId, null));
        }
    }
}
