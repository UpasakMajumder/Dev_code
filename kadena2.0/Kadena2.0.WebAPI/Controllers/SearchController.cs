using Kadena.WebAPI.Contracts;
using System.Web.Http;
using System;
using AutoMapper;
using Kadena.WebAPI.Infrastructure;
using System.Threading.Tasks;
using Kadena.WebAPI.Infrastructure.Filters;
using System.Net.Http;
using System.Linq;
using Kadena.Dto.Search.Responses;

namespace Kadena.WebAPI.Controllers
{
    public class SearchController : ApiControllerBase
    {
        private readonly ISearchService service;
        private readonly IMapper mapper;

        public SearchController(ISearchService service, IMapper mapper)
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
        public IHttpActionResult Search()
        {
            var phrase = this.Request.GetQueryNameValuePairs().First(p => p.Key == "phrase").Value;
            var serpPage = service.Search(phrase);
            var serpPageDto = mapper.Map<SearchResultPageResponseDTO>(serpPage);
            return ResponseJson(serpPageDto); 
        }
    }
}
