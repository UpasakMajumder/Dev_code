using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CMS.PortalEngine.Web.UI;
using CMS.Helpers;
using CMS.CustomTables;
using CMS.CustomTables.Types.KDA;
using System.Linq;
using CMS.EventLog;

public partial class CMSWebParts_Kadena_FYBudget_FyBudget : CMSAbstractWebPart
{
    #region "Properties"

    /// <summary>
    /// Budget Year Resource string
    /// </summary>
    public string BudgetYearText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.FYBudget.BudgetYearText"), string.Empty);
        }
        set
        {
            SetValue("BudgetYearText", value);
        }
    }

    /// <summary>
    /// User Budget Resource string
    /// </summary>
    public string UserBudgetText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.FYBudget.UserBudgetText"), string.Empty);
        }
        set
        {
            SetValue("UserBudgetText", value);
        }
    }

    /// <summary>
    /// User remaining Budget Resource string
    /// </summary>
    public string UserRemainingBudgetText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.FYBudget.UserRemainingBudgetText"), string.Empty);
        }
        set
        {
            SetValue("UserRemainingBudgetText", value);
        }
    }
    #endregion


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
           BindFiscalYearData();
        }
    }

    /// <summary>
    /// Finding if year is ended.
    /// </summary>
    /// <returns></returns>
    public bool IsYearExpired()
    {
        try
        {
            var userBudgetData = CustomTableItemProvider.GetItems<UserFYBudgetAllocationItem>().WhereEquals("UserID", CurrentUser.UserID).WhereEquals("SiteID", CurrentSite.SiteID).ToList();
            return false;
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("checking if year is ended", "IsYearExpired()", ex, CurrentSite.SiteID, ex.Message);
            return false;
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
    public void BindFiscalYearData()
    {
        try
        {
            var userBudgetData = CustomTableItemProvider.GetItems<UserFYBudgetAllocationItem>().WhereEquals("UserID",CurrentUser.UserID).WhereEquals("SiteID",CurrentSite.SiteID).ToList();
            var fiscalYearData = CustomTableItemProvider.GetItems<FiscalYearManagementItem>().ToList();
            var userBudgetDetails = from userBudget in userBudgetData
                                    join fisYear in fiscalYearData
                                    on userBudget.Year equals fisYear.Year 
                                    where userBudget.UserID == CurrentUser.UserID && userBudget.SiteID == CurrentSite.SiteID
                                    select new
                                    {
                                        Year = fisYear.Year,//newData?.Year ?? string.Empty,
                                        ItemID = userBudget.ItemID,
                                        UserRemainingBudget = userBudget.UserRemainingBudget,
                                        UserID = userBudget.UserID,
                                        SiteID = userBudget.SiteID,
                                        Budget = userBudget.Budget,
                                        IsYearEnded = fisYear.FiscalYearEndDate < DateTime.Now
                                    };
            userBudgetDetails = userBudgetDetails.ToList();
            //var userBudgetDataBasedOnFY = userBudgetData.Join(fiscalYearData, x => x.Year, y => y.Year, (x, y) => new
            //{ x.Budget, x.UserID,x.ItemID, x.UserRemainingBudget, x.SiteID, y.Year })
            //.Where(x => x.UserID == CurrentUser.UserID && x.SiteID == CurrentSite.SiteID).ToList();
            if (!DataHelper.DataSourceIsEmpty(userBudgetDetails))
            {
                fiscalDatagrid.DataSource = userBudgetDetails;
                fiscalDatagrid.DataBind();
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("Get fiscal year data of current user", "BindFiscalYearData()", ex, CurrentSite.SiteID, ex.Message);
        }
    }

    #endregion
}



