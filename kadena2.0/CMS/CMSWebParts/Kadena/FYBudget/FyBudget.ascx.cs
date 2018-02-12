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
using Kadena2.Container.Default;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System.Collections.Generic;
using Kadena.Models.FyBudget;

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
            var userBudgetData = DIContainer.Resolve<IkenticoUserBudgetProvider>().GetUserBudgetAllocationRecords(CurrentUser.UserID, CurrentSite.SiteID);
            var fiscalYearData = DIContainer.Resolve<IkenticoUserBudgetProvider>().GetFiscalYearRecords();
            List<FyBudget> userBudgetDetails = new List<FyBudget>();
            foreach (var fisYear in fiscalYearData)
            {
                var userData = userBudgetData.Where(x => x.GetValue("Year", string.Empty) == fisYear.GetValue("Year", string.Empty)).FirstOrDefault();
                if (DIContainer.Resolve<IkenticoUserBudgetProvider>().CheckIfYearExists(fisYear.GetValue("Year", string.Empty), CurrentUser.UserID))
                {
                    userBudgetDetails.Add(new FyBudget()
                    {
                        ItemID = userData.ItemID,
                        Budget = userData.GetValue("Budget", default(decimal)),
                        IsYearEnded = fisYear.GetValue("FiscalYearEndDate", default(DateTime)) < DateTime.Now,
                        UserID = userData.GetValue("UserID", default(int)),
                        UserRemainingBudget = userData.GetValue("UserRemainingBudget", default(decimal)),
                        Year = fisYear.GetValue("Year", string.Empty)
                    });
                }
                else
                {
                    var newUserRecord = DIContainer.Resolve<IkenticoUserBudgetProvider>().CreateUserBudgetWithYear(fisYear.GetValue("Year", string.Empty), CurrentSite.SiteID, CurrentUser.UserID);
                    if (newUserRecord != null)
                    {
                        userBudgetDetails.Add(new FyBudget()
                        {
                            ItemID = newUserRecord.ItemID,
                            Budget = newUserRecord.Budget,
                            IsYearEnded = fisYear.GetValue("FiscalYearEndDate", default(DateTime)) < DateTime.Now,
                            UserID = newUserRecord.UserID,
                            UserRemainingBudget = newUserRecord.UserRemainingBudget,
                            Year = fisYear.GetValue("Year", string.Empty)
                        });
                    }
                }
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

    #endregion
}



