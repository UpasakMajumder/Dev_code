using Kadena.Models.Site;
using System.Collections.Generic;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoModuleMappingProvider
    {
        IEnumerable<ModuleMapping> Get();
    }
}
