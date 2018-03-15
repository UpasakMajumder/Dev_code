using CMS.EventLog;
using CMS.PortalEngine.Web.UI;
using System;
using Kadena.Models.Dashboard;
using CMS.Ecommerce;
using CMS.Helpers;
using CMS.Membership;
using System.Linq;
using Kadena2.MicroserviceClients.Clients;
using Kadena.Dto.General;
using Kadena.Dto.Order;
using System.Collections.Generic;
using Kadena.Old_App_Code.Kadena.Constants;
using Kadena.Old_App_Code.Kadena;
using Kadena.Container.Default;
using Kadena2.MicroserviceClients.Contracts.Base;
using Kadena2.MicroserviceClients.Contracts;

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
            DashboardStatistics serviceCallResult = GetDashboardStatistics();
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
    /// <summary>
    /// Binding sales persons data to labels
    /// </summary>
    private void BindSalespersonData()
    {
        lblCurrentWeekUserCount.InnerText = GetSalespersonStatistics(7);
        lblCurrentMonthUserCount.InnerText = GetSalespersonStatistics(30);
        lblCurrentYearUserCount.InnerText = GetSalespersonStatistics(365);
    }
    /// <summary>
    /// Binding Open orders data to labels
    /// </summary>
    /// <param name="openOrders"></param>
    private void BindOpenOrdersData(StatisticBlock openOrders)
    {
        if (openOrders != null)
        {
            lblCurrentWeekOpenOrder.InnerText = openOrders.Week?.Count.ToString() ?? "0";
            lblCurrentWeekOpenMoney.InnerText = CurrencyInfoProvider.GetFormattedPrice(ValidationHelper.GetDouble(openOrders.Week.Cost, 0), CurrentSite.SiteID, true);
            lblCurrentMonthOpenOrder.InnerText = openOrders.Month?.Count.ToString() ?? "0";
            lblCurrentMonthOpenMoney.InnerText = CurrencyInfoProvider.GetFormattedPrice(ValidationHelper.GetDouble(openOrders.Month.Cost, 0), CurrentSite.SiteID, true);
            lblCurrentYearOpenOrdersCount.InnerText = openOrders.Year?.Count.ToString() ?? "0";
            lblCurrentYearOpenOrdersMoney.InnerText = CurrencyInfoProvider.GetFormattedPrice(ValidationHelper.GetDouble(openOrders.Year.Cost, 0), CurrentSite.SiteID, true);
        }
    }
    /// <summary>
    /// Binding Order placed data to labels
    /// </summary>
    /// <param name="ordersPlaced"></param>
    private void BindOrdersPlacedData(StatisticBlock ordersPlaced)
    {
        if (ordersPlaced != null)
        {
            lblCurrentWeekOrdersPlacedCount.InnerText = ordersPlaced.Week?.Count.ToString() ?? "0";
            lblCurrentWeekOrdersPlacedMoney.InnerText = CurrencyInfoProvider.GetFormattedPrice(ValidationHelper.GetDouble(ordersPlaced.Week.Cost, 0), CurrentSite.SiteID, true);
            lblCurrentMonthOrdersPlacedCount.InnerText = ordersPlaced.Month?.Count.ToString() ?? "0";
            lblCurrentMonthOrdersPlacedMoney.InnerText = CurrencyInfoProvider.GetFormattedPrice(ValidationHelper.GetDouble(ordersPlaced.Month.Cost, 0), CurrentSite.SiteID, true);
            lblcurrentYearordersPlacedMoneyCount.InnerText = ordersPlaced.Year?.Count.ToString() ?? "0";
            lblcurrentYearordersPlacedMoney.InnerText = CurrencyInfoProvider.GetFormattedPrice(ValidationHelper.GetDouble(ordersPlaced.Year.Cost, 0), CurrentSite.SiteID, true);
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
            var count = UserInfoProvider.GetUsers().Where(x => (DateTime.Now - x.UserCreated).Days <= lastNDays).ToList().Count;
            if (count != default(int))
            {
                return count.ToString();
            }
            else
            {
                return "0";
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("Getting user Statistics", "GetSalespersonStatistics", ex);
            return "0";
        }
    }
    /// <summary>
    /// getting orders from microservices
    /// </summary>
    /// <returns></returns>
    public DashboardStatistics GetDashboardStatistics()
    {
        DashboardStatistics statistics = new DashboardStatistics();
        var statisticClient = DIContainer.Resolve<IOrderViewClient>();
        BaseResponseDto<OrderListDto> response = statisticClient.GetOrders(CurrentSiteName, 1, 1000).Result;
        if (response.Success)
        {
            statistics.OpenOrders = GetOrdersBlock(response.Payload.Orders, OrderStatusConstants.OrderInProgress);
            statistics.OrdersPlaced = GetOrdersBlock(response.Payload.Orders, OrderStatusConstants.OrderPlaced);
        }
        return statistics;
    }
    /// <summary>
    /// Getting orders based on status
    /// </summary>
    /// <param name="ordersList"></param>
    /// <param name="orderStatus"></param>
    /// <returns></returns>
    private StatisticBlock GetOrdersBlock(IEnumerable<RecentOrderDto> ordersList, string orderStatus)
    {
        List<RecentOrderDto> Weekly = FilterOrders(ordersList, orderStatus, 7);
        List<RecentOrderDto> Monthly = FilterOrders(ordersList, orderStatus, 30);
        List<RecentOrderDto> Yearly = FilterOrders(ordersList, orderStatus, 365);
        return new StatisticBlock()
        {
            Week = new StatisticsReading()
            {
                Count = Weekly.Count,
                Cost = Weekly.Sum(x => x.TotalPrice).ToString()
            },
            Month = new StatisticsReading()
            {
                Count = Monthly.Count,
                Cost = Monthly.Sum(x => x.TotalPrice).ToString()
            },
            Year = new StatisticsReading()
            {
                Count = Yearly.Count,
                Cost = Yearly.Sum(x => x.TotalPrice).ToString()
            }
        };
    }
    /// <summary>
    /// Filtering orders based on status
    /// </summary>
    /// <param name="ordersList"></param>
    /// <param name="orderStatus"></param>
    /// <param name="lastNDays"></param>
    /// <returns></returns>
    private List<RecentOrderDto> FilterOrders(IEnumerable<RecentOrderDto> ordersList, string orderStatus, int lastNDays)
    {
        return ordersList.Where(x => x.Status.Equals(orderStatus) && (DateTime.Now - x.CreateDate).Days <= lastNDays).ToList();
    }
    #endregion "Methods"
}