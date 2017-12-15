using CMS.EventLog;
using CMS.PortalEngine.Web.UI;
using System;
using CMS.Membership;
using System.Net.Http;
using System.Net.Http.Headers;
using CMS.Helpers;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Kadena.Dto.General;
using Kadena.DTO.Dashboard;

public partial class CMSWebParts_Kadena_Global_Admin_Dashboard_GlobalAdminDashboard : CMSAbstractWebPart
{
    public string UsersCount
    {
        get
        {
            return UsersCount;
        }
        set
        {
            SetValue("POSNumberText", value);
        }
    }
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
        if (this.StopProcessing)
        {
        }
        else if (!RequestHelper.IsPostBack())
        {
            GetUserDetails();
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
    public async Task<bool> GetUserDetails()
    {
        try
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(Request.Url.GetLeftPart(UriPartial.Authority));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage dashBoardStatistics = client.GetAsync("api/globaladmindashboard/getdashboardstatistics").Result;

            if (dashBoardStatistics.IsSuccessStatusCode)
            {
                var orderDashboardJsonString = await dashBoardStatistics.Content.ReadAsStringAsync();
                JavaScriptSerializer oJS = new JavaScriptSerializer();
                var obj = oJS.Deserialize<BaseResponseDto<DashboardStatisticsDTO>>(orderDashboardJsonString);
                var salesPerson = obj.Payload.NewSalespersons;
                var openOrders = obj.Payload.OpenOrders;
                var ordersPlaced = obj.Payload.OrdersPlaced;
                lblCurrentWeekUserCount.InnerText = salesPerson?.Week?.Count.ToString()?? "0";
                lblCurrentMonthUserCount.InnerText = salesPerson?.Month.Count.ToString()?? "0";
                lblCurrentYearUserCount.InnerText = salesPerson?.Year?.Count.ToString()?? "0";
                lblCurrentWeekOpenOrder.InnerText = openOrders?.Week.Count.ToString()??"0";
                lblCurrentWeekOpenMoney.InnerText = "$" + openOrders?.Week.Cost??"0";
                lblCurrentMonthOpenOrder.InnerText = openOrders?.Month.Count.ToString() ?? "0";
                lblCurrentMonthOpenMoney.InnerText = "$" + openOrders?.Month.Cost ?? "0";
                lblCurrentYearOpenOrdersCount.InnerText = openOrders?.Year?.Count.ToString() ?? "0";
                lblCurrentYearOpenOrdersMoney.InnerText = "$" + openOrders?.Year?.Cost ?? "0";
                lblCurrentWeekOrdersPlacedCount.InnerText = ordersPlaced?.Week.Count.ToString() ?? "0";
                lblCurrentWeekOrdersPlacedMoney.InnerText = "$" + ordersPlaced?.Week.Cost ?? "0";
                lblCurrentMonthOrdersPlacedCount.InnerText = ordersPlaced?.Month.Count.ToString() ?? "0";
                lblCurrentMonthOrdersPlacedMoney.InnerText = "$" + ordersPlaced?.Month.Cost ?? "0";
                lblcurrentYearordersPlacedMoneyCount.InnerText = ordersPlaced?.Year?.Count.ToString() ?? "0";
                lblcurrentYearordersPlacedMoney.InnerText = "$" + ordersPlaced?.Year?.Cost??"0";
                return true;
            }
            else
            {
                lblCurrentWeekUserCount.InnerText = "0";
                lblCurrentMonthUserCount.InnerText = "0";
                lblCurrentYearUserCount.InnerText = "0";
                lblCurrentWeekOpenOrder.InnerText = "0";
                lblCurrentWeekOpenMoney.InnerText = "0";
                lblCurrentMonthOpenOrder.InnerText = "0";
                lblCurrentMonthOpenMoney.InnerText = "0";
                lblCurrentYearOpenOrdersCount.InnerText = "0";
                lblCurrentYearOpenOrdersMoney.InnerText = "0";
                lblCurrentWeekOrdersPlacedCount.InnerText = "0";
                lblCurrentWeekOrdersPlacedMoney.InnerText = "0";
                lblCurrentMonthOrdersPlacedCount.InnerText = "0";
                lblCurrentMonthOrdersPlacedMoney.InnerText = "0";
                lblcurrentYearordersPlacedMoneyCount.InnerText = "0";
                lblcurrentYearordersPlacedMoney.InnerText = "0";
                return true;
            }

        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("Getting users from database", ex.Message, ex);
            return false;
        }
    }

    #endregion "Methods"
}