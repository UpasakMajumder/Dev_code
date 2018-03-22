using CMS.UIControls;
using Kadena.Container.Default;
using Kadena.Helpers.Data;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Data;
using System.Linq;

namespace Kadena.CMSModules.Kadena.Pages.Orders
{
    public partial class FailedOrders : CMSPage
    {
        protected const string ActionResubmit = "resubmitOrder";

        public IKenticoLogger Logger { get; set; } = DIContainer.Resolve<IKenticoLogger>();

        protected void Page_Load(object sender, EventArgs e)
        {
            HideMessage();

            WireupEvents();
        }

        private void WireupEvents()
        {
            grdOrders.OnDataReload += GrdOrders_OnDataReload;
            grdOrders.OnAction += GrdOrders_OnAction;
        }

        private void GrdOrders_OnAction(string actionName, object actionArgument)
        {
            try
            {
                switch (actionName)
                {
                    case ActionResubmit:
                        ResubmitOrder((string)actionArgument);
                        ShowMessage($"Order #{actionArgument} resubmitted");
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"There was an error. '{ex.Message}'");
                Logger.LogException(nameof(FailedOrders), ex);
            }
        }

        private void ShowMessage(string messageText)
        {
            messageContainer.Visible = true;
            message.Text = messageText;
        }

        private void HideMessage() => messageContainer.Visible = false;

        private void ResubmitOrder(string orderId)
        {
            // TODO
        }

        private System.Data.DataSet GrdOrders_OnDataReload(string completeWhere, string currentOrder, int currentTopN,
            string columns, int currentOffset, int currentPageSize, ref int totalRecords)
        {
            totalRecords = 1000;

            var items = Enumerable.Range(1, totalRecords)
                .Skip(currentOffset)
                .Take(currentPageSize)
                .Select(num => new
                {
                    Id = num,
                    SiteName = "",
                    OrderDate = DateTime.Now,
                    TotalPrice = 0m,
                    SubmissionAttemptsCount = num % 3,
                    OrderStatus = ""
                });

            var dataset = items.ToDataSet();
            return dataset;
        }
    }
}