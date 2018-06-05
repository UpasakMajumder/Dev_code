using Kadena.Models.BusinessUnit;
using System.Collections.Generic;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IBusinessUnitsService
    {
        List<BusinessUnit> GetSiteActiveBusinessUnits();

        List<BusinessUnit> GetUserBusinessUnits(int UserID);
    }
}
