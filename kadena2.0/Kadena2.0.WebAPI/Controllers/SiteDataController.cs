using AutoMapper;
using Kadena.Dto.Site.Requests;
using Kadena.Dto.Site.Responses;
using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.Infrastructure;
using Kadena.WebAPI.Infrastructure.Filters.Authentication;
using System.Web.Http;
using System;

namespace Kadena.WebAPI.Controllers
{
    public class SiteDataController : ApiControllerBase
    {
        private readonly ISiteDataService _service;        
        private readonly IMapper _mapper;

        public SiteDataController(ISiteDataService service, IMapper mapper)
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

        
        [HttpGet]
        [Route("api/site")]
        [IdentityBasicAuthentication]
        public IHttpActionResult GetSiteData( [FromUri]SiteDataRequestDto request)
        {
            var result = _service.GetKenticoSite(request.SiteId, request.SiteName);
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