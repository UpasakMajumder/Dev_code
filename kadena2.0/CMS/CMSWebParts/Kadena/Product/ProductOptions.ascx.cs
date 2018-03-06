using CMS.Ecommerce;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;

namespace Kadena.CMSWebParts.Kadena.Product
{
    public partial class ProductOptions : CMSAbstractWebPart
    {
        private readonly Dictionary<OptionCategorySelectionTypeEnum, Func<OptionCategoryInfo, IEnumerable<SKUInfo>, string>> _selectorBuilders
            = new Dictionary<OptionCategorySelectionTypeEnum, Func<OptionCategoryInfo, IEnumerable<SKUInfo>, string>>
            {
                { OptionCategorySelectionTypeEnum.Dropdownlist, BuildDropdown },
                { OptionCategorySelectionTypeEnum.RadioButtonsHorizontal, BuildRadioHorizontal},
                { OptionCategorySelectionTypeEnum.RadioButtonsVertical, BuildRadioVertical }
            };


        public int SKUID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("SKUID"), 0);
            }
        }

        public string PriceElementName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("PriceElementName"), string.Empty);
            }
        }

        public string PriceUrl
        {
            get
            {
                return URLHelper.ResolveUrl(ValidationHelper.GetString(GetValue("PriceUrl"), string.Empty));
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var skuCategories = SKUOptionCategoryInfoProvider
                .GetSKUOptionCategories()
                .Columns(nameof(SKUOptionCategoryInfo.CategoryID))
                .WhereEquals(nameof(SKUOptionCategoryInfo.SKUID), SKUID)
                .OrderByDefault();
            foreach (var cat in skuCategories)
            {
                var category = OptionCategoryInfoProvider.GetOptionCategoryInfo(cat.CategoryID);
                if (category.CategoryType != OptionCategoryTypeEnum.Attribute 
                    || !category.CategoryEnabled)
                {
                    continue;
                }
                var optionSku = SKUInfoProvider.GetSKUOptionsForProduct(SKUID, cat.CategoryID, true);
                var markup = _selectorBuilders[category.CategorySelectionType](category, optionSku);
                phSelectors.Controls.Add(new LiteralControl(markup));
            }
        }

        private static string BuildDropdown(OptionCategoryInfo category, IEnumerable<SKUInfo> skus)
        {
            using (var stringWriter = new StringWriter())
            {
                using (var html = new HtmlTextWriter(stringWriter))
                {
                    html.AddAttribute(HtmlTextWriterAttribute.Class, "input__wrapper product-options__input");
                    html.RenderBeginTag(HtmlTextWriterTag.Div);
                    html.AddAttribute(HtmlTextWriterAttribute.Class, "input__select");
                    html.RenderBeginTag(HtmlTextWriterTag.Div);

                    html.AddAttribute(HtmlTextWriterAttribute.Class, "js-product-option js-add-to-cart-property");
                    html.AddAttribute(HtmlTextWriterAttribute.Name, category.CategoryName);
                    html.RenderBeginTag(HtmlTextWriterTag.Select);

                    html.AddAttribute(HtmlTextWriterAttribute.Disabled, null);
                    if (category.CategoryDefaultRecord == category.CategoryDefaultOptions)
                    {
                        html.AddAttribute(HtmlTextWriterAttribute.Selected, null);
                    }
                    html.RenderBeginTag(HtmlTextWriterTag.Option);
                    html.Write(category.CategoryDefaultRecord);
                    html.RenderEndTag();

                    foreach (var sku in skus)
                    {
                        if (sku.SKUID.ToString() == category.CategoryDefaultOptions)
                        {
                            html.AddAttribute(HtmlTextWriterAttribute.Selected, null);
                        }
                        html.AddAttribute(HtmlTextWriterAttribute.Value, sku.SKUID.ToString());
                        html.RenderBeginTag(HtmlTextWriterTag.Option);
                        html.Write(sku.SKUName);
                        html.RenderEndTag();
                    }

                    html.RenderEndTag();
                    html.RenderEndTag();
                    html.RenderEndTag();
                    return stringWriter.ToString();
                }
            }
        }

        private static string BuildRadio(OptionCategoryInfo category, IEnumerable<SKUInfo> skus, bool isHorizontal = false)
        {
            using (var stringWriter = new StringWriter())
            {
                using (var html = new HtmlTextWriter(stringWriter))
                {

                    html.AddAttribute(HtmlTextWriterAttribute.Class, isHorizontal ? "product-options__radio product-options__radio--row" : "product-options__radio product-options__radio--column");
                    html.RenderBeginTag(HtmlTextWriterTag.Div);
                    foreach (var sku in skus)
                    {
                        html.AddAttribute(HtmlTextWriterAttribute.Class, "input__wrapper");
                        html.RenderBeginTag(HtmlTextWriterTag.Div);

                        html.AddAttribute(HtmlTextWriterAttribute.Class, "input__radio js-product-option js-add-to-cart-property");
                        html.AddAttribute(HtmlTextWriterAttribute.Type, "radio");
                        html.AddAttribute(HtmlTextWriterAttribute.Name, category.CategoryName);
                        html.AddAttribute(HtmlTextWriterAttribute.Value, sku.SKUID.ToString());
                        html.AddAttribute(HtmlTextWriterAttribute.Id, sku.SKUID.ToString());

                        if (sku.SKUID.ToString() == category.CategoryDefaultOptions)
                        {
                            html.AddAttribute(HtmlTextWriterAttribute.Checked, null);
                        }
                        html.RenderBeginTag(HtmlTextWriterTag.Input);
                        html.RenderEndTag();

                        html.AddAttribute(HtmlTextWriterAttribute.Class, "input__label input__label--radio");
                        html.AddAttribute(HtmlTextWriterAttribute.For, sku.SKUID.ToString());
                        html.RenderBeginTag(HtmlTextWriterTag.Label);
                        html.Write(sku.SKUName);
                        html.RenderEndTag();
                        html.RenderEndTag();
                    }

                    html.RenderEndTag();
                    return stringWriter.ToString();
                }
            }
        }

        private static string BuildRadioHorizontal(OptionCategoryInfo category, IEnumerable<SKUInfo> skus)
        {
            return BuildRadio(category, skus, true);
        }

        private static string BuildRadioVertical(OptionCategoryInfo category, IEnumerable<SKUInfo> skus)
        {
            return BuildRadio(category, skus);
        }
    }
}