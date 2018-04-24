using Kadena.Models.Product;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoUnitOfMeasureProvider
    {
        UnitOfMeasure GetUnitOfMeasure(string name);
        UnitOfMeasure GetUnitOfMeasureByCode(string erpCode);
        UnitOfMeasure GetDefaultUnitOfMeasure();
        string GetDisplaynameByCode(string erpCode);
        string GetDisplayname(string unitName);
    }
}
