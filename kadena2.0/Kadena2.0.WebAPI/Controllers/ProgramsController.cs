using AutoMapper;
using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.Infrastructure;
using Kadena.WebAPI.Infrastructure.Filters;
using System;
using System.Web.Http;

namespace Kadena.WebAPI.Controllers
{
    [CustomerAuthorizationFilter]
    public class ProgramsController : ApiControllerBase
    {
        private readonly IProgramsService programService;
        private readonly IMapper mapper;

        public ProgramsController(IProgramsService programService, IMapper mapper)
        {
            if (programService == null)
            {
                throw new ArgumentNullException(nameof(programService));
            }

            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            this.mapper = mapper;
            this.programService = programService;
        }

        [HttpDelete]
        [Route("api/deleteprogram/{programID}")]
        public IHttpActionResult DeleteProgram(int programID)
        {
            programService.DeleteProgram(programID);
            return ResponseJson<string>("OK");
        }
    }
}
