using System;
using System.Threading.Tasks;

namespace Kadena.WebAPI.Contracts
{
    public interface IKListService
    {
        Task<bool> UseOnlyCorrectAddresses(Guid containerId);
    }
}
