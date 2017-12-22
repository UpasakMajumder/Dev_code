using CMS.EventLog;
using CMS.PortalEngine.Web.UI;
using System;
using Kadena.WebAPI.KenticoProviders;
using Kadena.Models.Dashboard;
using CMS.Ecommerce;
using CMS.Helpers;
using CMS.Membership;
using System.Linq;
using Kadena2.MicroserviceClients.Clients;
using Kadena.Helpers;
using Kadena.Dto.General;
using Kadena.Dto.Order;
using System.Collections.Generic;
using Kadena.Old_App_Code.Kadena.Constants;
using Kadena2.WebAPI.KenticoProviders;

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
            lblCurrentWeekOpenMoney.InnerText = string.Format(CurrentCurrencyFormat, ValidationHelper.GetDouble(openOrders.Week.Cost, 0));
            lblCurrentMonthOpenOrder.InnerText = openOrders.Month?.Count.ToString() ?? "0";
            lblCurrentMonthOpenMoney.InnerText = string.Format(CurrentCurrencyFormat, ValidationHelper.GetDouble(openOrders.Month.Cost, 0));
            lblCurrentYearOpenOrdersCount.InnerText = openOrders.Year?.Count.ToString() ?? "0";
            lblCurrentYearOpenOrdersMoney.InnerText = string.Format(CurrentCurrencyFormat, ValidationHelper.GetDouble(openOrders.Year.Cost, 0));
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
        var statisticClient = new OrderViewClient(ProviderFactory.MicroProperties);
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
    private StatisticBlock GetOrdersBlock(IEnumerable<OrderDto> ordersList, string orderStatus)
    {
        List<OrderDto> Weekly = FilterOrders(ordersList, orderStatus, 7);
        List<OrderDto> Monthly = FilterOrders(ordersList, orderStatus, 30);
        List<OrderDto> Yearly = FilterOrders(ordersList, orderStatus, 365);
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
    private List<OrderDto> FilterOrders(IEnumerable<OrderDto> ordersList, string orderStatus, int lastNDays)
    {
        return ordersList.Where(x => x.Status.Equals(orderStatus) && (DateTime.Now - x.CreateDate).Days <= lastNDays).ToList();
    }
    #endregion "Methods"
}