using CMS.PortalEngine.Web.UI;
using System.Linq;
using Kadena2.MicroserviceClients.Clients;
using Kadena.Old_App_Code.Kadena.MailingList;
using CMS.SiteProvider;
using CMS.DataEngine;
using CMS.EventLog;

namespace Kadena.CMSWebParts.Kadena.MailingList
{
    public partial class MailingList : CMSAbstractWebPart
    {
        private readonly string _mailingServiceUrlSettingKey = "KDA_MailingServiceUrl";

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
            var mailingServiceUrl = SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.{_mailingServiceUrlSettingKey}");

            var client = new MailingListClient();
            var serviceCallResult = client.GetMailingListsForCustomer(mailingServiceUrl, SiteContext.CurrentSiteName).Result;

            if (serviceCallResult.Success)
            {
                repMailingLists.DataSource = serviceCallResult.Payload
                      .OrderByDescending(x => x.CreateDate)
                      .Select(l => new
                      {
                          l.Name,
                          l.CreateDate,
                          l.AddressCount,
                          l.Id,
                          ErrorCount = (l.State.Equals(MailingListState.AddressesNeedToBeVerified)
                          || l.State.Equals(MailingListState.AddressesOnVerification)) ? "N/A" : l.ErrorCount.ToString()
                      });
                repMailingLists.DataBind();
            }
            else
            {
                EventLogProvider.LogException("Mailing List", "GET DATA", new System.Exception(serviceCallResult.Error?.Message ?? string.Empty));
                inpError.Value = serviceCallResult.ErrorMessages;
            }            
        }

        #endregion
    }
}