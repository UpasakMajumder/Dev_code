using AutoMapper;
using Kadena.BusinessLogic.Contracts;
using Kadena.Dto.Logon.Responses;
using Kadena.WebAPI.Infrastructure;
using Kadena.WebAPI.Infrastructure.Filters;
using System;
using System.Web.Http;

namespace Kadena.WebAPI.Controllers
{
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

        [HttpGet]
        [Route("api/user/accepttac")]
        public IHttpActionResult AcceptTaC()
        {
            userService.AcceptTaC();
            return SuccessJson();
        }

        [HttpGet]
        [Route("api/user/checktac")]
        public IHttpActionResult CheckTaC()
        {
            var serviceResult = userService.CheckTaC();
            var result = mapper.Map<CheckTaCResultDTO>(serviceResult);
            return ResponseJson(result);
        }

    }
}