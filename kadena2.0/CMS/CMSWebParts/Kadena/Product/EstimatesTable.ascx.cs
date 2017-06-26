using CMS.DocumentEngine;
using CMS.Membership;
using CMS.PortalEngine.Web.UI;
using CMS.SiteProvider;

namespace Kadena.CMSWebParts.Kadena.Product
{
    public partial class EstimatesTable : CMSAbstractWebPart
    {
        public override void OnContentLoaded()
        {
            base.OnContentLoaded();
            SetupControl();

        }

        protected void SetupControl()
        {
            if (StopProcessing)
            {
                tblEstimates.Visible = false;
            }
            else
            {
                if (string.IsNullOrEmpty(DocumentContext.CurrentDocument.GetStringValue("ProductProductionTime", string.Empty)))
                {
                    rowProductionTime.Visible = false;
                }
                else
                {
                    ltlProductionTime.Text = DocumentContext.CurrentDocument.GetStringValue("ProductProductionTime", string.Empty);
                }
                if (string.IsNullOrEmpty(DocumentContext.CurrentDocument.GetStringValue("ProductShipTime", string.Empty)))
                {
                    rowShipTime.Visible = false;
                }
                else
                {
                    ltlShipTime.Text = DocumentContext.CurrentDocument.GetStringValue("ProductShipTime", string.Empty);
                }
                if (string.IsNullOrEmpty(DocumentContext.CurrentDocument.GetStringValue("ProductShippingCost", string.Empty)) || !SeeProductPricing())
                {
                    rowShippingCost.Visible = false;
                }
                else
                {
                    ltlShippingCost.Text = DocumentContext.CurrentDocument.GetStringValue("ProductShippingCost", string.Empty);
                }
                if (string.IsNullOrEmpty(DocumentContext.CurrentDocument.GetStringValue("ProductProductionTime", string.Empty)) &&
                  string.IsNullOrEmpty(DocumentContext.CurrentDocument.GetStringValue("ProductShipTime", string.Empty)) &&
                  string.IsNullOrEmpty(DocumentContext.CurrentDocument.GetStringValue("ProductShippingCost", string.Empty)))
                {
                    tblEstimates.Visible = false;
                }
            }
        }

        private bool SeeProductPricing()
        {
            return UserInfoProvider.IsAuthorizedPerResource("Kadena_Orders", "KDA_SeePrices", SiteContext.CurrentSiteName, CurrentUser);
        }
    }
}