using CMS.PortalEngine.Web.UI;
using System.Linq;

namespace Kadena.CMSWebParts.Kadena.MailingList
{
  public partial class MailingList : CMSAbstractWebPart
  {
        public string ViewListUrl
        {
            get
            {
                return GetStringValue("ViewListUrl", string.Empty);
            }
        }

    #region Public methods

    public override void OnContentLoaded()
    {
      base.OnContentLoaded();
      SetupControl();
    }

    protected void SetupControl()
    {
      if (!StopProcessing)
      {
        GetMailingLists();
      }
    }

    #endregion

    #region Private methods

    private void GetMailingLists()
    {
      var mailingListData = Old_App_Code.Helpers.ServiceHelper.GetMailingLists();
      repMailingLists.DataSource = mailingListData.OrderByDescending(x => x.createDate);
      repMailingLists.DataBind();
    }

    #endregion
  }
}