using CMS.DocumentEngine;
using CMS.EventLog;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.PortalEngine.Web.UI;
using CMS.SiteProvider;
using Kadena.Old_App_Code.Kadena.DynamicPricing;
using System.Collections.Generic;
using System.Text;
using System.Web.Script.Serialization;

namespace Kadena.CMSWebParts.Kadena.Product
{
    public partial class DynamicPricingTable : CMSAbstractWebPart
    {
        private const string _TableRowTemplate = "<tr><td>{0}</td><td{2}>{1}</td></tr>";

        public string PriceElementName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("PriceElementName"), string.Empty);
            }
        }

        public override void OnContentLoaded()
        {

            if (!SeeProductPricing())
            {
                Visible = false;
                return;
            }

            base.OnContentLoaded();
            SetupControl();
        }

        private bool SeeProductPricing()
        {
            return UserInfoProvider.IsAuthorizedPerResource("Kadena_Orders", "KDA_SeePrices", SiteContext.CurrentSiteName, CurrentUser);
        }

        protected void SetupControl()
        {
            if (!StopProcessing)
            {
                var rawData = new JavaScriptSerializer().Deserialize<List<DynamicPricingRawData>>(DocumentContext.CurrentDocument.GetStringValue("ProductDynamicPricing", string.Empty));

                if (rawData == null || rawData.Count == 0)
                {
                    var basePrice = DocumentContext.CurrentDocument.GetDoubleValue("SKUPrice", 0);
                    ltlTableContent.Text = string.Format(_TableRowTemplate,
                        ResHelper.GetString("Kadena.Product.BasePriceTitle", LocalizationContext.CurrentCulture.CultureCode),
                        basePrice.ToString("C"),
                        string.IsNullOrWhiteSpace(PriceElementName) ? string.Empty : $" id='{PriceElementName}'");
                }
                else
                {
                    List<DynamicPricingData> data;
                    if (new DynamicPricingDataHelper().ConvertDynamicPricingData(rawData, out data))
                    {
                        var result = new StringBuilder();
                        foreach (var item in data)
                        {
                            result.Append(string.Format(_TableRowTemplate, string.Format(ResHelper.GetString("Kadena.Product.PiecesFormatString", LocalizationContext.CurrentCulture.CultureCode), item.Min, item.Max), item.Price.ToString("C"), string.Empty));
                        }
                        ltlTableContent.Text = result.ToString();
                    }
                    else
                    {
                        EventLogProvider.LogEvent("E", "Dynamic pricing table", "Display dynamic pricing data", "Dynamic pricing data couldn't be restored");
                    }
                }
            }
        }
    }
}