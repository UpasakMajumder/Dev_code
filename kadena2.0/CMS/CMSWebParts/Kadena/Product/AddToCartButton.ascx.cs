using CMS.Base.Web.UI;
using CMS.DocumentEngine;
using CMS.Ecommerce;
using CMS.EventLog;
using CMS.Helpers;
using CMS.Localization;
using CMS.PortalEngine.Web.UI;
using Kadena.Models;
using Kadena.Old_App_Code.Kadena.DynamicPricing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;

namespace Kadena.CMSWebParts.Kadena.Product
{
    public partial class AddToCartButton : CMSAbstractWebPart
    {
        #region Public methods

        public override void OnContentLoaded()
        {
            base.OnContentLoaded();
            SetupControl();
        }

        protected void SetupControl()
        {
            if (!StopProcessing)
            {
                SetupNumberOfItemsInPackageInformation();
                
                if (IsProductInventoryType() && IsStockEmpty())
                {
                    this.Visible = false;
                }

                Controls.Add(new LiteralControl(GetHiddenInput("documentId", DocumentContext.CurrentDocument.DocumentID.ToString())));
            }
        }

        #endregion
    
        #region Private methods

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
            if (DocumentContext.CurrentDocument.GetValue("SKUAvailableItems") != null)
            {
                return (int)DocumentContext.CurrentDocument.GetValue("SKUAvailableItems") == 0;
            }

            return true;
        }

        private bool IsProductInventoryType()
        {
            if (DocumentContext.CurrentDocument.GetValue("ProductType") != null)
            {
                return DocumentContext.CurrentDocument.GetValue("ProductType").ToString().Contains(ProductTypes.InventoryProduct);
            }

            return false;
        }

        private void SetupNumberOfItemsInPackageInformation()
        {
            if (DocumentContext.CurrentDocument.GetIntegerValue("ProductNumberOfItemsInPackage", 0) == 0 ||
              DocumentContext.CurrentDocument.GetIntegerValue("ProductNumberOfItemsInPackage", 0) == 1)
            {
                lblNumberOfItemsInPackageInfo.Visible = false;
            }
            else
            {
                lblNumberOfItemsInPackageInfo.Text = string.Format(ResHelper.GetString("Kadena.Product.NumberOfItemsInPackagesFormatString2", LocalizationContext.CurrentCulture.CultureCode), DocumentContext.CurrentDocument.GetIntegerValue("ProductNumberOfItemsInPackage", 0));
            }
        }

        #endregion
    }
}