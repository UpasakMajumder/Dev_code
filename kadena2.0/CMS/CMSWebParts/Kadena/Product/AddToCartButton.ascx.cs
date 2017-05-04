using CMS.DocumentEngine;
using CMS.Ecommerce;
using CMS.EventLog;
using CMS.Helpers;
using CMS.Localization;
using CMS.PortalEngine.Web.UI;
using Kadena.Old_App_Code.Kadena.DynamicPricing;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Kadena.CMSWebParts.Kadena.Product
{
  public partial class AddToCartButton : CMSAbstractWebPart
  {
    #region Public methods

    public override void OnContentLoaded()
    {
      base.OnContentLoaded();
      SetupControl();
    }

    protected void SetupControl()
    {
      if (!StopProcessing)
      {
        SetupNumberOfItemsInPackageInformation();

        btnAddToCart.Text = ResHelper.GetString("Kadena.Product.AddToCart", LocalizationContext.CurrentCulture.CultureCode);
        lblNumberOfItemsError.Visible = false;
        inpNumberOfItems.Attributes.Add("class", "input__text");
      }
    }

    #endregion

    #region Event handlers

    protected void btnAddToCart_Click(object sender, EventArgs e)
    {
      var previouslyAddedAmmount = GetPreviouslyAddedAmmout();

      if (IsAddedAmmountValid(ValidationHelper.GetInteger(inpNumberOfItems.Value, 0) + previouslyAddedAmmount))
      {
        AddItemsToShoppingCart(ValidationHelper.GetInteger(inpNumberOfItems.Value, 0), previouslyAddedAmmount);

        // redirect
      }
      else
      {
        lblNumberOfItemsError.Text = ResHelper.GetString("Kadena.Product.InsertedAmmountValueIsNotValid", LocalizationContext.CurrentCulture.CultureCode);
        lblNumberOfItemsError.Visible = true;
        inpNumberOfItems.Attributes.Add("class", "input__text input--error");
      }
    }

    #endregion

    #region Private methods

    private void SetupNumberOfItemsInPackageInformation()
    {
      if (DocumentContext.CurrentDocument.GetIntegerValue("ProductNumberOfItemsInPackage", 0) == 0 ||
        DocumentContext.CurrentDocument.GetIntegerValue("ProductNumberOfItemsInPackage", 0) == 1)
      {
        lblNumberOfItemsInPackageInfo.Visible = false;
      }
      else
      {
        lblNumberOfItemsInPackageInfo.Text = string.Format(ResHelper.GetString("Kadena.Product.NumberOfItemsInPackagesFormatString2", LocalizationContext.CurrentCulture.CultureCode), DocumentContext.CurrentDocument.GetIntegerValue("ProductNumberOfItemsInPackage", 0));
      }
    }

    private bool IsAddedAmmountValid(int ammount)
    {
      // is inserted value valid positive integer number?
      if (ammount > 0)
      {
        var rawData = new JavaScriptSerializer().Deserialize<List<DynamicPricingRawData>>(DocumentContext.CurrentDocument.GetStringValue("ProductDynamicPricing", string.Empty));
        // do I have dynamic pricing data or I am using regular SKU price?
        if (rawData != null && rawData.Count != 0)
        {
          List<DynamicPricingData> data;
          // is JSON convertable to dynamic numbers?
          if (new DynamicPricingDataHelper().ConvertDynamicPricingData(rawData, out data))
          {
            foreach (var item in data)
            {
              if (ammount >= item.Min && ammount <= item.Max)
              {
                return true;
              }
            }
          }
          else
          {
            EventLogProvider.LogEvent("E", "Add to cart button", "Dynamic pricing data", "Dynamic pricing data couldn't be restored");
            return false;
          }
        }
        else
        {
          return true;
        }
      }
      return false;
    }

    private void AddItemsToShoppingCart(int ammount, int previouslyAddedAmmount)
    {
      var product = SKUInfoProvider.GetSKUInfo(DocumentContext.CurrentDocument.GetIntegerValue("SKUID", 0));

      if (product != null)
      {
        var cart = ECommerceContext.CurrentShoppingCart;
        ShoppingCartInfoProvider.SetShoppingCartInfo(cart);

        var parameters = new ShoppingCartItemParameters(product.SKUID, ammount);
        var cartItem = cart.SetShoppingCartItem(parameters);

        var dynamicUnitPrice = GetUnitPriceForAmmount(ammount + previouslyAddedAmmount);
        if (dynamicUnitPrice > 0)
        {
          cartItem.CartItemPrice = dynamicUnitPrice;
        }
        ShoppingCartItemInfoProvider.SetShoppingCartItemInfo(cartItem);
      }
    }

    private double GetUnitPriceForAmmount(int ammount)
    {
      var rawData = new JavaScriptSerializer().Deserialize<List<DynamicPricingRawData>>(DocumentContext.CurrentDocument.GetStringValue("ProductDynamicPricing", string.Empty));

      if (rawData == null || rawData.Count == 0)
      {
        return DocumentContext.CurrentDocument.GetDoubleValue("SKUPrice", 0);
      }
      else
      {
        List<DynamicPricingData> data;
        if (new DynamicPricingDataHelper().ConvertDynamicPricingData(rawData, out data))
        {
          foreach (var item in data)
          {
            if (ammount >= item.Min && ammount <= item.Max)
            {
              return decimal.ToDouble(item.Price);
            }
          }
        }
      }
      return 0;
    }

    private int GetPreviouslyAddedAmmout()
    {
      var cart = ECommerceContext.CurrentShoppingCart;
      var currentSKUID = DocumentContext.CurrentDocument.GetIntegerValue("SKUID", 0);

      foreach (var item in cart.CartItems)
      {
        if (item.SKUID == currentSKUID)
        {
          return item.CartItemUnits;
        }
      }
      return 0;
    }

    #endregion
  }
}