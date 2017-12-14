using AutoMapper;
using Kadena.BusinessLogic.Contracts;
using Kadena.Dto.Campaigns;
using Kadena.WebAPI.Infrastructure;
using Kadena.WebAPI.Infrastructure.Filters;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace Kadena.WebAPI.Controllers
{
    [CustomerAuthorizationFilter]
    public class CampaignsController : ApiControllerBase
    {
        private readonly ICampaignsService campaignsService;
        private readonly IMapper mapper;

        public CampaignsController(ICampaignsService campaignsService, IMapper mapper)
        {
            if (campaignsService == null)
            {
                throw new ArgumentNullException(nameof(campaignsService));
            }

            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            this.mapper = mapper;
            this.campaignsService = campaignsService;
        }

        [HttpDelete]
        [Route("api/campaigns/{CampaignID}")]
        public IHttpActionResult DeleteCampaign(int campaignID)
        {
            campaignsService.DeleteCampaign(campaignID);
            return ResponseJson<string>("OK");
        }

        [HttpGet]
        [Route("api/getCampaigns/{ordertype}")]
        public IHttpActionResult GetCampaigns(string orderType)
        {
            var campaigns = campaignsService.GetCampaigns(orderType);
            var result = mapper.Map<CampaginDto[]>(campaigns);
            return ResponseJson(result);
        }
    }
}
