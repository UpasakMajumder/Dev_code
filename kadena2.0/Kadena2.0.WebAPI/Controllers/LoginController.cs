using AutoMapper;
using Kadena.Dto.Settings;
using Kadena.WebAPI.Infrastructure;
using System.Web.Http;
using Kadena.Models;
using Kadena.Dto.Logon.Requests;

namespace Kadena.WebAPI.Controllers
{
    public class LoginController : ApiControllerBase
    {
        private readonly ILoginService _service;
        private readonly IMapper _mapper;

        public LoginController(ILoginService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("api/login")]
        public IHttpActionResult Login([FromBody] LogonUserRequestDTO request)
        {
            var addressModel = _mapper.Map<DeliveryAddress>(address);
            _service.SaveShippingAddress(addressModel);
            var result = _mapper.Map<IdDto>(addressModel);
            return ResponseJson(result);
        }
    }
}