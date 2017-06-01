namespace Kadena.Old_App_Code.Kadena.Chili
{
  public class TemplateServiceResponseData
  {
    public TemplateServiceResponsePayload payload { get; set; }
    public bool success { get; set; }
    public string error { get; set; }
  }

  public class TemplateServiceResponsePayload
  {
    public string editorUrl { get; set; }
  }
}