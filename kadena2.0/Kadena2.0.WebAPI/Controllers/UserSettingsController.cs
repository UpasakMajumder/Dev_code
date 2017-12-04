using AutoMapper;
using Kadena.Dto.Settings;
using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.Infrastructure;
using System.Web.Http;
using Kadena.Models;
using Kadena.WebAPI.Infrastructure.Filters;

namespace Kadena.WebAPI.Controllers
{
    public class UserSettingsController : ApiControllerBase
    {
        private readonly ISettingsService _service;
        private readonly IMapper _mapper;

        public UserSettingsController(ISettingsService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("api/usersettings")]
        [CustomerAuthorizationFilter]
        public IHttpActionResult Get()
        {
            var addresses = _service.GetAddresses();
            var result = _mapper.Map<SettingsAddressesDto>(addresses);
            return ResponseJson(result);
        }

        [HttpPost]
        [Route("api/usersettings/saveshippingaddress")]
        [CustomerAuthorizationFilter]
        public IHttpActionResult SaveShippingAddress([FromBody] AddressDto address)
        {
            var addressModel = _mapper.Map<DeliveryAddress>(address);
            _service.SaveShippingAddress(addressModel);
            var result = _mapper.Map<IdDto>(addressModel);
            return ResponseJson(result);
        }

        [HttpPut]
        [Route("api/usersettings/setdefaultshippingaddress")]
        [CustomerAuthorizationFilter]
        public IHttpActionResult SetDefaultShippingAddress([FromBody] DefaultAddressDto address)
        {
            _service.SetDefaultShippingAddress(address.Id);
            return SuccessJson();
        }

        [HttpPut]
        [Route("api/usersettings/unsetdefaultshippingaddress")]
        [CustomerAuthorizationFilter]
        public IHttpActionResult UnsetDefaultShippingAddress()
        {
            _service.UnsetDefaultShippingAddress();
            return SuccessJson();
        }

        [HttpPut]
        [Route("api/usersettings/savelocalization")]
        public IHttpActionResult SaveLocalization([FromBody] LocalizationDto localization)
        {
            var language = _mapper.Map<string>(localization);
            var result = _service.SaveLocalization(language);
            return ResponseJson(result);
        }
    }
}