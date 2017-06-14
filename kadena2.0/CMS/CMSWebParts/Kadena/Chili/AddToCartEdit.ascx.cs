using CMS.Base.Web.UI;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Ecommerce;
using CMS.EventLog;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.PortalEngine.Web.UI;
using Kadena.Old_App_Code.Helpers;
using Kadena.Old_App_Code.Kadena.DynamicPricing;
using Kadena.Old_App_Code.Kadena.MailingList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kadena.CMSWebParts.Kadena.Chili
{
    public partial class AddToCartExtended : CMSAbstractWebPart
    {
        public override void OnContentLoaded()
        {
            base.OnContentLoaded();

            if (IsProductMailingType())
            {
                SetMailingListData();
                SetNumberOfAddresses();
            }
                    
            SetupControl();

        }

        private int NumberOfAddressesReturnedByService { get; set; }

        private int NumberOfItemsInInput
        {
            get
            {
                return ValidationHelper.GetInteger(inpNumberOfItems.Value, 0);
            }
          
        }

        private MailingListData MailingListData
        {
            get; set;
        }

        private bool IsProductMailingType()
        {
            return GetProductType().Contains("KDA.MailingProduct");
        }

        protected void btnAddToCart_Click(object sender, EventArgs e)
        {
            if (NumberOfItemsInInput > 0 && IsAddedAmmountValid(NumberOfItemsInInput))
            {
                if (IsProductMailingType())
                {
                    if (NumberOfItemsInInput.Equals(NumberOfAddressesReturnedByService))
                    {
                        AddItemsToShoppingCart(NumberOfItemsInInput);
                    }
                    else
                    {
                        DisplayErrorMessage();
                    }
                }
                else
                {
                    AddItemsToShoppingCart(NumberOfItemsInInput);
                }

            }
            else
            {
                DisplayErrorMessage();

            }
        }

        protected void SetupControl()
        {
            if (!StopProcessing)
            {
                btnAddToCart.Text = ResHelper.GetString("Kadena.Product.AddToCart", LocalizationContext.CurrentCulture.CultureCode);
                inpNumberOfItems.Attributes.Add("class", "input__text");
                lblNumberOfItemsError.Visible = false;            

                lblQuantity.Text = ResHelper.GetString("Kadena.Product.AddToCartQuantity", LocalizationContext.CurrentCulture.CultureCode);

                if (IsProductMailingType())
                {
                    inpNumberOfItems.Attributes.Add("disabled", "true");
                    inpNumberOfItems.Value = NumberOfAddressesReturnedByService.ToString();
                } 

            }

        }

        private void DisplayErrorMessage()
        {
            lblNumberOfItemsError.Text = ResHelper.GetString("Kadena.Product.InsertedAmmountValueIsNotValid", LocalizationContext.CurrentCulture.CultureCode);
            SetErrorLblVisible();
        }
        private void SetNumberOfAddresses()
        {
            if (MailingListData != null)
            {
                NumberOfAddressesReturnedByService = MailingListData.addressCount;
            }
        }

        private void SetMailingListData()
        {
            Guid containerId;
          
            if (Guid.TryParse(Request.QueryString["containerId"], out containerId))
            {
                MailingListData = ServiceHelper.GetMailingList(containerId);    
            }
       
        }

        private void SetErrorLblVisible()
        {
            lblNumberOfItemsError.Visible = true;
            inpNumberOfItems.Attributes.Add("class", "input__text input--error");
        }

        private string GetProductType()
        {
            int documentId;
            string productType = string.Empty;

            if (int.TryParse(Request.QueryString["id"], out documentId))
            {
               productType = DocumentHelper.GetDocument(
                   documentId, 
                   new TreeProvider(MembershipContext.AuthenticatedUser)).GetStringValue("ProductType", string.Empty);
            }

            return productType;
        }
      
        private void AddItemsToShoppingCart(int ammount)
        {
            int skuID;
            int documentId;
            Guid templateId;
            if (int.TryParse(Request.QueryString["skuId"], out skuID) &&
                int.TryParse(Request.QueryString["id"], out documentId) &&
                Guid.TryParse(Request.QueryString["templateId"], out templateId))
            {
                var product = SKUInfoProvider.GetSKUInfo(skuID);
                var document = DocumentHelper.GetDocument(documentId, new TreeProvider(MembershipContext.AuthenticatedUser));

                if (document != null && product != null)
                {
                    var artworkLocation = document.GetStringValue("ProductArtworkLocation", string.Empty);
                    var chiliTemplateId = document.GetGuidValue("ProductChiliTemplateID", Guid.Empty);
                    var productType = document.GetStringValue("ProductType", string.Empty);


                    var cart = ECommerceContext.CurrentShoppingCart;
                    AssignCartShippingAddress(cart);
                    ShoppingCartInfoProvider.SetShoppingCartInfo(cart);

                    var parameters = new ShoppingCartItemParameters(product.SKUID, ammount);
                    var cartItem = cart.SetShoppingCartItem(parameters);

                    cartItem.SetValue("ChiliTemplateID", chiliTemplateId);
                    cartItem.SetValue("ArtworkLocation", artworkLocation);
                    cartItem.SetValue("ProductType", productType);
                    cartItem.SetValue("ProductPageID", documentId);
                    cartItem.SetValue("ChilliEditorTemplateID", templateId);

                    if (MailingListData != null)
                    {
                        cartItem.SetValue("MailingListName", MailingListData.name);
                        cartItem.SetValue("MailingListGuid", MailingListData.id);
                    }
                    

                    var dynamicUnitPrice = GetUnitPriceForAmmount(ammount);
                    if (dynamicUnitPrice > 0)
                    {
                        cartItem.CartItemPrice = dynamicUnitPrice;
                    }

                    ShoppingCartItemInfoProvider.SetShoppingCartItemInfo(cartItem);
                    ScriptHelper.RegisterClientScriptBlock(Page, typeof(string), "Alert", ScriptHelper.GetScript("alert('" + ResHelper.GetString("Kadena.Product.ItemsAddedToCart", LocalizationContext.CurrentCulture.CultureCode) +"');"));
                                      
                }
            }
                   
        }

        private void AssignCartShippingAddress(ShoppingCartInfo cart)
        {
            var customerAddress = AddressInfoProvider.GetAddresses(ECommerceContext.CurrentCustomer?.CustomerID ?? 0).FirstOrDefault();

            if (customerAddress != null)
            {
                cart.ShoppingCartShippingAddress = customerAddress;
            }
            else
            {
                cart.ShoppingCartShippingAddress = null;
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
    }
}