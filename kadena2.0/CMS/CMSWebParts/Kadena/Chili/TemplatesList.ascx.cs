using CMS.DocumentEngine;
using CMS.Ecommerce;
using CMS.Membership;
using CMS.PortalEngine.Web.UI;
using Kadena.Old_App_Code.Kadena.Chili;
using System;
using System.Linq;

namespace Kadena.CMSWebParts.Kadena.Chili
{
    public partial class TemplatesList : CMSAbstractWebPart
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
                SetupTemplatesList();
            }
        }

        #endregion

        #region Private methods

        private void SetupTemplatesList()
        {
            var templatesData = new TemplateServiceHelper()
                .GetMasterTemplateCopies(MembershipContext.AuthenticatedUser.UserID, DocumentContext.CurrentDocument.GetStringValue("ProductChiliTemplateID", string.Empty));

            if ((templatesData?.Count ?? 0) > 0)
            {
                repTemplates.DataSource = templatesData
                    .Select(d => new
                    {
                        EditorUrl = string.Format("{0}?id={1}&skuid={2}&templateid={3}&workspaceid={4}&containerId={5}", ProductEditorUrl, DocumentContext.CurrentDocument.DocumentID, ECommerceContext.CurrentProduct.SKUID, d.templateId, DocumentContext.CurrentDocument.GetStringValue("ProductChiliWorkgroupID", string.Empty), d.MailingList?.ContainerID ?? string.Empty),
                        TemplateID = d.templateId,
                        Date = DateTime.Parse(d.created).ToString()
                    })
                    .OrderByDescending(t => t.Date);

                repTemplates.DataBind();
            }
            else
            {
                this.Visible = false;
            }
        }

        #endregion
    }
}