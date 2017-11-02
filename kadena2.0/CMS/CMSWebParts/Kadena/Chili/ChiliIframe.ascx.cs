using CMS.DataEngine;
using CMS.PortalEngine.Web.UI;
using CMS.SiteProvider;
using Kadena2.MicroserviceClients.Clients;
using System;

namespace Kadena.CMSWebParts.Kadena.Chili
{
    public partial class ChiliIframe : CMSAbstractWebPart
    {
        private const string _TemplateIDKey = "templateid";
        private const string _TemplateWorkspaceID = "workspaceid";
        private const string _Use3dID = "use3d";

        private string TemplateID
        {
            get
            {
                return Request.QueryString[_TemplateIDKey];
            }
        }

        private string WorkspaceID
        {
            get
            {
                return Request.QueryString[_TemplateWorkspaceID];
            }
        }

        private bool Use3d
        {
            get
            {
                return (Request?.QueryString?[_Use3dID] ?? string.Empty).ToLower() == "true";
            }
        }

        public string ServiceBaseUrl
        {
            get
            {
                return SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_TemplatingServiceEndpoint");
            }
        }

        public override void OnContentLoaded()
        {
            base.OnContentLoaded();
            SetupControl();
        }

        protected void SetupControl()
        {
            if (!StopProcessing && TemplateID != null)
            {
                chilliIframe.Src = new TemplatedClient() { SuppliantDomain = CMS.Helpers.RequestContext.CurrentDomain }.GetEditorUrl(ServiceBaseUrl, Guid.Parse(TemplateID), Guid.Parse(WorkspaceID), false, Use3d).Result?.Payload ?? string.Empty;
            }
        }
    }
}