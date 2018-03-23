using AutoMapper;
using Kadena.BusinessLogic.Contracts;
using Kadena.Dto.Logon.Requests;
using Kadena.Dto.Logon.Responses;
using Kadena.Models.Login;
using Kadena.WebAPI.Infrastructure;
using Kadena.WebAPI.Infrastructure.Filters;
using System;
using System.Web.Http;

namespace Kadena.WebAPI.Controllers
{
    [Route("api/user")]
    [CustomerAuthorizationFilter]
    public class UserController : ApiControllerBase
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost]
        [Route("accepttac")]
        public IHttpActionResult AcceptTaC()
        {
            userService.AcceptTaC();
            return SuccessJson();
        }

        [HttpPost]
        [Route("checktac")]
        public IHttpActionResult CheckTaC([FromBody] CheckTaCRequestDTO request)
        {
            var loginRequestModel = mapper.Map<LoginRequest>(request);
            var serviceResult = userService.CheckTaC(loginRequestModel);
            var result = mapper.Map<CheckTaCResultDTO>(serviceResult);
            return ResponseJson(result);
        }

    }
}