using Kadena.Old_App_Code.CMSModules.Macros.Kadena;
using CMS.Helpers;
using CMS.MacroEngine;
using System;
using System.Linq;
using CMS.DocumentEngine;
using CMS.Membership;
using CMS.Localization;
using CMS.SiteProvider;

[assembly: CMS.RegisterExtension(typeof(Kadena.Old_App_Code.CMSModules.Macros.Kadena.KadenaMacroMethods), typeof(KadenaMacroNamespace))]
namespace Kadena.Old_App_Code.CMSModules.Macros.Kadena
{
    public class KadenaMacroMethods : MacroMethodContainer
    {
        [MacroMethod(typeof(bool), "Validates combination of product types - static type variant.", 1)]
        [MacroMethodParam(0, "productTypes", typeof(string), "Product types piped string")]
        public static object IsStaticProductTypeCombinationValid(EvaluationContext context, params object[] parameters)
        {
            if (parameters.Length != 1)
            {
                throw new NotSupportedException();
            }
            var selectedProductTypeCodeNames = ValidationHelper.GetString(parameters[0], "").Split("|".ToCharArray());
            // Static product - can be of type Inventory or can be print on demand (POD) or can be withh add-on
            if (selectedProductTypeCodeNames.Contains("KDA.StaticProduct"))
            {
                if (selectedProductTypeCodeNames.Contains("KDA.MailingProduct") ||
                    selectedProductTypeCodeNames.Contains("KDA.TemplatedProduct"))
                {
                    return false;
                }
            }
            return true;
        }

        [MacroMethod(typeof(bool), "Validates combination of product types - inventory type variant.", 1)]
        [MacroMethodParam(0, "productTypes", typeof(string), "Product types piped string")]
        public static object IsInventoryProductTypeCombinationValid(EvaluationContext context, params object[] parameters)
        {
            if (parameters.Length != 1)
            {
                throw new NotSupportedException();
            }
            var selectedProductTypeCodeNames = ValidationHelper.GetString(parameters[0], "").Split("|".ToCharArray());
            // Inventory product - Must be of type static
            if (selectedProductTypeCodeNames.Contains("KDA.InventoryProduct"))
            {
                if (!selectedProductTypeCodeNames.Contains("KDA.StaticProduct") ||
                    selectedProductTypeCodeNames.Contains("KDA.POD") ||
                    selectedProductTypeCodeNames.Contains("KDA.MailingProduct") ||
                    selectedProductTypeCodeNames.Contains("KDA.TemplatedProduct"))
                {
                    return false;
                }
            }
            return true;
        }

        [MacroMethod(typeof(bool), "Validates combination of product types - mailing type variant.", 1)]
        [MacroMethodParam(0, "productTypes", typeof(string), "Product types piped string")]
        public static object IsMailingProductTypeCombinationValid(EvaluationContext context, params object[] parameters)
        {
            if (parameters.Length != 1)
            {
                throw new NotSupportedException();
            }
            var selectedProductTypeCodeNames = ValidationHelper.GetString(parameters[0], "").Split("|".ToCharArray());
            // Mailing product - Must be of type Template
            if (selectedProductTypeCodeNames.Contains("KDA.MailingProduct"))
            {
                if (!selectedProductTypeCodeNames.Contains("KDA.TemplatedProduct") ||
                    selectedProductTypeCodeNames.Contains("KDA.StaticProduct") ||
                    selectedProductTypeCodeNames.Contains("KDA.InventoryProduct") ||
                    selectedProductTypeCodeNames.Contains("KDA.POD"))
                {
                    return false;
                }
            }
            return true;
        }

        [MacroMethod(typeof(bool), "Validates combination of product types - mailing type variant.", 1)]
        [MacroMethodParam(0, "productTypes", typeof(string), "Product types piped string")]
        public static object IsTemplatedProductTypeCombinationValid(EvaluationContext context, params object[] parameters)
        {
            if (parameters.Length != 1)
            {
                throw new NotSupportedException();
            }
            var selectedProductTypeCodeNames = ValidationHelper.GetString(parameters[0], "").Split("|".ToCharArray());
            // Templated product - Can be of type Mailing
            if (selectedProductTypeCodeNames.Contains("KDA.TemplatedProduct"))
            {
                if (selectedProductTypeCodeNames.Contains("KDA.StaticProduct") ||
                    selectedProductTypeCodeNames.Contains("KDA.InventoryProduct") ||
                    selectedProductTypeCodeNames.Contains("KDA.POD"))
                {
                    return false;
                }
            }
            return true;
        }

