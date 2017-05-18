using CMS.DataEngine;
using CMS.EventLog;
using CMS.SiteProvider;
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
    /// <returns>Editor url (for iframe)</returns>
    public string CreateNewTemplate(int userID, string masterTemplateID)
    {
      var requestUrl = string.Format("{0}api/template", ServiceBaseUrl);
      var request = (HttpWebRequest)WebRequest.Create(requestUrl);
      request.ContentType = "application/json";
      request.Method = "POST";

      using (var streamWriter = new StreamWriter(request.GetRequestStream()))
      {
        var data = new NewTemplateRequestData { user = userID.ToString(), templateId = masterTemplateID };
        streamWriter.Write(new JavaScriptSerializer().Serialize(data));
      }

      var response = (HttpWebResponse)request.GetResponse();
      if (response.StatusCode == HttpStatusCode.OK)
      {
        var resultString = string.Empty;
        using (var streamReader = new StreamReader(response.GetResponseStream()))
        {
          resultString = streamReader.ReadToEnd();
        }
        var result = new JavaScriptSerializer().Deserialize<TemplateServiceResponseData>(resultString);

        if (result.success)
        {
          return result.payload.editorUrl;
        }
        else
        {
          EventLogProvider.LogEvent("E", "TEMPLATE SERVICE HELPER - CREATE NEW TEMPLATE", "ERROR", result.error);
        }
      }
      else
      {
        EventLogProvider.LogEvent("E", "TEMPLATE SERVICE HELPER - CREATE NEW TEMPLATE", "ERROR", response.StatusCode.ToString());
      }
      return string.Empty;
    }

    /// <summary>
    /// Returns editor/iframe url for the given template ID
    /// </summary>
    /// <param name="templateID"></param>
    /// <returns>iframe/editor url</returns>
    public string GetEditorUrl(string templateID)
    {
      var requestUrl = string.Format("{0}api/template/{1}", ServiceBaseUrl, templateID);
      var request = (HttpWebRequest)WebRequest.Create(requestUrl);
      request.ContentType = "application/json";
      request.Method = "GET";

      var response = (HttpWebResponse)request.GetResponse();
      if (response.StatusCode == HttpStatusCode.OK)
      {
        string resultString = string.Empty;

        using (var streamReader = new StreamReader(response.GetResponseStream()))
        {
          resultString = streamReader.ReadToEnd();
        }
        var result = new JavaScriptSerializer().Deserialize<TemplateServiceResponseData>(resultString);

        if (result.success)
        {
          return result.payload.editorUrl;
        }
        else
        {
          EventLogProvider.LogEvent("E", "TEMPLATE SERVICE HELPER - GET EDITOR URL", "ERROR", result.error);
        }
      }
      else
      {
        EventLogProvider.LogEvent("E", "TEMPLATE SERVICE HELPER - GET EDITOR URL", "ERROR", response.StatusCode.ToString());
      }
      return string.Empty;
    }

    /// <summary>
    /// Returns all copies for master templete for given user
    /// </summary>
    /// <param name="userID"></param>
    /// <param name="masterTemplateID"></param>
    /// <returns>List of template data</returns>
    public List<TemplateServiceDocumentResponse> GetMasterTemplateCopies(int userID, string masterTemplateID)
    {
      var requestUrl = string.Format("{0}api/template/{1}/users/{2}", ServiceBaseUrl, masterTemplateID, userID);
      var request = (HttpWebRequest)WebRequest.Create(requestUrl);
      request.ContentType = "application/json";
      request.Method = "GET";

      var response = (HttpWebResponse)request.GetResponse();
      if (response.StatusCode == HttpStatusCode.OK)
      {
        string resultString = string.Empty;

        using (var streamReader = new StreamReader(response.GetResponseStream()))
        {
          resultString = streamReader.ReadToEnd();
        }
        var result = new JavaScriptSerializer().Deserialize<TemplateServiceListResponseData>(resultString);

        if (result.success)
        {
          return result.payload.documents;
        }
        else
        {
          EventLogProvider.LogEvent("E", "TEMPLATE SERVICE HELPER - GET MASTER TEMPLATE COPIES", "ERROR", result.error);
        }
      }
      else
      {
        EventLogProvider.LogEvent("E", "TEMPLATE SERVICE HELPER - GET MASTER TEMPLATE COPIES", "ERROR", response.StatusCode.ToString());
      }
      return new List<TemplateServiceDocumentResponse>();
    }
  }
}