using CMS.Base.Web.UI;
using CMS.Ecommerce;
using CMS.Ecommerce.Web.UI;
using CMS.EventLog;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using Kadena.Old_App_Code.Kadena.Constants;
using System;
using System.Linq;

namespace Kadena.CMSWebParts.Kadena.Cart
{
    public partial class RemoveItemFromCart : CMSCheckoutWebPart
    {
        #region "Variables"

        private ShoppingCartItemInfo mShoppingCartItemInfoObject;
        private CMSAbstractWebPart mShoppingCartContent;
        private readonly EcommerceActivityLogger mActivityLogger = new EcommerceActivityLogger();
        public ShoppingCartInfo cart;

        #endregion "Variables"

        #region "Private properties"

        /// <summary>
        /// The parent web part, in which transformation the current control is placed.
        /// </summary>
        private CMSAbstractWebPart ShoppingCartContent
        {
            get
            {
                if (mShoppingCartContent == null)
                {
                    mShoppingCartContent = ControlsHelper.GetParentControl<CMSAbstractWebPart>(this);
                }
                return mShoppingCartContent;
            }
        }

        /// <summary>
        /// The current ShoppingCartInfo object on which the transformation is applied.
        /// </summary>
        private ShoppingCartItemInfo ShoppingCartItemInfoObject
        {
            get
            {
                if (mShoppingCartItemInfoObject == null)
                {
                    mShoppingCartItemInfoObject =
                        cart.CartItems.FirstOrDefault(i => i.CartItemID == CurrentCartItemID);
                }
                return mShoppingCartItemInfoObject;
            }
            set { mShoppingCartItemInfoObject = value; }
        }

        #endregion "Private properties"

        #region "Public properties"

        /// <summary>
        /// Distributor Cart ID.
        /// </summary>
        public int CartID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("CartID"), default(int));
            }
            set
            {
                SetValue("CartID", value);
            }
        }

        /// <summary>
        /// Distributor Cart current cart itemID.
        /// </summary>
        public int CurrentCartItemID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("CurrentCartItemID"), default(int));
            }
            set
            {
                SetValue("CurrentCartItemID", value);
            }
        }

        #endregion "Public properties"

        #region "Event handling"

        /// <summary>
        /// Removes the current cart item and the associated product options from the shopping cart.
        /// </summary>
        protected void Remove(object sender, EventArgs e)
        {
            try
            {
                cart = ShoppingCartInfoProvider.GetShoppingCartInfo(CartID);
                foreach (ShoppingCartItemInfo scii in cart.CartItems)
                {
                    if ((scii.CartItemBundleGUID == ShoppingCartItemInfoObject.CartItemGUID) ||
                        (scii.CartItemParentGUID == ShoppingCartItemInfoObject.CartItemGUID))
                    {
                        ShoppingCartItemInfoProvider.DeleteShoppingCartItemInfo(scii);
                    }
                }
                ShoppingCartItemInfoProvider.DeleteShoppingCartItemInfo(ShoppingCartItemInfoObject.CartItemGUID);
                ShoppingCartInfoProvider.RemoveShoppingCartItem(cart, ShoppingCartItemInfoObject.CartItemGUID);
                if (cart.CartItems.Count == 0)
                {
                    ShoppingCartInfoProvider.DeleteShoppingCartInfo(cart.ShoppingCartID);
                }
                mActivityLogger.LogProductRemovedFromShoppingCartActivity(ShoppingCartItemInfoObject.SKU,
                    ShoppingCartItemInfoObject.CartItemUnits, ContactID);
                ShoppingCartInfoProvider.EvaluateShoppingCart(cart);
                ComponentEvents.RequestEvents.RaiseEvent(sender, e, SHOPPING_CART_CHANGED);
                URLHelper.Redirect($"{Request.RawUrl}?status={QueryStringStatus.Deleted}");
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_RemoveItemFromCart", "Remove", ex.Message);
            }
        }

        #endregion "Event handling"

        #region "Page events"

        /// <summary>
        /// Initializes the control.
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                base.OnLoad(e);
                cart = ShoppingCartInfoProvider.GetShoppingCartInfo(CartID);
                if (!StopProcessing)
                {
                    btnRemoveItem.Visible = true;
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_RemoveItemFromCart", "OnLoad", ex.Message);
            }
        }

        /// <summary>
        /// Handle the visibility of the control.
        /// </summary>
        protected override void OnPreRender(EventArgs e)
        {
            try
            {
                base.OnPreRender(e);
                cart = ShoppingCartInfoProvider.GetShoppingCartInfo(CartID);
                bool ItemIsProductOption =
                    (ShoppingCartItemInfoObject != null) && ShoppingCartItemInfoObject.IsProductOption;
                bool CartContentIsReadOnly =
                    ValidationHelper.GetBoolean(ShoppingCartContent.GetValue("ReadOnlyMode"), false);
                btnRemoveItem.Visible = !(ItemIsProductOption || CartContentIsReadOnly);
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_RemoveItemFromCart", "OnPreRender", ex.Message);
            }
        }

        #endregion "Page events"
    }
}