using Kadena.WebAPI.Models;

namespace Kadena.WebAPI.Contracts
{
    public interface IResourceStringService
    {
        string GetResourceString(string name);
    }
}
