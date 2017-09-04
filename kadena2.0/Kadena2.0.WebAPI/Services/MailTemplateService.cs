using Kadena.WebAPI.Contracts;
using AutoMapper;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Models;

namespace Kadena.WebAPI.Services
{
    public class MailTemplateService : IMailTemplateService
    {
        private readonly IMapper mapper;
        private readonly IKenticoResourceService resources;
        private readonly IKenticoSearchService kenticoSearch;
        private readonly IKenticoProviderService kenticoProvider;

        public MailTemplateService(IMapper mapper, IKenticoResourceService resources, IKenticoSearchService kenticoSearch, IKenticoProviderService kenticoProvider)
        {
            this.mapper = mapper;
            this.resources = resources;
            this.kenticoSearch = kenticoSearch;
            this.kenticoProvider = kenticoProvider;
        }

        public MailTemplate GetMailTemplate(string templateId)
        {
            return null;
        }
    }
}
