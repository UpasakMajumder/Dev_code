using CMS.DataEngine;
using CMS.EventLog;
using CMS.PortalEngine.Web.UI;
using CMS.SiteProvider;
using Kadena.WebAPI.Helpers;
using Kadena.WebAPI.KenticoProviders;
using Kadena2.MicroserviceClients.Clients;
using System;
using System.IO;

namespace Kadena.CMSWebParts.Kadena.MailingList
{
    public partial class Downloader : CMSAbstractWebPart
    {
        private Guid _containerId;
        private readonly string _mailingServiceUrlSettingKey = "KDA_MailingServiceUrl";

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
                var mailingServiceUrl = SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.{_mailingServiceUrlSettingKey}");
                var mailingListClient = new MailingListClient();

                var mailingListResponse = mailingListClient.GetMailingList(mailingServiceUrl, SiteContext.CurrentSiteName, _containerId).Result;
                if (mailingListResponse.Success)
                {
                    var mailingList = mailingListResponse.Payload;
                    var exportClient = new ExportClient(new MicroProperties(new KenticoResourceService()));
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