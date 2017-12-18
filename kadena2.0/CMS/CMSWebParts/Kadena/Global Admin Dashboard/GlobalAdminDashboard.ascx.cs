using CMS.EventLog;
using CMS.PortalEngine.Web.UI;
using System;
using Kadena.BusinessLogic.Services;
using Kadena.WebAPI.KenticoProviders;
using Kadena.Models.Dashboard;
using CMS.Ecommerce;
using CMS.Helpers;
using CMS.Membership;
using System.Linq;

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
            var client = new DashboardStatisticsService(new KenticoResourceService());
            DashboardStatistics serviceCallResult = client.GetDashboardStatistics();
            BindSalespersonData();
            if (serviceCallResult != null)
            {
                BindOpenOrdersData(serviceCallResult.OpenOrders);
                BindOrdersPlacedData(serviceCallResult.OrdersPlaced);
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("Getting Statistics", "GetStatistics", ex);
        }
    }

    private void BindSalespersonData()
    {
            lblCurrentWeekUserCount.InnerText = GetSalespersonStatistics(7);
            lblCurrentMonthUserCount.InnerText = GetSalespersonStatistics(30);
            lblCurrentYearUserCount.InnerText = GetSalespersonStatistics(365);
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
    /// <summary>
    /// Gets users count according to last N days
    /// </summary>
    /// <param name="lastNDays"></param>
    /// <returns></returns>
    private string GetSalespersonStatistics(int lastNDays)
    {
        try
        {
            var count =UserInfoProvider.GetUsers().Where(x =>(DateTime.Now - x.UserCreated).Days <= lastNDays).ToList().Count;
            if (count != default(int))
            {
                return count.ToString();
            }
            else {
                return "0";
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("Getting user Statistics", "GetSalespersonStatistics", ex);
            return "0";
        }
    }
    #endregion "Methods"
}