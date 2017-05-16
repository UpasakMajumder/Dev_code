using System;
using CMS.Base;
using System.Linq;
using CMS.Base.Web.UI;
using CMS.Ecommerce;
using CMS.Ecommerce.Web.UI;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;

/// <summary>
/// Remove button control for shopping cart transformation
/// </summary>
public partial class CMSModules_Ecommerce_Controls_Checkout_RichCartItemRemove : CMSCheckoutWebPart
{
    private ShoppingCartItemInfo mShoppingCartItemInfoObject;
    private CMSAbstractWebPart mShoppingCartContent;
    private readonly EcommerceActivityLogger mActivityLogger = new EcommerceActivityLogger();

    public string ButtonCssClass { get; set; }
    public string ControlLabel { get; set; }

    public string ImageURL { get; set; }

    public string ControlType { get; set; } = "Button";

    public int CartItemID { get; set; }

    /// <summary>
    /// The parent web part, in which transformation the current control is placed.
    /// </summary>
    private CMSAbstractWebPart ShoppingCartContent
    {
        get
        {
            if (mShoppingCartContent == null)
            {
                // Gets the parent control
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
                // Gets the current displayed CartItemInfo by CartItemID                
                mShoppingCartItemInfoObject = ShoppingCart.CartItems.FirstOrDefault(i => i.CartItemID == CartItemID);
            }

            return mShoppingCartItemInfoObject;
        }
        set
        {
            mShoppingCartItemInfoObject = value;
        }
    }




    /// <summary>
    /// Indicates, if the control should be displayed as a button.
    /// </summary>
    public bool IsButton
    {
        get
        {
            return ControlType.ToLowerCSafe() == "button";
        }
    }


    /// <summary>
    /// Indicates, if the control should be displayed as a link.
    /// </summary>
    public bool IsLink
    {
        get
        {
            return ControlType.ToLowerCSafe() == "link";
        }
    }

    /// <summary>
    /// Removes the current cart item and the associated product options from the shopping cart.
    /// </summary>
    protected void Remove(object sender, EventArgs e)
    {
        // Delete all the children from the database if available        
        foreach (ShoppingCartItemInfo scii in ShoppingCart.CartItems)
        {
            if ((scii.CartItemBundleGUID == ShoppingCartItemInfoObject.CartItemGUID) || (scii.CartItemParentGUID == ShoppingCartItemInfoObject.CartItemGUID))
            {
                ShoppingCartItemInfoProvider.DeleteShoppingCartItemInfo(scii);
            }
        }

        // Deletes the CartItem from the database
        ShoppingCartItemInfoProvider.DeleteShoppingCartItemInfo(ShoppingCartItemInfoObject.CartItemGUID);
        // Delete the CartItem form the shopping cart object (session)
        ShoppingCartInfoProvider.RemoveShoppingCartItem(ShoppingCart, ShoppingCartItemInfoObject.CartItemGUID);
        // Log remove item activity
        mActivityLogger.LogProductRemovedFromShoppingCartActivity(ShoppingCartItemInfoObject.SKU, ShoppingCartItemInfoObject.CartItemUnits, ContactID);

        // Recalculate shopping cart
        ShoppingCartInfoProvider.EvaluateShoppingCart(ShoppingCart);

        // Raise the change event for all subscribed web parts
        ComponentEvents.RequestEvents.RaiseEvent(sender, e, SHOPPING_CART_CHANGED);
    }
}
