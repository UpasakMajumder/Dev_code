using AutoMapper;
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
        [Route("/klist/useonlycorrect/{containerId}")]
        public async Task<IHttpActionResult> UsOnlyCorrect(Guid containerId)
        {
            return ResponseJson(await _service.UseOnlyCorrectAddresses(containerId));
        }
    }
}