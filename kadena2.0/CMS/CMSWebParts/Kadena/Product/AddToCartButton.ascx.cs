using CMS.DocumentEngine;
using CMS.Localization;
using CMS.Membership;
using CMS.PortalEngine.Web.UI;
using Kadena.Models.Product;
using System.IO;
using System.Web.UI;

namespace Kadena.CMSWebParts.Kadena.Product
{
    public partial class AddToCartButton : CMSAbstractWebPart
    {
        private TreeNode _productDocument;

        public override void OnContentLoaded()
        {
            base.OnContentLoaded();
            SetupControl();
        }

        protected void SetupControl()
        {
            if (StopProcessing)
            {
                return;
            }

            SetupDocument();
            SetupQuantity();
            
            if (IsProductInventoryType() && IsStockEmpty())
            {
                this.Visible = false;
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

        private void SetupQuantity()
        {
            if (!int.TryParse(Request.QueryString["quantity"], out int quantity))
            {
                quantity = 1;
            }
            inpNumberOfItems.Value = quantity.ToString();

            if (IsProductMailingType())
            {
                inpNumberOfItems.Attributes.Add("disabled", "true");
            }
        }

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
            var productType = _productDocument.GetStringValue("ProductType", string.Empty);
            return ProductTypes.IsOfType(ProductTypes.InventoryProduct, productType);
        }

        private bool IsProductMailingType()
        {
            var productType = _productDocument.GetStringValue("ProductType", string.Empty);
            return ProductTypes.IsOfType(ProductTypes.MailingProduct, productType);
        }

        private bool IsProductTemplatedType()
        {
            var productType = _productDocument.GetStringValue("ProductType", string.Empty);
            return ProductTypes.IsOfType(ProductTypes.TemplatedProduct, productType);
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
    }
}