using CMS.PortalEngine.Web.UI;
using System.Linq;
using Kadena2.MicroserviceClients.Clients;
using CMS.SiteProvider;
using CMS.DataEngine;

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
            var url = SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.KDA_GetMailingListsUrl");

            var client = new MailingListClient();
            var mailingListData = client.GetMailingListsForCustomer(url, SiteContext.CurrentSiteName).Result.Payload;
      repMailingLists.DataSource = mailingListData.OrderByDescending(x => x.CreateDate);
      repMailingLists.DataBind();
    }

    #endregion
  }
}