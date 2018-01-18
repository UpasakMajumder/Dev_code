using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.PortalEngine.Web.UI;
using Kadena.Dto.TemplatedProduct.MicroserviceRequests;
using Kadena.Models.Product;
using Kadena.Old_App_Code.Kadena;
using Kadena.WebAPI.KenticoProviders;
using Kadena2.MicroserviceClients.Clients;
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
            var use3d = CurrentDocument.GetBooleanValue("ProductChili3dEnabled", false);
            var resource = new KenticoResourceService();
            var client = new TemplatedClient(ProviderFactory.SuppliantDomain, ProviderFactory.MicroProperties);
            var requestBody = new NewTemplateRequestDto
            {
                User = MembershipContext.AuthenticatedUser.UserID.ToString(),
                TemplateId = masterTemplateID,
                WorkSpaceId = workspaceID,
                UseHtmlEditor = false,
                Use3d = use3d
            };
            var newTemplateUrl = client.CreateNewTemplate(requestBody).Result?.Payload;
            if (!string.IsNullOrEmpty(newTemplateUrl))
            {
                var uri = new Uri(newTemplateUrl);
                var newTemplateID = HttpUtility.ParseQueryString(uri.Query).Get("doc");
                var destinationUrl = string.Format("{0}?nodeId={1}&templateId={2}&workspaceid={3}&use3d={4}",
                  ProductEditorUrl,
                  DocumentContext.CurrentDocument.NodeID,
                  newTemplateID,
                  workspaceID,
                  use3d);

                var productTypes = DocumentContext.CurrentDocument.GetValue("ProductType").ToString().Split('|').ToLookup(s => s);
                if (productTypes.Contains(ProductTypes.MailingProduct) && productTypes.Contains(ProductTypes.TemplatedProduct))
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