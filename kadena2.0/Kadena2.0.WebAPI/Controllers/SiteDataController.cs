using AutoMapper;
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

        [HttpGet]
        [Route("api/site")]
        [IdentityBasicAuthentication]
        public IHttpActionResult GetSiteData(int siteId = 0, string siteName = "")
        {
            var result = _service.GetKenticoSite(siteId, siteName);
            var resultDto = _mapper.Map<SiteDataResponseDto>(result);
            return ResponseJsonCheckingNull(resultDto, "Unable to find site");
        }

        [HttpGet]
        [Route("api/site/{siteId}/artworkftp")]
        [IdentityBasicAuthentication]

        public IHttpActionResult GetArtworkFtpSettings(int siteId)
        {
            var result = _service.GetArtworkFtpSettings(siteId);
            var resultDto = _mapper.Map<ArtworkFtpResponseDto>(result);
            return ResponseJson(result);
        }
    }
}