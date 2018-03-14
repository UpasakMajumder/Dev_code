using CMS.PortalEngine.Web.UI;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Container.Default;
using Kadena.MicroserviceClients.Contracts;
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
                var resources = DIContainer.Resolve<IKenticoResourceService>();
                chilliIframe.Src = DIContainer.Resolve<ITemplatedClient>()
                    .GetEditorUrl(Guid.Parse(TemplateID), Guid.Parse(WorkspaceID), false, Use3d)
                    .Result?
                    .Payload ?? string.Empty;
            }
        }
    }
}