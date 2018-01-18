using CMS.PortalEngine.Web.UI;
using System.Linq;
using Kadena2.MicroserviceClients.Clients;
using Kadena.Old_App_Code.Kadena.MailingList;
using CMS.EventLog;
using Kadena.Old_App_Code.Kadena;
using Kadena2.Container.Default;
using Kadena2.MicroserviceClients.Contracts.Base;

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
            var client = new MailingListClient(ContainerBuilder.Resolve<IMicroProperties>());
            var serviceCallResult = client.GetMailingListsForCustomer().Result;

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