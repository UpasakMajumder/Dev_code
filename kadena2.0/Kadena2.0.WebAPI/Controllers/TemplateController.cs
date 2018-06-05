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
            _templateService = templateService ?? throw new ArgumentNullException(nameof(templateService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost]
        [CustomerAuthorizationFilter]
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
        [Route("api/products/{documentId}/templates")]
        public async Task<IHttpActionResult> GetTemplates(int documentId)
        {
            var result = await _templateService.GetTemplatesByProduct(documentId);
            var resultDto = _mapper.Map<ProductTemplatesDTO>(result);
            return ResponseJson(resultDto);
        }

        [HttpGet]        
        [Route("api/template/{templateId}/preview/{settingId}")]
        public async Task<IHttpActionResult> GetPreview(Guid templateId, Guid settingId)
        {
            var result = await _templateService.GetPreviewUri(templateId, settingId);
            if (result == null)
            {
                return NotFound();
            }
            return Redirect(result);
        }
    }
}