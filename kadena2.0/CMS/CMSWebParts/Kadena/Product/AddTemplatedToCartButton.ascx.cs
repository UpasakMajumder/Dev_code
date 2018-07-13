using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.PortalEngine.Web.UI;
using Kadena.BusinessLogic.Contracts;
using Kadena.Container.Default;
using Kadena.Models.Product;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using TreeNode = CMS.DocumentEngine.TreeNode;

namespace Kadena.CMSWebParts.Kadena.Product
{
    public partial class AddTemplatedToCartButton : CMSAbstractWebPart
    {
        private TreeNode _productDocument;
        private bool _hasTieredPricing;

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

            selNumberOfItems.Visible = _hasTieredPricing;
            inpNumberOfItems.Visible = !_hasTieredPricing;

            btnAddToCart.Disabled = true;
            
            Controls.Add(new LiteralControl(GetHiddenInput("nodeId", _productDocument.NodeID.ToString())));
            if (!string.IsNullOrWhiteSpace(Request.QueryString["templateId"]))
            {
                Controls.Add(new LiteralControl(GetHiddenInput("templateId", Request.QueryString["templateId"])));
            }
            if (!string.IsNullOrWhiteSpace(Request.QueryString["containerId"]))
            {
                Controls.Add(new LiteralControl(GetHiddenInput("containerId", Request.QueryString["containerId"])));
            }

            var productUom = _productDocument.GetStringValue("SKUUnitOfMeasure", UnitOfMeasure.DefaultUnit);
            this.pcs.InnerText = DIContainer.Resolve<IProductsService>().TranslateUnitOfMeasure(productUom, LocalizationContext.CurrentCulture.CultureCode);
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

        private bool IsProductMailingType()
        {
            var productType = _productDocument.GetStringValue("ProductType", string.Empty);
            return ProductTypes.IsOfType(productType, ProductTypes.MailingProduct);
        }

        private void SetupDocument()
        {
            if(!int.TryParse(Request.QueryString["nodeId"], out var nodeId))
            {
                throw new ArgumentException("Missing node id parameter");
            }

            _productDocument = DocumentHelper.GetDocument(nodeId, LocalizationContext.CurrentCulture.CultureCode, new TreeProvider(MembershipContext.AuthenticatedUser));

            if (_productDocument.GetStringValue("ProductPricingModel", PricingModel.GetDefault()) == PricingModel.Tiered)
            {
                _hasTieredPricing = true;

                var ranges = DIContainer.Resolve<ITieredPriceRangeProvider>().GetTieredRanges(_productDocument.DocumentID).ToList();

                selNumberOfItems.Items.Clear();
                selNumberOfItems.Items.Add( new ListItem( ResHelper.GetString("Kadena.Product.SelectPriceTier") , "0", false));

                ranges.ForEach(r => selNumberOfItems.Items.Add(r.Quantity.ToString()));
            }
        }
    }
}