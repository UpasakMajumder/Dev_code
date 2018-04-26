using CMS.DocumentEngine;
using CMS.Ecommerce;
using CMS.EventLog;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.PortalEngine.Web.UI;
using CMS.SiteProvider;
using Kadena.BusinessLogic.Contracts;
using Kadena.Container.Default;
using Kadena.Models.Product;
using Kadena.Old_App_Code.Kadena.DynamicPricing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace Kadena.CMSWebParts.Kadena.Product
{
    public partial class DynamicPricingTable : CMSAbstractWebPart
    {
        private const string _TableRowTemplate = "<tr><td>{0} {1}</td><td{3}>{4} {2}</td></tr>";

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
                var uom = DocumentContext.CurrentDocument.GetStringValue("SKUUnitOfMeasure", UnitOfMeasure.DefaultUnit);
                var uomLocalized = DIContainer.Resolve<IProductsService>().TranslateUnitOfMeasure(uom, LocalizationContext.CurrentCulture.CultureCode);

                if (rawData == null || rawData.Count == 0)
                {
                    if (DocumentContext.CurrentDocument.HasSKU)
                    {
                        var skuCategories = SKUOptionCategoryInfoProvider
                            .GetSKUOptionCategories()
                            .Columns(nameof(SKUOptionCategoryInfo.CategoryID))
                            .WhereEquals(nameof(SKUOptionCategoryInfo.SKUID), DocumentContext.CurrentDocument.NodeSKUID);
                        var optionCategories = OptionCategoryInfoProvider
                            .GetOptionCategories()
                            .Columns(nameof(OptionCategoryInfo.CategoryDefaultOptions))
                            .WhereIn(nameof(OptionCategoryInfo.CategoryID), skuCategories)
                            .And()
                            .WhereEquals(nameof(OptionCategoryInfo.CategoryType), OptionCategoryTypeEnum.Attribute.ToStringRepresentation())
                            .And()
                            .WhereEquals(nameof(OptionCategoryInfo.CategoryEnabled), true)
                            .ToList();

                        SKUInfo variant = null;

                        if (optionCategories.Count > 0)
                        {
                            variant = VariantHelper.GetProductVariant(DocumentContext.CurrentDocument.NodeSKUID,
                                new ProductAttributeSet(optionCategories.Select(c =>
                                {
                                    int.TryParse(c.CategoryDefaultOptions, out int id);
                                    return id;
                                })));
                        }

                        var basePrice = variant?.SKUPrice ?? DocumentContext.CurrentDocument.GetDoubleValue("SKUPrice", 0);

                        

                        ltlTableContent.Text = string.Format(_TableRowTemplate,
                                ResHelper.GetString("Kadena.Product.BasePriceTitle", LocalizationContext.CurrentCulture.CultureCode),
                                uomLocalized,
                                basePrice.ToString("N2"),
                                string.IsNullOrWhiteSpace(PriceElementName) ? string.Empty : $" id='{PriceElementName}'",
                                ResHelper.GetString("Kadena.Checkout.ItemPricePrefix", LocalizationContext.CurrentCulture.CultureCode));
                    }
                }
                else
                {
                    List<DynamicPricingData> data;
                    if (new DynamicPricingDataHelper().ConvertDynamicPricingData(rawData, out data))
                    {
                        var result = new StringBuilder();
                        foreach (var item in data)
                        {
                            result.Append(string.Format(_TableRowTemplate,
                                string.Format(ResHelper.GetString("Kadena.Product.PiecesFormatString", LocalizationContext.CurrentCulture.CultureCode), item.Min, item.Max),
                                uomLocalized,
                                item.Price.ToString("N2"),
                                string.Empty,
                                ResHelper.GetString("Kadena.Checkout.ItemPricePrefix", LocalizationContext.CurrentCulture.CultureCode)));
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