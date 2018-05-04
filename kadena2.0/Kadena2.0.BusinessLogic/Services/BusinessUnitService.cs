using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System.Collections.Generic;
using Kadena.Models.BusinessUnit;
using System;
using System.Linq;

namespace Kadena.BusinessLogic.Services
{
    public class BusinessUnitService : IBusinessUnitsService
    {
        private readonly IKenticoBusinessUnitsProvider kenticoBusinessUnits;
        private readonly IShoppingCartProvider _shoppingCartProvider;

        public BusinessUnitService(IKenticoBusinessUnitsProvider kenticoBusinessUnits, IShoppingCartProvider shoppingCartProvider)
        {
            this.kenticoBusinessUnits = kenticoBusinessUnits ?? throw new ArgumentNullException(nameof(kenticoBusinessUnits));
            _shoppingCartProvider = shoppingCartProvider ?? throw new ArgumentNullException(nameof(shoppingCartProvider));
        }

        public List<BusinessUnit> GetSiteActiveBusinessUnits()
        {
            return kenticoBusinessUnits
                .GetBusinessUnits()?
                .OrderBy(bu => bu.BusinessUnitNumber)
                .ToList();
        }

        public List<BusinessUnit> GetUserBusinessUnits(int UserID)
        {
            return kenticoBusinessUnits.GetUserBusinessUnits(UserID);
        }
    }
}
