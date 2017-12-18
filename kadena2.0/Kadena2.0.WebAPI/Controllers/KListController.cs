using AutoMapper;
using Kadena.Dto.MailingList;
using Kadena.Models;
using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.Infrastructure;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using Kadena.WebAPI.Infrastructure.Filters;

namespace Kadena.WebAPI.Controllers
{
    [CustomerAuthorizationFilter]
    public class KListController : ApiControllerBase
    {
        private readonly IKListService _service;
        private readonly IMapper _mapper;

        public KListController(IKListService service, IMapper mapper)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            _service = service;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("klist/useonlycorrect/{containerId}")]
        public async Task<IHttpActionResult> UseOnlyCorrect(Guid containerId)
        {
            var result = await _service.UseOnlyCorrectAddresses(containerId);
            if (result)
            {
                return ResponseJson(result);
            }
            else
            {
                return ErrorJson("Failed request.");
            }
        }

        [HttpPost]
        [Route("klist/update/{containerId}")]
        public async Task<IHttpActionResult> Update([FromUri] Guid containerId, [FromBody] UpdateAddressDto[] addresses)
        {
            var changes = _mapper.Map<MailingAddress[]>(addresses);
            var result = await _service.UpdateAddresses(containerId, changes);
            if (result)
            {
                return ResponseJson(result);
            }
            else
            {
                return ErrorJson("Failed request.");
            }
        }
    }
}