using CMS.DataEngine;
using CMS.PortalEngine.Web.UI;
using CMS.SiteProvider;
using Kadena.Helpers;
using Kadena.WebAPI.KenticoProviders;
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
        
        public override void OnContentLoaded()
        {
            base.OnContentLoaded();
            SetupControl();
        }

        protected void SetupControl()
        {
            if (!StopProcessing && TemplateID != null)
            {
                var resources = new KenticoResourceService();
                chilliIframe.Src = new TemplatedClient(new SuppliantDomain(resources), new MicroProperties(resources))
                    .GetEditorUrl(Guid.Parse(TemplateID), Guid.Parse(WorkspaceID), false, Use3d)
                    .Result?
                    .Payload ?? string.Empty;
            }
        }
    }
}