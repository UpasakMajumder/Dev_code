using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System.Collections.Generic;
using Kadena.Models.BusinessUnit;
using System;

namespace Kadena.BusinessLogic.Services
{
    public class BusinessUnitService : IBusinessUnitsService
    {
        private readonly IKenticoBusinessUnitsProvider kenticoBusinessUnits;

        public BusinessUnitService(IKenticoBusinessUnitsProvider kenticoBusinessUnits)
        {
            if (kenticoBusinessUnits == null)
            {
                throw new ArgumentNullException(nameof(kenticoBusinessUnits));
            }
            this.kenticoBusinessUnits = kenticoBusinessUnits;
        }

        public List<BusinessUnit> GetBusinessUnits()
        {
            return kenticoBusinessUnits.GetBusinessUnits();
        }

        public List<BusinessUnit> GetUserBusinessUnits(int UserID)
        {
            return kenticoBusinessUnits.GetUserBusinessUnits(UserID);
        }
    }
}
