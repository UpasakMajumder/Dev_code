using Kadena.Models.BusinessUnit;
using System.Collections.Generic;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoBusinessUnitsProvider
    {
        List<BusinessUnit> GetBusinessUnits();

        List<BusinessUnit> GetUserBusinessUnits(int userID);

        string GetDistributorBusinessUnit(int distributorID);

        string GetDistributorBusinessUnitNumber(int distributorID);

        string GetBusinessUnitName(long businessUnitNumber);
    }
}
