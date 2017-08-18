using CMS.DocumentEngine;
using CMS.Ecommerce;
using CMS.Membership;
using CMS.PortalEngine.Web.UI;
using Kadena.Models;

namespace Kadena.CMSWebParts.Kadena.Chili
{
    public partial class AddToCartExtended : CMSAbstractWebPart
    {
        public override void OnContentLoaded()
        {
            base.OnContentLoaded();
            SetupControl();
        }

        private ShoppingCartItemInfo CurrentShoppingCartItem { get; set; }

        private TreeNode ReferencedDocument { get; set; }

        private bool IsProductMailingType()
        {
            return GetProductType().Contains(ProductTypes.MailingProduct);
        }

        protected void SetupControl()
        {
            if (!StopProcessing)
            {
                SetupDocument();
                InitializeCurrentShoppingCartItem();

                if (IsProductMailingType())
                {
                    inpNumberOfItems.Attributes.Add("disabled", "true");
                    inpNumberOfItems.Value = Request.QueryString["quantity"];
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
            if (ReferencedDocument != null)
            {
                CurrentShoppingCartItem = ShoppingCartItemInfoProvider.GetShoppingCartItems().
                    WhereEquals("SKUID", ReferencedDocument.NodeSKUID).
                    WhereEquals("ShoppingCartID", ECommerceContext.CurrentShoppingCart.ShoppingCartID).FirstObject;
            }
        }

        private string GetProductType()
        {
            string productType = string.Empty;

            if (ReferencedDocument != null)
            {
                productType = ReferencedDocument.GetStringValue("ProductType", string.Empty);
            }

            return productType;
        }

        private void SetupDocument()
        {
            int documentId;

            if (int.TryParse(Request.QueryString["documentId"], out documentId))
            {
                ReferencedDocument = DocumentHelper.GetDocument(documentId, new TreeProvider(MembershipContext.AuthenticatedUser));
            }

        }
    }
}