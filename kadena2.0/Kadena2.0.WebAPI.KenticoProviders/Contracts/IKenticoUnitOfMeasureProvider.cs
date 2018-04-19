using Kadena.Models.Product;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoUnitOfMeasureProvider
    {
        UnitOfMeasure GetUnitOfMeasure(string name);
        UnitOfMeasure GetUnitOfMeasureByErpCode(string erpCode);
        UnitOfMeasure GetDefaultUnitOfMeasure();
        string GetLocalizedUnitOfMeasure(string codeOfUnit);
    }
}
