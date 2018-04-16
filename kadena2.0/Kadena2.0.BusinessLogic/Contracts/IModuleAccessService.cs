using Kadena.Models.ModuleAccess;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IModuleAccessService
    {
        string GetMainNavigationWhereCondition(KadenaModuleState moduleState);
        bool IsAccessible(string moduleName);
    }
}
