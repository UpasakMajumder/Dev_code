using System.Collections.Generic;
using Kadena.Models.ErpSystem;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IErpSystemsService
    {
        IEnumerable<ErpSystem> GetAll();
    }
}
