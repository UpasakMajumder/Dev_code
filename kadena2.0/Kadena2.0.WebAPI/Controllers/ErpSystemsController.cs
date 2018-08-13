using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.Infrastructure;
using System;
using System.Web.Http;
using AutoMapper;
using Kadena.Dto.ErpSystem;
using Kadena.Helpers.Routes;

namespace Kadena.WebAPI.Controllers
{
    public class ErpSystemsController : ApiControllerBase
    {
        private readonly IErpSystemsService _erpSystemsService;
        private readonly IMapper _mapper;

        public ErpSystemsController(IErpSystemsService erpSystemsService, IMapper mapper)
        {
            _erpSystemsService = erpSystemsService ?? throw new ArgumentNullException(nameof(erpSystemsService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [Route(Routes.ErpSystems.Get)]
        public IHttpActionResult GetErpSystems()
        {
            var data = _erpSystemsService.GetAll();
            var result = _mapper.Map<ErpSystemDto[]>(data);

            return ResponseJson(result);
        }

    }
}