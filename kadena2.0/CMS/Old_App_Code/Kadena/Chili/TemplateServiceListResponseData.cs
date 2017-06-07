using System.Collections.Generic;

namespace Kadena.Old_App_Code.Kadena.Chili
{
  public class TemplateServiceListResponseData
  {
    public List<TemplateServiceDocumentResponse> payload { get; set; }
    public bool success { get; set; }
    public string error { get; set; }
  }

  public class TemplateServiceListResponsePayload
  {
    public List<TemplateServiceDocumentResponse> documents { get; set; }
  }

  public class TemplateServiceDocumentResponse
  {
    public string templateId { get; set; }
    public string masterTemplateId { get; set; }
    public string user { get; set; }
    public string created { get; set; }
  }
}