using Kadena.Old_App_Code.CMSModules.Macros.Kadena;
using CMS.Helpers;
using CMS.MacroEngine;
using System;
using System.Linq;

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
      // Static product - can be of type Inventory or can be print on demand (POD)
      if (selectedProductTypeCodeNames.Contains("KDA.StaticProduct"))
      {
        if (selectedProductTypeCodeNames.Contains("KDA.MailingProduct") ||
            selectedProductTypeCodeNames.Contains("KDA.TemplatedProduct") ||
            selectedProductTypeCodeNames.Contains("KDA.ProductWithAddOns"))
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
            selectedProductTypeCodeNames.Contains("KDA.TemplatedProduct") ||
            selectedProductTypeCodeNames.Contains("KDA.ProductWithAddOns"))
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
            selectedProductTypeCodeNames.Contains("KDA.POD") ||
            selectedProductTypeCodeNames.Contains("KDA.ProductWithAddOns"))
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
            selectedProductTypeCodeNames.Contains("KDA.POD") ||
            selectedProductTypeCodeNames.Contains("KDA.ProductWithAddOns"))
        {
          return false;
        }
      }
      return true;
    }
  }
}