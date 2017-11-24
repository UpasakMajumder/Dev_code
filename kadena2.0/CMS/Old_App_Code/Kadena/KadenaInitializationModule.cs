using CMS;
using CMS.CustomTables;
using CMS.CustomTables.Types.KDA;
using CMS.DataEngine;
using CMS.Membership;
using CMS.SiteProvider;
using System;
using System.Linq;

// Registers the custom module into the system
[assembly: RegisterModule(typeof(KadenaInitializationModule))]

public class KadenaInitializationModule : Module
{
    // Module class constructor, the system registers the module under the name "CustomInit"
    public KadenaInitializationModule() : base("CustomInit") { }

    // Contains initialization code that is executed when the application starts
    protected override void OnInit()
    {
        base.OnInit();
        UserInfo.TYPEINFO.Events.Update.After += KDAUser_Update_After;
    }

    /// <summary>
    /// Global event for User object to update FY Budget
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void KDAUser_Update_After(object sender, ObjectEventArgs e)
    {
        bool userFYBudgetEnabled = SettingsKeyInfoProvider.GetBoolValue("KDA_UserFYBudgetEnabled", SiteContext.CurrentSiteID);
        UserInfo user = e.Object as UserInfo;
        if (user != null && userFYBudgetEnabled)
        {
            double budget = user.GetDoubleValue("FYBudget", 0);
            if (budget > 0)
            {
                UserFYBudgetAllocationItem userBudget = CustomTableItemProvider.GetItems<UserFYBudgetAllocationItem>()
                                                        .Where(x => x.UserID.Equals(user.UserID) &&
                                                                    x.Year.Equals(DateTime.Now.Year.ToString()) &&
                                                                    x.SiteID.Equals(SiteContext.CurrentSiteID))
                                                        .FirstOrDefault();

                if (userBudget == null)
                    userBudget = new UserFYBudgetAllocationItem();

                userBudget.Year = DateTime.Now.Year.ToString();
                userBudget.UserID = user.UserID;
                userBudget.Budget = budget;
                userBudget.SiteID = SiteContext.CurrentSiteID;

                if (userBudget.ItemID > 0)
                    userBudget.Update();
                else
                    userBudget.Insert();
            }
        }
    }

}