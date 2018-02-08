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
    /// Reloads the control data.
    /// </summary>
    public override void ReloadData()
    {
        base.ReloadData();

        SetupControl();
    }

    /// <summary>
    /// Binding Fiscal year Budget data
    /// </summary>
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



