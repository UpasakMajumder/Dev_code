using CMS.PortalEngine.Web.UI;
using Kadena.Container.Default;
using System;
using Kadena2.MicroserviceClients.Contracts;
using Kadena.Helpers;
using Kadena.Dto.MailingList.MicroserviceResponses;

namespace Kadena.CMSWebParts.Kadena.MailingList
{
    public partial class Downloader : CMSAbstractWebPart
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Visible = false;
            if (!string.IsNullOrWhiteSpace(Request.QueryString["containerId"]))
            {
                var containerId = new Guid(Request.QueryString["containerId"]);
                var mailingClient = DIContainer.Resolve<IMailingListClient>();
                var mailingList = mailingClient.GetMailingList(containerId).Result;
                if (mailingList.Success && mailingList.Payload.State.Equals(ContainerState.AddressesVerified))
                {
                    hlnkDownload.NavigateUrl = UrlHelper.GetMailingListExportUrl(containerId).OriginalString;
                    hlnkDownload.Attributes.Add("download", mailingList.Payload.Name);
                    Visible = true;
                }
            }
        }
    }
}