        [MacroMethod(typeof(string), "Validates whether product is an invertory product type.", 1)]
        [MacroMethodParam(0, "productType", typeof(string), "Current product type")]
        [MacroMethodParam(1, "numberOfAvailableProducts", typeof(object), "NumberOfAvailableProducts")]
        [MacroMethodParam(2, "cultureCode", typeof(string), "Current culture code")]
        public static object GetAvailableProductsString(EvaluationContext context, params object[] parameters)
        {
            if (parameters.Length != 3)
            {
                throw new NotSupportedException();
            }

            if (!ValidationHelper.GetString(parameters[0], "").Contains("KDA.InventoryProduct"))
            {
                return string.Empty;
            }
            else
            {
                string formattedValue = string.Empty;

                if (parameters[1] == null)
                {
                    formattedValue = ResHelper.GetString("Kadena.Product.Unavailable", ValidationHelper.GetString(parameters[2], ""));
                }
                else if ((int)parameters[1] == 0)
                {
                    formattedValue = ResHelper.GetString("Kadena.Product.OutOfStock", ValidationHelper.GetString(parameters[2], ""));
                }
                else
                {
                    formattedValue = string.Format(
                    ResHelper.GetString("Kadena.Product.NumberOfAvailableProducts", ValidationHelper.GetString(parameters[2], "")),
                    ValidationHelper.GetString(parameters[1], ""));
                }

                return formattedValue;
            }
        }

        [MacroMethod(typeof(string), "Gets appropriate css class for label that holds amount of products in stock", 1)]
        [MacroMethodParam(0, "numberOfAvailableProducts", typeof(object), "NumberOfAvailableProducts")]
        [MacroMethodParam(1, "productType", typeof(string), "Current product type")]
        public static object GetAppropriateCssClassOfAvailability(EvaluationContext context, params object[] parameters)
        {
            if (parameters.Length != 2)
            {
                throw new NotSupportedException();
            }

            if (ValidationHelper.GetString(parameters[1], "").Contains("KDA.InventoryProduct"))
            {
                if (parameters[0] == null)
                {
                    return "stock stock--unavailable";
                }

                if ((int)parameters[0] == 0)
                {
                    return "stock stock--out";
                }              
                
                return "stock stock--available";
                
            }

            return string.Empty;
        }

        [MacroMethod(typeof(string), "Returns html (set) of products, that could be in a kit with particular product (for particular user).", 1)]
        [MacroMethodParam(0, "nodeID", typeof(int), "ID of Node, that represents the base product")]
        [MacroMethodParam(1, "productsAliasPath", typeof(string), "Alias path for products")]
        public static object GetKitProductsHtml(EvaluationContext context, params object[] parameters)
        {
            var result = string.Empty;
            var selectedItemTemplate = "<div class=\"input__wrapper input__wrapper--disabled\"><input id=\"dom-0\" type=\"checkbox\" class=\"input__checkbox\" data-id=\"{1}\" checked disabled><label for=\"dom-0\" class=\"input__label input__label--checkbox\">{0}</label></div>";
            var itemTemplate = "<div class=\"input__wrapper\"><input id=\"dom-{0}\" type=\"checkbox\" class=\"input__checkbox\" data-id=\"{2}\"><label for=\"dom-{0}\" class=\"input__label input__label--checkbox\">{1}</label></div>";

            if (parameters.Length != 2)
            {
                throw new NotSupportedException();
            }
            var originalNodeID = ValidationHelper.GetInteger(parameters[0], 0);
            var productsPath = ValidationHelper.GetString(parameters[1], string.Empty);

            var tree = new TreeProvider(MembershipContext.AuthenticatedUser);

            if (originalNodeID > 0)
            {
                var originalDocument = tree.SelectSingleNode(originalNodeID, LocalizationContext.CurrentCulture.CultureCode);
                result += string.Format(selectedItemTemplate, originalDocument.DocumentName, originalDocument.NodeID);
            }
            var kitDocuments = tree.SelectNodes()
                .OnCurrentSite()
                .Path(productsPath, PathTypeEnum.Children)
                .Culture(LocalizationContext.CurrentCulture.CultureCode)
                .Types("KDA.Product")
                .CheckPermissions()
                .WhereLike("ProductType", "%KDA.InventoryProduct%").Or().WhereLike("ProductType", "%KDA.POD%").Or().WhereLike("ProductType", "%KDA.StaticProduct%");

            if (kitDocuments != null)
            {
                var kitList = kitDocuments.ToList();
                for (int i = 1; i <= kitList.Count; i++)
                {
                    var node = kitList[i - 1];
                    if (node.NodeID != originalNodeID && !node.IsLink)
                    {
                        result += string.Format(itemTemplate, i, node.DocumentName, node.NodeID);
                    }
                }
            }
            return result;
        }
    }
}