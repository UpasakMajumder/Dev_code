using CMS.Base.Web.UI;
using CMS.DocumentEngine;
using CMS.Ecommerce;
using CMS.EventLog;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.PortalEngine.Web.UI;
using Kadena.Dto.MailingList.MicroserviceResponses;
using Kadena.Old_App_Code.Helpers;
using Kadena.Old_App_Code.Kadena.DynamicPricing;
using Kadena2.MicroserviceClients.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Threading.Tasks;
using CMS.DataEngine;
using CMS.SiteProvider;

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
            SetupDocument();

        }

        private int NumberOfAddressesReturnedByService { get; set; }

        private int NumberOfItemsInInput
        {
            get
            {
                return ValidationHelper.GetInteger(inpNumberOfItems.Value, 0);
            }

        }

        private MailingListDataDTO MailingListData
        {
            get; set;
        }

        private ShoppingCartItemInfo CurrentShoppingCartItem
        {
            get; set;
        }

        private TreeNode ReferencedDocument { get; set; }

        private bool IsProductMailingType()
        {
            return GetProductType().Contains("KDA.MailingProduct");
        }

        protected void btnAddToCart_Click(object sender, EventArgs e)
        {
            if (NumberOfItemsInInput < 1)
            {
                DisplayErrorMessage(ResHelper.GetString("Kadena.Product.InsertedAmmountValueIsNotValid", LocalizationContext.CurrentCulture.CultureCode));
                return;
            }

            if (IsProductMailingType()
                    && !NumberOfItemsInInput.Equals(NumberOfAddressesReturnedByService))
            {
                DisplayErrorMessage(ResHelper.GetString("Kadena.Product.InsertedAmmountValueIsNotValid", LocalizationContext.CurrentCulture.CultureCode));
                return;
            }

            if (!IsAddedAmmountValid(NumberOfItemsInInput + GetItemsInCart()))
            {
                DisplayErrorMessage(ResHelper.GetString("Kadena.Product.QuantityOutOfRange", LocalizationContext.CurrentCulture.CultureCode));
                return;
            }

            AddItemsToShoppingCart(NumberOfItemsInInput);

        }

        protected void SetupControl()
        {
            if (!StopProcessing)
            {
                btnAddToCart.Text = ResHelper.GetString("Kadena.Product.AddToCart", LocalizationContext.CurrentCulture.CultureCode);
                inpNumberOfItems.Attributes.Add("class", "input__text");
                lblNumberOfItemsError.Visible = false;

                lblQuantity.Text = ResHelper.GetString("Kadena.Product.AddToCartQuantity", LocalizationContext.CurrentCulture.CultureCode);

                InitializeCurrentShoppingCartItem();

                if (IsProductMailingType())
                {
                    inpNumberOfItems.Attributes.Add("disabled", "true");
                    inpNumberOfItems.Value = NumberOfAddressesReturnedByService.ToString();
                }
                else
                {
                    if (CurrentShoppingCartItem != null)
                    {
                        inpNumberOfItems.Value = CurrentShoppingCartItem.CartItemUnits.ToString();
                    }
                }
            }
        }


        private void InitializeCurrentShoppingCartItem()
        {
            int skuID;

            if (int.TryParse(Request.QueryString["skuId"], out skuID))
            {
                CurrentShoppingCartItem = ShoppingCartItemInfoProvider.GetShoppingCartItems().
                    WhereEquals("SKUID", skuID).
                    WhereEquals("ShoppingCartID", ECommerceContext.CurrentShoppingCart.ShoppingCartID).FirstObject;

            }
        }
        private void DisplayErrorMessage(string errorMessage)
        {
            lblNumberOfItemsError.Text = errorMessage;
            SetErrorLblVisible();
        }
        private void SetNumberOfAddresses()
        {
            if (MailingListData != null)
            {
                NumberOfAddressesReturnedByService = MailingListData.AddressCount;
            }
        }

        private void SetMailingListData()
        {
            Guid containerId;

            if (Guid.TryParse(Request.QueryString["containerId"], out containerId))
            {
                try
                {
                    MailingListData = ServiceHelper.GetMailingList(containerId);
                }
                catch (System.Net.Http.HttpRequestException ex)
                {
                    EventLogProvider.LogException("Add to cart edit", "SET MAILING LIST DATA", ex);
                    this.Visible = false;
                }                
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
                    var chiliPdfGeneratorSettingsId = document.GetGuidValue("ProductChiliPdfGeneratorSettingsId", Guid.Empty);
                    var productChiliWorkspaceId = document.GetGuidValue("ProductChiliWorkgroupID", Guid.Empty);
                    var productType = document.GetStringValue("ProductType", string.Empty);
                    var productThumbnail = document.GetGuidValue("ProductThumbnail", Guid.Empty);


                    var cart = ECommerceContext.CurrentShoppingCart;
                    AssignCartShippingAddress(cart);
                    ShoppingCartInfoProvider.SetShoppingCartInfo(cart);

                    ShoppingCartItemInfo cartItem = null;

                    if (CurrentShoppingCartItem != null)
                    {
                        cartItem = CurrentShoppingCartItem;
                        ShoppingCartItemInfoProvider.UpdateShoppingCartItemUnits(CurrentShoppingCartItem, int.Parse(inpNumberOfItems.Value));
                    }
                    else
                    {
                        var parameters = new ShoppingCartItemParameters(product.SKUID, ammount);
                        cartItem = cart.SetShoppingCartItem(parameters);
                    }

                    var customizedName = Request?.Form?.GetValues("customizedProductName").FirstOrDefault();
                    if (!string.IsNullOrEmpty(customizedName) && customizedName != cartItem.CartItemText)
                    {
                        cartItem.CartItemText = customizedName;
                    }

                    cartItem.SetValue("ChiliTemplateID", chiliTemplateId);
                    cartItem.SetValue("ArtworkLocation", artworkLocation);
                    cartItem.SetValue("ProductType", productType);
                    cartItem.SetValue("ProductPageID", documentId);
                    cartItem.SetValue("ChilliEditorTemplateID", templateId);
                    cartItem.SetValue("ProductChiliPdfGeneratorSettingsId", chiliPdfGeneratorSettingsId);
                    cartItem.SetValue("ProductChiliWorkspaceId", productChiliWorkspaceId);
                    cartItem.SetValue("ProductThumbnail", productThumbnail);

                    if (MailingListData != null)
                    {
                        cartItem.SetValue("MailingListName", MailingListData.Name);
                        cartItem.SetValue("MailingListGuid", MailingListData.Id);
                    }


                    var dynamicUnitPrice = GetUnitPriceForAmmount(ammount);
                    if (dynamicUnitPrice > 0)
                    {
                        cartItem.CartItemPrice = dynamicUnitPrice;
                    }

                    if (productType.Contains("KDA.TemplatedProduct"))
                    {
                        if (!CallRunGeneratePdfTask(cartItem, templateId, chiliPdfGeneratorSettingsId))
                        {
                            ScriptHelper.RegisterClientScriptBlock(Page, typeof(string), "Error", ScriptHelper.GetScript("alert('Unable to add item into cart because start generating hires PDF failed');"));
                            return;
                        }
                    }

                    ShoppingCartItemInfoProvider.SetShoppingCartItemInfo(cartItem);
                    ScriptHelper.RegisterClientScriptBlock(Page, typeof(string), "Alert", ScriptHelper.GetScript("alert('" + ResHelper.GetString("Kadena.Product.ItemsAddedToCart", LocalizationContext.CurrentCulture.CultureCode) + "');"));

                }
            }

        }

        private bool CallRunGeneratePdfTask(ShoppingCartItemInfo cartItem, Guid templateId, Guid settingsId)
        {
            string endpoint = SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.KDA_TemplatingServiceEndpoint");
            var templatedService = new TemplatedProductService();
            // async approchach caused error in webpart:
            // An asynchronous operation cannot be started at this time. Asynchronous operations may only be started
            // within an asynchronous handler or module or during certain events in the Page lifecycle.
            var response = templatedService.RunGeneratePdfTask(endpoint, templateId.ToString(), settingsId.ToString()).Result;
            if (response.Success && response.Payload != null)
            {
                cartItem.SetValue("DesignFilePathTaskId", response.Payload.TaskId);
                if (response.Payload.Finished)
                {
                    cartItem.SetValue("DesignFilePathObtained", true);
                    cartItem.SetValue("DesignFilePath", response.Payload.FileName);
                }
                else
                {
                    cartItem.SetValue("DesignFilePathObtained", false);
                }

                cartItem.SubmitChanges(false);
                cartItem.Update();
                return true;
            }
            else
            {
                EventLogProvider.LogEvent("Error", $"Template service client with templateId={templateId} and settingsId={settingsId}", "ERROR", response?.Error?.Message ?? string.Empty);
                return false;
            }
        }

        private void AssignCartShippingAddress(ShoppingCartInfo cart)
        {
            var customerAddress = AddressInfoProvider.GetAddresses(ECommerceContext.CurrentCustomer?.CustomerID ?? 0).FirstOrDefault();
            cart.ShoppingCartShippingAddress = customerAddress;
        }



        private void SetupDocument()
        {
            int documentId;

            if (int.TryParse(Request.QueryString["id"], out documentId))
            {
                ReferencedDocument = DocumentHelper.GetDocument(documentId, new TreeProvider(MembershipContext.AuthenticatedUser));
            }

        }

        private bool IsAddedAmmountValid(int ammount)
        {

            var rawData = new JavaScriptSerializer().Deserialize<List<DynamicPricingRawData>>(ReferencedDocument?.GetStringValue("ProductDynamicPricing", string.Empty) ?? string.Empty);
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

        private double GetUnitPriceForAmmount(int ammount)
        {
            var rawData = new JavaScriptSerializer().Deserialize<List<DynamicPricingRawData>>(ReferencedDocument?.GetStringValue("ProductDynamicPricing", string.Empty) ?? string.Empty);

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

        private int GetItemsInCart()
        {
            var cartItem = ECommerceContext.CurrentShoppingCart.CartItems.Where(item => item.SKUID == DocumentContext.CurrentDocument.GetIntegerValue("SKUID", 0)).FirstOrDefault();

            return cartItem?.CartItemUnits ?? 0;
        }
    }
}