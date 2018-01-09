using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Kadena.WebAPI.Controllers
{
    public class FileController : ApiControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }

        [HttpGet]
        [Route("api/file/get")]
        public async Task<IHttpActionResult> GetFile(string path)
        {
            var link = await _fileService.GetUrlFromS3(path);
            return Redirect(link);
        }

    }
}