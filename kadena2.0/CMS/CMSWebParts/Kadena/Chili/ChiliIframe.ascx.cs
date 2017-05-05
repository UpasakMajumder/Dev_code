using CMS.DataEngine;
using CMS.EventLog;
using CMS.PortalEngine.Web.UI;
using CMS.SiteProvider;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kadena.CMSWebParts.Kadena.Chili
{
  public partial class ChiliIframe : CMSAbstractWebPart
  {
    private class ChiliServiceResponse
    {
      public ChiliPayload payload { get; set; }
      public bool success { get; set; }
      public string error { get; set; }
    }

    private class ChiliPayload
    {
      public string editorUrl { get; set; }
    }

    private const string _TemplateIDKey = "templateid";
    private string TemplateID
    {
      get
      {
        return Request.QueryString[_TemplateIDKey];
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
        chilliIframe.Src = GetChiliEditorUrl(TemplateID);
      }
    }

    private string GetChiliEditorUrl(string templateID)
    {
      var requestUrl = string.Format("{0}api/template/{1}", SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_TemplatingServiceEndpoint"), templateID);
      var request = (HttpWebRequest)WebRequest.Create(requestUrl);
      request.ContentType = "application/json";
      request.Method = "GET";

      var response = (HttpWebResponse)request.GetResponse();
      string resultString = string.Empty;

      using (var streamReader = new StreamReader(response.GetResponseStream()))
      {
        resultString = streamReader.ReadToEnd();
      }
      var result = new JavaScriptSerializer().Deserialize<ChiliServiceResponse>(resultString);

      if (!result.success)
      {
        EventLogProvider.LogEvent("E", "Chili editor iframe", "ERROR", result.error);
      }
      return result.payload.editorUrl;
    }
  }
}