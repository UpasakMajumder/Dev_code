using CMS.EventLog;
using CMS.PortalEngine.Web.UI;
using CMS.SiteProvider;
using Kadena.Old_App_Code.Kadena;
using Kadena2.Container.Default;
using Kadena2.MicroserviceClients.Clients;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.MicroserviceClients.Contracts.Base;
using System;
using System.IO;

namespace Kadena.CMSWebParts.Kadena.MailingList
{
    public partial class Downloader : CMSAbstractWebPart
    {
        private Guid _containerId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Request.QueryString["containerId"]))
            {
                _containerId = new Guid(Request.QueryString["containerId"]);
            }
        }

        private void DownloadFile()
        {
            if (_containerId != Guid.Empty)
            {
                var mailingListClient = DIContainer.Resolve<IMailingListClient>();

                var mailingListResponse = mailingListClient.GetMailingList(_containerId).Result;
                if (mailingListResponse.Success)
                {
                    var mailingList = mailingListResponse.Payload;
                    var exportClient = DIContainer.Resolve<IExportClient>();
                    var exportResponse = exportClient.ExportMailingList(_containerId, SiteContext.CurrentSiteName).Result;
                    if (exportResponse.Success)
                    {
                        var exportedStream = exportResponse.Payload;
                        Response.Clear();
                        Response.ContentType = "text/csv";
                        Response.AddHeader("Content-Disposition", $"attachment; filename=\"{mailingList.Name}.csv\";");
                        Response.BufferOutput = false;
                        exportedStream.Seek(0, SeekOrigin.Begin);
                        exportedStream.CopyTo(Response.OutputStream);
                        Response.End();
                    }
                    else
                    {
                        EventLogProvider.LogEvent(EventType.ERROR, GetType().Name, "ExportClient", exportResponse.ErrorMessages);
                    }
                }
                else
                {
                    EventLogProvider.LogEvent(EventType.ERROR, GetType().Name, "MailingListClient", mailingListResponse.ErrorMessages);
                }
            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            DownloadFile();
        }
    }
}