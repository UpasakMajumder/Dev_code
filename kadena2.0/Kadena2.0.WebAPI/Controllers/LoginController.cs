using AutoMapper;
using Kadena.WebAPI.Infrastructure;
using System.Web.Http;
using Kadena.Dto.Logon.Requests;
using Kadena.BusinessLogic.Contracts;
using Kadena.Models.Login;
using Kadena.Dto.Logon.Responses;
using System;

namespace Kadena.WebAPI.Controllers
{
    public class LoginController : ApiControllerBase
    {
        private readonly ILoginService service;
        private readonly IMapper mapper;

        public LoginController(ILoginService service, IMapper mapper)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            this.service = service;
            this.mapper = mapper;
        }

        [HttpPost]
        [Route("api/login")]
        public IHttpActionResult Login([FromBody] LogonUserRequestDTO request)
        {
            var loginRequestModel = mapper.Map<LoginRequest>(request);
            var serviceResult = service.Login(loginRequestModel);
            var result = mapper.Map<LogonUserResultDTO>(serviceResult);
            return ResponseJson(result);
        }

        [HttpPost]
        [Route("api/login/checktac")]
        public IHttpActionResult CheckTaC([FromBody] CheckTaCRequestDTO request)
        {
            var loginRequestModel = mapper.Map<LoginRequest>(request);
            var serviceResult = service.CheckTaC(loginRequestModel);
            var result = mapper.Map<CheckTaCResultDTO>(serviceResult);
            return ResponseJson(result);
        }

        
        [HttpPost]
        [Route("api/login/accepttac")]
        public IHttpActionResult AcceptTaC([FromBody] AcceptTaCRequestDTO request)
        {
            var loginRequestModel = mapper.Map<LoginRequest>(request);
            service.AcceptTaC(loginRequestModel);
            return SuccessJson();
        }

        [HttpPost]
        [Route("api/logout")]
        public IHttpActionResult Logout()
        {
            var rerirecturl = service.Logout();
            return ResponseJson(rerirecturl);
        }
    }
}