using AutoMapper;
using Kadena.Dto.MailingList;
using Kadena.Models;
using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.Infrastructure;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace Kadena.WebAPI.Controllers
{
    public class KListController : ApiControllerBase
    {
        private readonly IKListService _service;
        private readonly IMapper _mapper;

        public KListController(IKListService service, IMapper mapper)
        {
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
                return ErrorJson("Failed request.")
            }
        }
    }
}