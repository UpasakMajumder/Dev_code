using CMS.DocumentEngine;
using CMS.Ecommerce;
using CMS.Localization;
using CMS.Membership;
using CMS.PortalEngine.Web.UI;
using Kadena.Models;
using System.IO;
using System.Web.UI;

namespace Kadena.CMSWebParts.Kadena.Product
{
    public partial class AddToCartButton : CMSAbstractWebPart
    {
        private TreeNode _productDocument;

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
                SetupDocument();

                if (IsProductMailingType())
                {
                    inpNumberOfItems.Attributes.Add("disabled", "true");
                    inpNumberOfItems.Value = Request.QueryString["quantity"];
                }
                else
                {
                    if (IsProductTemplatedType())
                    {
                        var cartItem = ShoppingCartItemInfoProvider.GetShoppingCartItems()
                            .WhereEquals("SKUID", _productDocument.NodeSKUID)
                            .WhereEquals("ShoppingCartID", ECommerceContext.CurrentShoppingCart.ShoppingCartID)
                            .FirstObject;
                        if (cartItem != null)
                        {
                            inpNumberOfItems.Value = cartItem.CartItemUnits.ToString();
                        }
                    }
                    if (IsProductInventoryType() && IsStockEmpty())
                    {
                        this.Visible = false;
                    }
                }

                if (IsProductTemplatedType())
                {
                    btnAddToCart.Attributes.Add("class", "btn-action js-chili-addtocart");
                    btnAddToCart.Disabled = true;
                }
                else
                {
                    btnAddToCart.Attributes.Add("class", "btn-action js-add-to-cart");
                }

                Controls.Add(new LiteralControl(GetHiddenInput("documentId", _productDocument.DocumentID.ToString())));
                if (!string.IsNullOrWhiteSpace(Request.QueryString["templateId"]))
                {
                    Controls.Add(new LiteralControl(GetHiddenInput("templateId", Request.QueryString["templateId"])));
                }
                if (!string.IsNullOrWhiteSpace(Request.QueryString["containerId"]))
                {
                    Controls.Add(new LiteralControl(GetHiddenInput("containerId", Request.QueryString["containerId"])));
                }
            }
        }

        #endregion

        #region Private methods

        private static string GetHiddenInput(string name, string value)
        {
            using (var stringWriter = new StringWriter())
            {
                using (var html = new HtmlTextWriter(stringWriter))
                {
                    html.AddAttribute(HtmlTextWriterAttribute.Class, "js-add-to-cart-property");
                    html.AddAttribute(HtmlTextWriterAttribute.Name, name);
                    html.AddAttribute(HtmlTextWriterAttribute.Value, value);
                    html.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
                    html.RenderBeginTag(HtmlTextWriterTag.Input);
                    html.RenderEndTag();
                    return stringWriter.ToString();
                }
            }
        }

        private bool IsStockEmpty()
        {
            if (_productDocument.GetValue("SKUAvailableItems") != null)
            {
                return (int)_productDocument.GetValue("SKUAvailableItems") == 0;
            }

            return true;
        }

        private bool IsProductInventoryType()
        {
            if (_productDocument.GetValue("ProductType") != null)
            {
                return _productDocument.GetStringValue("ProductType", string.Empty).Contains(ProductTypes.InventoryProduct);
            }

            return false;
        }

        private bool IsProductMailingType()
        {
            if (_productDocument.GetValue("ProductType") != null)
            {
                return _productDocument.GetStringValue("ProductType", string.Empty).Contains(ProductTypes.MailingProduct);
            }

            return false;
        }

        private bool IsProductTemplatedType()
        {
            if (_productDocument.GetValue("ProductType") != null)
            {
                return _productDocument.GetStringValue("ProductType", string.Empty).Contains(ProductTypes.TemplatedProduct);
            }

            return false;
        }

        private void SetupDocument()
        {
            var nodeId = Request.QueryString["nodeId"];
            if (string.IsNullOrWhiteSpace(nodeId))
            {
                _productDocument = DocumentContext.CurrentDocument;
            }
            else
            {
                _productDocument = DocumentHelper.GetDocument(int.Parse(nodeId), LocalizationContext.CurrentCulture.CultureCode,
                    new TreeProvider(MembershipContext.AuthenticatedUser));
            }
        }
        #endregion
    }
}