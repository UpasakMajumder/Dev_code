using AutoMapper;
using Kadena.BusinessLogic.Contracts;
using Kadena.Dto.BusinessUnits;
using Kadena.WebAPI.Infrastructure;
using Kadena.WebAPI.Infrastructure.Filters;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Web.Http;

namespace Kadena.WebAPI.Controllers
{
    [CustomerAuthorizationFilter]
    public class BusinessUnitsController : ApiControllerBase
    {
        private readonly IBusinessUnitsService businessUnits;
        private readonly IShoppingCartProvider _shoppingCartProvider;
        private readonly IMapper mapper;

        public BusinessUnitsController(IBusinessUnitsService businessUnits, IMapper mapper, IShoppingCartProvider shoppingCartProvider)
        {
            if (businessUnits == null)
            {
                throw new ArgumentNullException(nameof(businessUnits));
            }

            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            if (shoppingCartProvider == null)
            {
                throw new ArgumentNullException(nameof(shoppingCartProvider));
            }
            this.mapper = mapper;
            this.businessUnits = businessUnits;
            _shoppingCartProvider = shoppingCartProvider;
        }

        [HttpGet]
        [Route("api/businessunits")]
        public IHttpActionResult GetAllActiveBusienssUnits()
        {
            var activeBusinessUnits = businessUnits.GetBusinessUnits();
            var activeBusinessUnitsDto = mapper.Map<BusinessUnitDto[]>(activeBusinessUnits);
            return ResponseJson(activeBusinessUnitsDto);
        }

        [HttpGet]
        [Route("api/userbusinessunits/{userID}")]
        public IHttpActionResult GetUserBusinessUnitData(int userID)
        {
            var userBusinessUnits = businessUnits.GetUserBusinessUnits(userID);
            var userBusinessUnitsDto = mapper.Map<BusinessUnitDto[]>(userBusinessUnits);
            return ResponseJson(userBusinessUnitsDto);
        }
    }
}
