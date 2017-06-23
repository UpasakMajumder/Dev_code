using CMS.DocumentEngine;
using CMS.Ecommerce;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.PortalEngine.Web.UI;
using Kadena.Old_App_Code.Kadena.Chili;
using System;
using System.Linq;
using System.Web;

namespace Kadena.CMSWebParts.Kadena.Chili
{
    public partial class OpenTemplatedProductButton : CMSAbstractWebPart
    {
        #region Public properties

        public string ProductEditorUrl
        {
            get
            {
                return GetStringValue("ProductEditorUrl", string.Empty);
            }
        }

        #endregion
        public string SelectMailingListUrl
        {
            get
            {
                return GetStringValue("SelectMailingListUrl", string.Empty);
            }
        }

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
                btnOpenTemplatedProduct.Text = ResHelper.GetString("Kadena.Product.OpenTemplateInDesign", LocalizationContext.CurrentCulture.CultureCode);
            }
        }

        #endregion

        #region Event handlers

        protected void btnOpenTemplatedProduct_Click(object sender, EventArgs e)
        {
            var masterTemplateID = CurrentDocument.GetStringValue("ProductChiliTemplateID", string.Empty);
            var workspaceID = CurrentDocument.GetStringValue("ProductChiliWorkgroupID", string.Empty);
            var newTemplateUrl = new TemplateServiceHelper().CreateNewTemplate(MembershipContext.AuthenticatedUser.UserID, masterTemplateID, workspaceID);
            if (!string.IsNullOrEmpty(newTemplateUrl))
            {
                var uri = new Uri(newTemplateUrl);
                var newTemplateID = HttpUtility.ParseQueryString(uri.Query).Get("doc");
                var destinationUrl = String.Format("{0}?id={1}&skuid={2}&templateid={3}&workspaceid={4}",
                  ProductEditorUrl,
                  DocumentContext.CurrentDocument.DocumentID,
                  ECommerceContext.CurrentProduct.SKUID,
                  newTemplateID,
                  workspaceID);

                var productTypes = DocumentContext.CurrentDocument.GetValue("ProductType").ToString().Split('|').ToLookup(s => s);
                if (productTypes.Contains("KDA.MailingProduct") && productTypes.Contains("KDA.TemplatedProduct"))
                {
                    var selectMailingList = URLHelper.AddParameterToUrl(SelectMailingListUrl, "url", URLHelper.URLEncode(destinationUrl));
                    Response.Redirect(selectMailingList);
                }
                else
                {
                    Response.Redirect(destinationUrl);
                }
            }
            else
            {
                btnOpenTemplatedProduct.Attributes.Clear();
                btnOpenTemplatedProduct.Attributes.Add("class", "btn-action btn-action input--error");
                spanErrorMessage.Visible = true;
            }
        }

        #endregion

    }
}