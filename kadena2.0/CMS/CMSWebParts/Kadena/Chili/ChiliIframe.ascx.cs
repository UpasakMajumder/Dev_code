using CMS.PortalEngine.Web.UI;
using Kadena.Old_App_Code.Kadena.Chili;

namespace Kadena.CMSWebParts.Kadena.Chili
{
  public partial class ChiliIframe : CMSAbstractWebPart
  {
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
        chilliIframe.Src = new TemplateServiceHelper().GetEditorUrl(TemplateID);
      }
    }
  }
}