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
        private readonly ILoginService loginService;
        private readonly IMapper mapper;
        private readonly IIdentityService identityService;
        private readonly IUserService userService;

        public LoginController(ILoginService loginService, IMapper mapper, IIdentityService identityService, IUserService userService)
        {
            this.loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpPost]
        [Route("api/login")]
        public IHttpActionResult Login([FromBody] LogonUserRequestDTO request)
        {
            var loginRequestModel = mapper.Map<LoginRequest>(request);
            var serviceResult = loginService.Login(loginRequestModel);
            var result = mapper.Map<LogonUserResultDTO>(serviceResult);
            return ResponseJson(result);
        }

        [HttpPost]
        [Route("api/login/checktac")]
        public IHttpActionResult CheckTaC([FromBody] CheckTaCRequestDTO request)
        {
            var loginRequestModel = mapper.Map<LoginRequest>(request);
            var serviceResult = userService.CheckTaC(loginRequestModel);
            var result = mapper.Map<CheckTaCResultDTO>(serviceResult);
            return ResponseJson(result);
        }

        [HttpPost]
        [Route("api/logout")]
        public IHttpActionResult Logout()
        {
            var rerirecturl = loginService.Logout();
            return ResponseJson(rerirecturl);
        }

        [HttpPost]
        [Route("api/login/saml2")]
        public IHttpActionResult LoginSaml2([FromBody] SamlAuthenticationDto request)
        {
            var authenticationResultPage = identityService.TryAuthenticate(request.SAMLResponse);
            if (authenticationResultPage == null)
            {
                return NotFound();
            }
            return Redirect(authenticationResultPage);
        }
    }
}