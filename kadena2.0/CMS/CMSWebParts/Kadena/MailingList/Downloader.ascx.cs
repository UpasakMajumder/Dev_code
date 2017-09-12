using CMS.DataEngine;
using CMS.PortalEngine.Web.UI;
using CMS.SiteProvider;
using Kadena2.MicroserviceClients.Clients;
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
                var mailingListUrl = SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.KDA_GetMailingListByIdUrl");
                var mailingListClient = new MailingListClient();

                var mailingListResponse = mailingListClient.GetMailingList(mailingListUrl, SiteContext.CurrentSiteName, _containerId).Result;
                if (mailingListResponse.Success)
                {
                    var mailingList = mailingListResponse.Payload;
                    var exportUrl = SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.KDA_ExportServiceUrl");
                    var exportClient = new ExportClient();
                    var exportResponse = exportClient.ExportMailingList(exportUrl, _containerId, SiteContext.CurrentSiteName).Result;
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
                }
            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            DownloadFile();
        }
    }
}