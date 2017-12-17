using CMS.EventLog;
using CMS.PortalEngine.Web.UI;
using System;
using Kadena.BusinessLogic.Services;
using Kadena.WebAPI.KenticoProviders;
using Kadena.Models.Dashboard;
using CMS.Ecommerce;
using CMS.Helpers;

public partial class CMSWebParts_Kadena_Global_Admin_Dashboard_GlobalAdminDashboard : CMSAbstractWebPart
{
    private string CurrentCurrencyFormat = ECommerceContext.CurrentCurrency.CurrencyFormatString;

    #region "Methods"
    /// <summary>
    /// Content loaded event handler.
    /// </summary>
    public override void OnContentLoaded()
    {
        base.OnContentLoaded();
        SetupControl();
    }

    /// <summary>
    /// Initializes the control properties.
    /// </summary>
    protected void SetupControl()
    {
        if (!this.StopProcessing)
        {
            GetStatistics();
        }
    }

    /// <summary>
    /// Reloads the control data.
    /// </summary>
    public override void ReloadData()
    {
        base.ReloadData();

        SetupControl();
    }
    /// <summary>
    /// Get user details using services
    /// </summary>
    public void GetStatistics()
    {
        try
        {
            var client = new DashboardStatisticsService(new KenticoDashboardStatisticsProvider(), new KenticoResourceService());
            DashboardStatistics serviceCallResult = client.GetDashboardStatistics();
            if (serviceCallResult != null)
            {
                BindSalespersonData(serviceCallResult.NewSalespersons);
                BindOpenOrdersData(serviceCallResult.OpenOrders);
                BindOrdersPlacedData(serviceCallResult.OrdersPlaced);
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("Getting Statistics", "GetStatistics", ex);
        }
    }

    private void BindSalespersonData(StatisticBlock salesPerson)
    {
        if (salesPerson != null)
        {
            lblCurrentWeekUserCount.InnerText = salesPerson.Week?.Count.ToString() ?? "0";
            lblCurrentMonthUserCount.InnerText = salesPerson.Month?.Count.ToString() ?? "0";
            lblCurrentYearUserCount.InnerText = salesPerson.Year?.Count.ToString() ?? "0";
        }
    }

    private void BindOpenOrdersData(StatisticBlock openOrders)
    {
        if (openOrders != null)
        {
            lblCurrentWeekOpenOrder.InnerText = openOrders.Week?.Count.ToString() ?? "0";
            lblCurrentWeekOpenMoney.InnerText = string.Format(CurrentCurrencyFormat, ValidationHelper.GetDouble(openOrders.Week.Cost, 0));
            lblCurrentMonthOpenOrder.InnerText = openOrders.Month?.Count.ToString() ?? "0";
            lblCurrentMonthOpenMoney.InnerText = string.Format(CurrentCurrencyFormat, ValidationHelper.GetDouble(openOrders.Month.Cost, 0));
            lblCurrentYearOpenOrdersCount.InnerText = openOrders.Year?.Count.ToString() ?? "0";
            lblCurrentYearOpenOrdersMoney.InnerText = string.Format(CurrentCurrencyFormat, ValidationHelper.GetDouble(openOrders.Year.Cost, 0));
        }
    }

    private void BindOrdersPlacedData(StatisticBlock ordersPlaced)
    {
        if (ordersPlaced != null)
        {
            lblCurrentWeekOrdersPlacedCount.InnerText = ordersPlaced.Week?.Count.ToString() ?? "0";
            lblCurrentWeekOrdersPlacedMoney.InnerText = string.Format(CurrentCurrencyFormat, ValidationHelper.GetDouble(ordersPlaced.Week.Cost, 0));
            lblCurrentMonthOrdersPlacedCount.InnerText = ordersPlaced.Month?.Count.ToString() ?? "0";
            lblCurrentMonthOrdersPlacedMoney.InnerText = string.Format(CurrentCurrencyFormat, ValidationHelper.GetDouble(ordersPlaced.Month.Cost, 0));
            lblcurrentYearordersPlacedMoneyCount.InnerText = ordersPlaced.Year?.Count.ToString() ?? "0";
            lblcurrentYearordersPlacedMoney.InnerText = string.Format(CurrentCurrencyFormat, ValidationHelper.GetDouble(ordersPlaced.Year.Cost, 0));
        }
    }

    #endregion "Methods"
}