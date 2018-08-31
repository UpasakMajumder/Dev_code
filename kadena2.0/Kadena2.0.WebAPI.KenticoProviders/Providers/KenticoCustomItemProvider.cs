using AutoMapper;
using CMS.CustomTables;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;

namespace Kadena.WebAPI.KenticoProviders.Providers
{
    public class KenticoCustomItemProvider : IKenticoCustomItemProvider
    {
        private readonly IMapper mapper;

        public KenticoCustomItemProvider(IMapper mapper)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public T GetItem<T>(int id, string className)
        {
            var item = CustomTableItemProvider.GetItem(id, className);
            return mapper.Map<T>(item);
        }
    }
}
