using CMS.Base.Web.UI;
using CMS.Helpers;
using CMS.UIControls;
using Kadena.BusinessLogic.Contracts.Orders;
using Kadena.BusinessLogic.Services.Orders;
using Kadena.Container.Default;
using Kadena.Helpers.Data;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace Kadena.CMSModules.Kadena.Pages.Orders
{
    public partial class FailedOrders : CMSPage
    {
        protected const string ActionResubmit = "resubmitOrder";

        public IKenticoLogger Logger { get; set; } 
            = DIContainer.Resolve<IKenticoLogger>();
        public IOrderResubmissionService ResubmissionService { get; set; } 
            = DIContainer.Resolve<IOrderResubmissionService>();

        protected void Page_Load(object sender, EventArgs e)
        {
            HideMessage();

            WireupEvents();
        }

        private void WireupEvents()
        {
            grdOrders.OnDataReload += GrdOrders_OnDataReload;
            grdOrders.OnAction += GrdOrders_OnAction;
            grdOrders.OnExternalDataBound += GrdOrders_OnExternalDataBound;
        }

        private object GrdOrders_OnExternalDataBound(object sender, string sourceName, object parameter)
        {
            switch (sourceName)
            {
                case ActionResubmit:
                    var statusCellvalue = ((DataRowView)((GridViewRow)parameter).DataItem).Row["OrderStatus"];
                    var status = ValidationHelper.GetString(statusCellvalue, "");
                    if (status != OrderResubmissionService.OrderFailureStatus)
                    {
                        var button = (CMSGridActionButton)sender;
                        button.Enabled = false;
                    }
                    break;
            }

            return parameter;
        }

        private void GrdOrders_OnAction(string actionName, object actionArgument)
        {
            try
            {
                switch (actionName)
                {
                    case ActionResubmit:
                        ResubmitOrder((string)actionArgument);
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
            var result = ResubmissionService.ResubmitOrder(orderId).Result;
            if (result.Success)
            {
                ShowMessage($"Order #{orderId} resubmitted");
            }
            else
            {
                ShowMessage($"Failure. Error '{result.ErrorMessage}'");
            }
        }

        private DataSet GrdOrders_OnDataReload(
            string completeWhere, string currentOrder, int currentTopN,
            string columns, int currentOffset, int currentPageSize, ref int totalRecords)
        {
            var page = (currentOffset / currentPageSize) + 1;
            var orders = ResubmissionService.GetFailedOrders(page, currentPageSize).Result;

            totalRecords = orders.Pagination.RowsCount;
            var dataset = orders.Data.ToDataSet();
            return dataset;
        }
    }
}