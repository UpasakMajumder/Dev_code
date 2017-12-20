using AutoMapper;
using Kadena.BusinessLogic.Contracts;
using Kadena.Dto.CustomerData;
using Kadena.WebAPI.Infrastructure.Filters;
using System;
using System.Web.Http;

namespace Kadena.WebAPI.Controllers
{
    [CustomerAuthorizationFilter]
    public class DistributorController : ApiController
    {
        private readonly IDistributorService _distributor;
        private readonly IMapper _mapper;

        public DistributorController(IDistributorService distributor, IMapper mapper)
        {
            if (distributor == null)
            {
                throw new ArgumentNullException(nameof(distributor));
            }
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }
            this._mapper = mapper;
            this._distributor = distributor;
        }
        [HttpPost]
        [Route("api/distributor/update/{cartID}")]
        public IHttpActionResult UpdateData(int cartID)
        {
            var qunatity = 36;
            var serviceResponse = _distributor.UpdateItemQuantity(cartID, qunatity);
            return Ok(serviceResponse);
        }
        [HttpPost]
        [Route("api/distributor/update")]
        public IHttpActionResult UpdateDatas([FromBody]DistributorDTO request)
        {
            var submitRequest = _mapper.Map<DistributorDTO>(request);
            var serviceResponse = _distributor.UpdateItemQuantity(submitRequest.CartItemId, submitRequest.ItemQuantity);
            return Ok(serviceResponse);
        }
    }
}
