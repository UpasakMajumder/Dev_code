using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.WebAPI.KenticoProviders;
using System.Collections.Generic;
using Kadena.Models.BusinessUnit;
using System;

namespace Kadena.BusinessLogic.Services
{
    public class BusinessUnitService : IBusinessUnitsService
    {
        private readonly IKenticoBusinessUnitsProvider kenticoBusinessUnits;
        private readonly IShoppingCartProvider _shoppingCartProvider;

        public BusinessUnitService(IKenticoBusinessUnitsProvider kenticoBusinessUnits, IShoppingCartProvider shoppingCartProvider)
        {
            if (kenticoBusinessUnits == null)
            {
                throw new ArgumentNullException(nameof(kenticoBusinessUnits));
            }

            if (shoppingCartProvider == null)
            {
                throw new ArgumentNullException(nameof(shoppingCartProvider));
            }
            this.kenticoBusinessUnits = kenticoBusinessUnits;
            _shoppingCartProvider = shoppingCartProvider;
        }

        public List<BusinessUnit> GetBusinessUnits()
        {
            return kenticoBusinessUnits.GetBusinessUnits();
        }

        public List<BusinessUnit> GetUserBusinessUnits(int UserID)
        {
            return kenticoBusinessUnits.GetUserBusinessUnits(UserID);
        }

        public bool UpdateItemQuantity(int cartItemID, int quantity)
        {
            if (cartItemID != default(int) && quantity != default(int))
            {
                return _shoppingCartProvider.UpdateCartQuantity(cartItemID, quantity);
            }
            else
            {
                return false;
            }
        }
    }
}
