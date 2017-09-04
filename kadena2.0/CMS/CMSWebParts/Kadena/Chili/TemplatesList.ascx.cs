using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.EventLog;
using CMS.Membership;
using CMS.PortalEngine.Web.UI;
using CMS.SiteProvider;
using Kadena.Old_App_Code.Kadena.Chili;
using Kadena2.MicroserviceClients.Clients;
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
            var clientEndpoint = SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.KDA_TemplatingServiceEndpoint");
            var client = new TemplatedProductService();
            var requestResult = client
                .GetTemplates(clientEndpoint,
                    MembershipContext.AuthenticatedUser.UserID,
                    DocumentContext.CurrentDocument.GetGuidValue("ProductChiliTemplateID", Guid.Empty))
                .Result;

            if (requestResult.Success)
            {
                var templatesData = requestResult.Payload;

                if ((templatesData?.Count ?? 0) > 0)
                {
                    repTemplates.DataSource =
                        templatesData
                        .Select(d => new
                        {
                            EditorUrl = string.Format("{0}?documentId={1}&templateId={2}&workspaceid={3}{4}&quantity={5}{6}",
                                ProductEditorUrl,
                                DocumentContext.CurrentDocument.DocumentID,
                                d.TemplateId,
                                DocumentContext.CurrentDocument.GetStringValue("ProductChiliWorkgroupID", string.Empty),
                                string.IsNullOrWhiteSpace(d.MailingList?.ContainerId) ? string.Empty : $"&containerId={d.MailingList.ContainerId}",
                                (d.MailingList?.RowCount ?? 0).ToString(),
                                string.IsNullOrWhiteSpace(d.Name) ? string.Empty : $"&customName={d.Name}"
                                ),
                            TemplateID = d.TemplateId,
                            DateCreated = DateTime.Parse(d.Created),
                            DateUpdated = DateTime.Parse(d.Updated),
                            Name = d.Name
                        })
                        .OrderByDescending(t => t.DateCreated);
                    repTemplates.DataBind();
                }
                else
                {
                    this.Visible = false;
                }
            }
            else
            {
                this.Visible = false;
                EventLogProvider.LogEvent("E", "GET TEMPLATE LIST", "EXCEPTION", requestResult.ErrorMessages);
            }
        }

        #endregion
    }
}