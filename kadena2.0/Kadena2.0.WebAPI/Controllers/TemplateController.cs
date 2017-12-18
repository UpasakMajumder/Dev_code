using AutoMapper;
using Kadena.Dto.TemplatedProduct.Responses;
using Kadena.Models.Checkout;
using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.Infrastructure;
using Kadena.WebAPI.Infrastructure.Filters;
using System.Threading.Tasks;
using System.Web.Http;
using System;

namespace Kadena.WebAPI.Controllers
{
    public class TemplateController : ApiControllerBase
    {
        private readonly ITemplateService _templateService;
        private readonly IMapper _mapper;

        public TemplateController(ITemplateService templateService, IMapper mapper)
        {
            if (templateService == null)
            {
                throw new ArgumentNullException(nameof(templateService));
            }
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            _templateService = templateService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("api/template/setname")]
        public async Task<IHttpActionResult> UpdateTemplate([FromBody] NewCartItem item)
        {
            var result = await _templateService.UpdateTemplate(item.TemplateId, item.CustomProductName, item.Quantity);
            if (result)
            {
                return ResponseJson(result);
            }
            else
            {
                return ErrorJson("Failed request. See logs for more information.");
            }
        }

        [HttpGet]
        [CustomerAuthorizationFilter]
        [Route("api/products/{nodeId}/templates")]
        public async Task<IHttpActionResult> GetTemplates(int nodeId)
        {
            var result = await _templateService.GetTemplatesByProduct(nodeId);
            var resultDto = _mapper.Map<ProductTemplatesDTO>(result);
            return ResponseJson(resultDto);
        }
    }
}