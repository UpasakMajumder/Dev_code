using CMS.PortalEngine.Web.UI;
using Kadena.Container.Default;
using System;
using Kadena2.MicroserviceClients.Contracts;
using Kadena.Old_App_Code.Kadena.MailingList;
using Kadena.Helpers;

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
                if (mailingList.Success && mailingList.Payload.State.Equals(MailingListState.AddressesVerified))
                {
                    hlnkDownload.NavigateUrl = UrlHelper.GetMailingListExportUrl(containerId).OriginalString;
                    hlnkDownload.Attributes.Add("download", mailingList.Payload.Name);
                    Visible = true;
                }
            }
        }
    }
}