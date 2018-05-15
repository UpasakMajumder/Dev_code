using AutoMapper;
using Kadena.Dto.MailingList;
using Kadena.Models;
using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.Infrastructure;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using Kadena.WebAPI.Infrastructure.Filters;
using Kadena.Helpers.Routes;

namespace Kadena.WebAPI.Controllers
{
    [CustomerAuthorizationFilter]
    public class KListController : ApiControllerBase
    {
        private readonly IKListService _service;
        private readonly IMapper _mapper;
        private readonly IFileService fileService;

        public KListController(IKListService service, IMapper mapper, IFileService fileService)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }

        [HttpPost]
        [Route(Klist.UseOnlyCorrect)]
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
        [Route(Klist.Update)]
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

        [HttpGet]
        [Route(Klist.Export)]
        public async Task<IHttpActionResult> Export([FromUri] Guid containerId)
        {
            var uri = await fileService.GetContainerFileUrl(containerId);
            if (uri == null)
            {
                return NotFound();
            }
            return Redirect(uri);
        }
    }
}