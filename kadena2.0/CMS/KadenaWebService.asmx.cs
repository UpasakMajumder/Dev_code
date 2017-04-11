namespace CMSApp
{
  using CMS.Membership;
  using System.Web.Script.Services;
  using System.Web.Services;

  [WebService]
  [ScriptService]
  public class KadenaWebService : System.Web.Services.WebService
  {
    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public void SignOut()
    {
      AuthenticationHelper.SignOut();
    }
  }
}
