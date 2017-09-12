using AutoMapper;
using Kadena.Dto.Site.Requests;
using Kadena.Dto.Site.Responses;
using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.Infrastructure;
using Kadena.WebAPI.Infrastructure.Filters.Authentication;
using System.Web.Http;

namespace Kadena.WebAPI.Controllers
{
    public class SiteDataController : ApiControllerBase
    {
        private readonly ISiteDataService _service;
        private readonly IMapper _mapper;

        public SiteDataController(ISiteDataService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("api/sitedata/ordermanageremail")]
        [IdentityBasicAuthentication]
        public IHttpActionResult GetOrderManagerEmail([FromBody]SiteDataRequestDto request)
        {
            var result = _service.GetOrderInfoRecepients(request.SiteId);
            return ResponseJson(result);
        }

        [HttpPost]
        [Route("api/sitedata/artworkftp")]
        [IdentityBasicAuthentication]

        public IHttpActionResult GetArtworkFtpSettings([FromBody]ArtworkFtpRequestDto request)
        {
            var result = _service.GetArtworkFtpSettings(request.SiteId);
            var resultDto = _mapper.Map<ArtworkFtpResponseDto>(result);
            return ResponseJson(result);
        }
    }
}