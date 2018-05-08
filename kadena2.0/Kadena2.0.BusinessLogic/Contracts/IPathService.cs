using Kadena.AmazonFileSystemProvider;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IPathService : IS3PathService
    {
        string EnsureFullKey(string key);
    }
}
