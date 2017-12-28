using Kadena.BusinessLogic.Contracts;
using System.Web.Http;
using System;
using AutoMapper;
using Kadena.WebAPI.Infrastructure;
using Kadena.WebAPI.Infrastructure.Filters;
using System.Net.Http;
using System.Linq;
using Kadena.Dto.Search.Responses;

namespace Kadena.WebAPI.Controllers
{
    [CustomerAuthorizationFilter]
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
        [QuerystringParameterRequired("phrase")]
        public IHttpActionResult Search()
        {
            var phrase = this.Request.GetQueryNameValuePairs().First(p => p.Key == "phrase").Value;
            var serpPage = service.Search(phrase);
            var serpPageDto = mapper.Map<SearchResultPageResponseDTO>(serpPage);
            return ResponseJson(serpPageDto); 
        }

        [HttpPost]
        [Route("api/autocomplete")]
        [QuerystringParameterRequired("phrase")]
        public IHttpActionResult Autocomplete()
        {
            var phrase = this.Request.GetQueryNameValuePairs().First(p => p.Key == "phrase").Value;
            var result = service.Autocomplete(phrase);
            var resultDto = mapper.Map<AutocompleteResponseDTO>(result);
            return ResponseJson(resultDto);
        }
    }
}
