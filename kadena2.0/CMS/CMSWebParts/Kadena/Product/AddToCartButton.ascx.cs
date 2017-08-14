using CMS.Base.Web.UI;
using CMS.DocumentEngine;
using CMS.Ecommerce;
using CMS.EventLog;
using CMS.Helpers;
using CMS.Localization;
using CMS.PortalEngine.Web.UI;
using Kadena.Old_App_Code.Kadena.DynamicPricing;
using System;
using System.Collections.Generic;
using System.Linq;
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
                //SetupNumberOfItemsInPackageInformation();

                //lblNumberOfItemsError.Visible = false;
                //inpNumberOfItems.Attributes.Add("class", "input__text");

                PreviouslyAddedAmount = GetPreviouslyAddedAmmout();

                if (IsStockEmpty() && IsProductInventoryType())
                {
                    this.Visible = false;
                }
            }
        }

        #endregion

        #region Event handlers

        protected void btnAddToCart_Click(object sender, EventArgs e)
        {
            if (NumberOfItemsInInput > 0)
            {

                if (IsProductInventoryType())
                {
                    if (InsertedItemsHigherThanAvailableProducts())
                    {                     
                        DisplayErrorMessage(ResHelper.GetString("Kadena.Product.LowerNumberOfAvailableProducts", LocalizationContext.CurrentCulture.CultureCode));
                    }

                    else if (ItemsInCartAreExceeded())
                    {
                        DisplayErrorMessage(string.Format(
                            ResHelper.GetString("Kadena.Product.ItemsInCartExceeded", LocalizationContext.CurrentCulture.CultureCode),
                            PreviouslyAddedAmount, DocumentContext.CurrentDocument.GetIntegerValue("SKUAvailableItems", 0) - PreviouslyAddedAmount));
                    }

                    else if (!IsAddedAmmountValid(NumberOfItemsInInput + PreviouslyAddedAmount))
                    {                      
                        DisplayErrorMessage(ResHelper.GetString("Kadena.Product.QuantityOutOfRange", LocalizationContext.CurrentCulture.CultureCode));
                    }
                    else
                    {
                        AddToCartProcess();
                    }

                }

                else
                {
                    AddToCartProcess();
                }              
            }
            else
            {                
                DisplayErrorMessage(ResHelper.GetString("Kadena.Product.InsertedAmmountValueIsNotValid", LocalizationContext.CurrentCulture.CultureCode));
            }
        }

        #endregion

        #region Private methods

        private static void AddHiddenInput(string name, string value)
        {
            
        }

        private int NumberOfItemsInInput
        {
            get
            {
                return ValidationHelper.GetInteger(inpNumberOfItems.Value, 0);
            }

        }

        private int PreviouslyAddedAmount { get; set; }
        private void AddToCartProcess()
        {
            AddItemsToShoppingCart(ValidationHelper.GetInteger(inpNumberOfItems.Value, 0), PreviouslyAddedAmount, DocumentContext.CurrentDocument.DocumentID);
            ScriptHelper.RegisterClientScriptBlock(Page, typeof(string), "Alert", ScriptHelper.GetScript("alert('" + ResHelper.GetString("Kadena.Product.ItemsAddedToCart", LocalizationContext.CurrentCulture.CultureCode) + "');"));
        }
      

        private bool ItemsInCartAreExceeded()
        {
            return PreviouslyAddedAmount + NumberOfItemsInInput > DocumentContext.CurrentDocument.GetIntegerValue("SKUAvailableItems", 0);           
        }

        private bool InsertedItemsHigherThanAvailableProducts()
        {
            return NumberOfItemsInInput > DocumentContext.CurrentDocument.GetIntegerValue("SKUAvailableItems", 0);
        }

        private void DisplayErrorMessage(string errorMessage)
        {
            lblNumberOfItemsError.Text = errorMessage;
            SetErrorLblVisible();
        }
        private void SetErrorLblVisible()
        {
            lblNumberOfItemsError.Visible = true;
            inpNumberOfItems.Attributes.Add("class", "input__text input--error");

        }

        private bool IsStockEmpty()
        {
            if (DocumentContext.CurrentDocument.GetValue("SKUAvailableItems") != null)
            {
                return (int)DocumentContext.CurrentDocument.GetValue("SKUAvailableItems") == 0;
            }

            return true;
        }

        private bool IsProductInventoryType()
        {
            if (DocumentContext.CurrentDocument.GetValue("ProductType") != null)
            {
                return DocumentContext.CurrentDocument.GetValue("ProductType").ToString().Contains("KDA.InventoryProduct");
            }

            return false;
        }

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
            
            return false;
        }

        private void AddItemsToShoppingCart(int ammount, int previouslyAddedAmmount, int documentId)
        {
            var product = SKUInfoProvider.GetSKUInfo(DocumentContext.CurrentDocument.GetIntegerValue("SKUID", 0));

            var artworkLocation = DocumentContext.CurrentDocument.GetStringValue("ProductArtworkLocation", string.Empty);
            var chiliTemplateId = DocumentContext.CurrentDocument.GetGuidValue("ProductChiliTemplateID", Guid.Empty);
            var productType = DocumentContext.CurrentDocument.GetStringValue("ProductType", string.Empty);
            var productThumbnail = DocumentContext.CurrentDocument.GetGuidValue("ProductThumbnail", Guid.Empty);

            if (product != null)
            {
                var cart = ECommerceContext.CurrentShoppingCart;
                AssignCartShippingAddress(cart);
                ShoppingCartInfoProvider.SetShoppingCartInfo(cart);
                
                var parameters = new ShoppingCartItemParameters(product.SKUID, ammount);
                var cartItem = cart.SetShoppingCartItem(parameters);

                cartItem.CartItemText = product.SKUName;
                cartItem.SetValue("ChiliTemplateID", chiliTemplateId);
                cartItem.SetValue("ArtworkLocation", artworkLocation);
                cartItem.SetValue("ProductType", productType);
                cartItem.SetValue("ProductPageID", documentId);
                cartItem.SetValue("ProductThumbnail", productThumbnail);

                var dynamicUnitPrice = GetUnitPriceForAmmount(ammount + previouslyAddedAmmount);
                if (dynamicUnitPrice > 0)
                {
                    cartItem.CartItemPrice = dynamicUnitPrice;
                }
                ShoppingCartItemInfoProvider.SetShoppingCartItemInfo(cartItem);
            }
        }

        private void AssignCartShippingAddress(ShoppingCartInfo cart)
        {
            var customerAddress = AddressInfoProvider.GetAddresses(ECommerceContext.CurrentCustomer?.CustomerID ?? 0).FirstOrDefault();
            cart.ShoppingCartShippingAddress = customerAddress;
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