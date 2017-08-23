using System;
using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;
using System.Threading.Tasks;

namespace Kadena.WebAPI.Services
{
    public class TemplateService : ITemplateService
    {
        private readonly IKenticoResourceService _resources;
        private readonly IKenticoLogger _logger;
        private readonly ITemplatedProductService _templateClient;

        public TemplateService(IKenticoResourceService resources, IKenticoLogger logger, ITemplatedProductService templateClient)
        {
            _resources = resources;
            _logger = logger;
            _templateClient = templateClient;
        }

        public async Task<bool> SetName(Guid templateId, string name)
        {
            string endpoint = _resources.GetSettingsKey("KDA_TemplatingServiceEndpoint");
            var result = await _templateClient.SetName(endpoint, templateId, name);
            if (!result.Success)
            {
                _logger.LogError("Template set name", result.ErrorMessages);
            }
            return result.Success;
        }
    }
}