using CMS.DocumentEngine;
using CMS.PortalEngine.Web.UI;

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
        if (string.IsNullOrEmpty(DocumentContext.CurrentDocument.GetStringValue("ProductShippingCost", string.Empty)))
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
  }
}