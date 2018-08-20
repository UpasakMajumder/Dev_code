using System.Collections.Generic;
using Kadena.Models.ErpSystem;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoErpSystemsProvider
    {
        IEnumerable<ErpSystem> GetErpSystems();
    }
}
