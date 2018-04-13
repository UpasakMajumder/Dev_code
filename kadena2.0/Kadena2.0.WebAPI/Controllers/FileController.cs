using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.Infrastructure;
using System;
using System.Threading.Tasks;
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
        [Route(Helpers.Routes.File.Get)]
        public async Task<IHttpActionResult> GetFile(string path)
        {
            var link = await _fileService.GetUrlFromS3(path);
            if (string.IsNullOrWhiteSpace(link))
            {
                return this.NotFound();
            }
            return Redirect(link);
        }

    }
}