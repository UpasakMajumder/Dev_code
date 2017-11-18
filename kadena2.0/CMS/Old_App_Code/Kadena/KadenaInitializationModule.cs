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
    public KadenaInitializationModule()
        : base("CustomInit")
    {
    }

    // Contains initialization code that is executed when the application starts
    protected override void OnInit()
    {
        base.OnInit();

        // Assigns custom handlers to events
        UserInfo.TYPEINFO.Events.Update.After += KDAUser_Update_After;
    }

    private void KDAUser_Update_After(object sender, ObjectEventArgs e)
    {
        bool UserFYBudgetEnabled = SettingsKeyInfoProvider.GetBoolValue("KDA_UserFYBudgetEnabled", SiteContext.CurrentSiteID);
        UserInfo User = e.Object as UserInfo;
        if (User != null && UserFYBudgetEnabled)
        {
            double Budget = User.GetDoubleValue("FYBudget", 0);
            if (Budget > 0)
            {
                UserFYBudgetAllocationItem UserBudget = CustomTableItemProvider.GetItems<UserFYBudgetAllocationItem>().Where(x => x.UserID.Equals(User.UserID) && x.Year.Equals(DateTime.Now.Year.ToString()) && x.SiteID.Equals(SiteContext.CurrentSiteID)).FirstOrDefault();

                if (UserBudget == null)
                    UserBudget = new UserFYBudgetAllocationItem();

                UserBudget.Year = DateTime.Now.Year.ToString();
                UserBudget.UserID = User.UserID;
                UserBudget.Budget = Budget;
                UserBudget.SiteID = SiteContext.CurrentSiteID;

                if (UserBudget.ItemID > 0)
                    UserBudget.Update();
                else
                    UserBudget.Insert();
            }
        }
    }

}