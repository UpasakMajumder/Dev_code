using Kadena.WebAPI.Contracts;
using System.Web.Http;
using System;
using AutoMapper;
using Kadena.WebAPI.Infrastructure;
using System.Threading.Tasks;
using Kadena.WebAPI.Infrastructure.Filters;
using System.Net.Http;
using System.Linq;

namespace Kadena.WebAPI.Controllers
{
    public class SearchController : ApiControllerBase
    {
        private readonly IShoppingCartService service;
        private readonly IMapper mapper;

        public SearchController(IShoppingCartService service, IMapper mapper)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            this.service = service;
            this.mapper = mapper;
        }


        [HttpGet]
        [Route("api/search")]
        [AuthorizationFilter]
        [QuerystringParameterRequired("phrase")]
        public async Task<IHttpActionResult> Search()
        {
            var phrase = this.Request.GetQueryNameValuePairs().First(p => p.Key == "phrase").Value;
            /*var detailPage = await service.GetOrderDetail(orderId);
            var detailPageDto = mapper.Map<OrderDetailDTO>(detailPage);
            return ResponseJson(detailPageDto); */
            return null;
        }

    }
}
