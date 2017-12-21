using AutoMapper;
using Kadena.BusinessLogic.Contracts;
using Kadena.Dto.BusinessUnits;
using Kadena.Dto.CustomerData;
using Kadena.WebAPI.Infrastructure;
using Kadena.WebAPI.Infrastructure.Filters;
using System;
using System.Net;
using System.Web.Http;

namespace Kadena.WebAPI.Controllers
{
    [CustomerAuthorizationFilter]
    public class BusinessUnitsController : ApiControllerBase
    {
        private readonly IBusinessUnitsService businessUnits;
        private readonly IMapper mapper;

        public BusinessUnitsController(IBusinessUnitsService businessUnits, IMapper mapper)
        {
            if (businessUnits == null)
            {
                throw new ArgumentNullException(nameof(businessUnits));
            }

            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            this.mapper = mapper;
            this.businessUnits = businessUnits;
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

        [HttpPost]
        [Route("api/distributor/update")]
        public IHttpActionResult UpdateData([FromBody]DistributorDTO request)
        {
            var submitRequest = mapper.Map<DistributorDTO>(request);
            var serviceResponse = businessUnits.UpdateItemQuantity(submitRequest.CartItemId, submitRequest.ItemQuantity);
            return Ok(serviceResponse == "success" ? HttpStatusCode.OK : HttpStatusCode.InternalServerError);
        }
    }
}
