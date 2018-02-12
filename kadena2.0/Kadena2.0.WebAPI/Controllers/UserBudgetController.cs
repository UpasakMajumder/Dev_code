using AutoMapper;
using Kadena.BusinessLogic.Contracts;
using Kadena.Dto.CustomerData;
using Kadena.WebAPI.Infrastructure;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Kadena.WebAPI.Controllers
{
    public class UserBudgetController : ApiControllerBase
    {
        private readonly IUserBudgetService userBudgetService;
        private readonly IMapper mapper;

        public UserBudgetController(IUserBudgetService userBudgetService, IMapper mapper)
        {
            if (userBudgetService == null)
            {
                throw new ArgumentNullException(nameof(userBudgetService));
            }

            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            this.mapper = mapper;
            this.userBudgetService = userBudgetService;

        }
        [HttpPost]
        [Route("api/userbudget")]
        public string UpdateUserBudget([FromBody]UserBudgetDto request)
        {
            var submitRequest = mapper.Map<UserBudgetDto>(request);
            var serviceResponse = (string)userBudgetService.UpdateUserBudgetAllocation(submitRequest.ItemID, submitRequest.UserBudget);
            return serviceResponse;
        }
    }
}