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
using Kadena.Container.Default;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System.Collections.Generic;
using Kadena.Models.FyBudget;
using Kadena.Models.UserBudget;

public partial class CMSWebParts_Kadena_FYBudget_FyBudget : CMSAbstractWebPart
{
    public IKenticoUserBudgetProvider UserBudgetProvider { get; protected set; } = DIContainer.Resolve<IKenticoUserBudgetProvider>();

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
            var userBudgetData = UserBudgetProvider.GetUserBudgetAllocationRecords(CurrentUser.UserID, CurrentSite.SiteID);
            var fiscalYearData = UserBudgetProvider.GetFiscalYearRecords();
            var userBudgetDetails = new List<FyBudget>();
            foreach (var fisYear in fiscalYearData)
            {
                var userData = userBudgetData.Where(x => x.Year == fisYear.Year).FirstOrDefault();
                var userBudget = UserBudgetProvider.GetOrCreateUserBudgetWithYear(fisYear.Year, CurrentSite.SiteID, CurrentUser.UserID);
                userBudgetDetails.Add(MapToFyBudget(userBudget, fisYear));
            }
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

    private FyBudget MapToFyBudget(UserBudgetItem item, FiscalYear fisYear) => new FyBudget
    {
        ItemID = item.ItemID,
        Budget = item.Budget,
        IsYearEnded = fisYear.EndDate < DateTime.Now,
        UserID = item.UserID,
        UserRemainingBudget = item.UserRemainingBudget,
        Year = item.Year
    };

    #endregion
}



