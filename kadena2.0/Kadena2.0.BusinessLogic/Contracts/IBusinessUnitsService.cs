using Kadena.Models.BusinessUnit;
using System.Collections.Generic;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IBusinessUnitsService
    {
        List<BusinessUnit> GetBusinessUnits();

        List<BusinessUnit> GetUserBusinessUnits(int UserID);

        bool UpdateItemQuantity(int CartItemID, int quantity);
    }
}
