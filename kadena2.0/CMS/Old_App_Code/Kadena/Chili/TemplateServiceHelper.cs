using CMS.DataEngine;
using CMS.EventLog;
using CMS.SiteProvider;
using Kadena.Dto.General;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
namespace Kadena.Old_App_Code.Kadena.Chili
{
    public class TemplateServiceHelper
    {
        public string ServiceBaseUrl
        {
            get
            {
                return SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_TemplatingServiceEndpoint");
            }
        }

        /// <summary>
        /// Assign specified container to specified template.
        /// </summary>
        /// <param name="containerId">Id of container.</param>
        /// <param name="templateId">Id of template.</param>
        /// <param name="workspaceId">Id of template workspace</param>
        /// <returns>Url to Chili's editor.</returns>
        public string SetMailingList(string containerId, string templateId, string workSpaceId, bool use3d)
        {
            var requestUrl = string.Format("{0}api/template/datasource", ServiceBaseUrl);
            var request = (HttpWebRequest)WebRequest.Create(requestUrl);
            request.ContentType = "application/json";
            request.Method = "POST";
            request.Headers.Add("suppliantDomain", CMS.Helpers.RequestContext.CurrentDomain);

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                var data = new { containerId, templateId, workSpaceId, use3d };
                streamWriter.Write(new JavaScriptSerializer().Serialize(data));
            }

            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = (BaseResponseDto<string>)response;

                    if (result?.Success ?? false)
                    {
                        return result.Payload;
                    }
                    else
                    {
                        EventLogProvider.LogEvent("E", "TEMPLATE SERVICE HELPER - SET MAILING LIST", "ERROR", result?.Error?.Message ?? string.Empty);
                    }
                }
                else
                {
                    EventLogProvider.LogEvent("E", "TEMPLATE SERVICE HELPER - SET MAILING LIST", "ERROR", response.StatusCode.ToString());
                }
            }
            catch (WebException ex)
            {
                EventLogProvider.LogException("TEMPLATE SERVICE HELPER", "SET MAILING LIST", ex);
            }
            return string.Empty;
        }
    }
}