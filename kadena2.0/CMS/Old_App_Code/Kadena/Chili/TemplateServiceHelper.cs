using CMS.DataEngine;
using CMS.EventLog;
using CMS.SiteProvider;
using Kadena.Dto.General;
using Kadena.Dto.TemplatedProduct.MicroserviceResponses;
using System.Collections.Generic;
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
        /// Creates new template from master template for particular user
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="masterTemplateID"></param>
        /// <param name="workspaceID"></param>
        /// <returns>Editor url (for iframe)</returns>
        public string CreateNewTemplate(int userID, string masterTemplateID, string workspaceID)
        {
            var requestUrl = string.Format("{0}api/template", ServiceBaseUrl);
            var request = (HttpWebRequest)WebRequest.Create(requestUrl);
            request.ContentType = "application/json";
            request.Method = "POST";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                var data = new NewTemplateRequestData { user = userID.ToString(), templateId = masterTemplateID, workSpaceId = workspaceID };
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
                        EventLogProvider.LogEvent("E", "TEMPLATE SERVICE HELPER - CREATE NEW TEMPLATE", "ERROR", result?.Error?.Message ?? string.Empty);
                    }
                }
                else
                {
                    EventLogProvider.LogEvent("E", "TEMPLATE SERVICE HELPER - CREATE NEW TEMPLATE", "ERROR", response.StatusCode.ToString());
                }
            }
            catch (WebException ex)
            {
                EventLogProvider.LogException("TEMPLATE SERVICE HELPER", "CREATE NEW TEMPLATE", ex);
            }
            return string.Empty;
        }

        /// <summary>
        /// Returns editor/iframe url for the given template ID
        /// </summary>
        /// <param name="templateID"></param>
        /// <returns>iframe/editor url</returns>
        public string GetEditorUrl(string templateID, string workspaceID)
        {
            var requestUrl = string.Format("{0}api/template/{1}/workspace/{2}", ServiceBaseUrl, templateID, workspaceID);
            var request = (HttpWebRequest)WebRequest.Create(requestUrl);
            request.ContentType = "application/json";
            request.Method = "GET";

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
                        EventLogProvider.LogEvent("E", "TEMPLATE SERVICE HELPER - GET EDITOR URL", "ERROR", result?.Error?.Message ?? string.Empty);
                    }
                }
                else
                {
                    EventLogProvider.LogEvent("E", "TEMPLATE SERVICE HELPER - GET EDITOR URL", "ERROR", response.StatusCode.ToString());
                }
            }
            catch (WebException ex)
            {
                EventLogProvider.LogException("TEMPLATE SERVICE HELPER", "GET EDITOR URL", ex);
            }
            return string.Empty;
        }

        /// <summary>
        /// Assign specified container to specified template.
        /// </summary>
        /// <param name="containerId">Id of container.</param>
        /// <param name="templateId">Id of template.</param>
        /// <param name="workspaceId">Id of template workspace</param>
        /// <returns>Url to Chili's editor.</returns>
        public string SetMailingList(string containerId, string templateId, string workSpaceId)
        {
            var requestUrl = string.Format("{0}api/template/datasource", ServiceBaseUrl);
            var request = (HttpWebRequest)WebRequest.Create(requestUrl);
            request.ContentType = "application/json";
            request.Method = "POST";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                var data = new { containerId, templateId, workSpaceId };
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