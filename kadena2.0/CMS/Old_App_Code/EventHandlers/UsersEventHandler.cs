using AutoMapper;
using CMS;
using CMS.CustomTables;
using CMS.CustomTables.Types.KDA;
using CMS.DataEngine;
using CMS.Membership;
using CMS.SiteProvider;
using Kadena.Container.Default;
using Kadena2.MicroserviceClients.Contracts;
using System;
using System.Linq;
using Kadena.Models.Membership;
using CMS.EventLog;
using Kadena.Dto.Membership;

[assembly: RegisterModule(typeof(Kadena.Old_App_Code.EventHandlers.UsersEventHandler))]

namespace Kadena.Old_App_Code.EventHandlers
{
    public class UsersEventHandler : Module
    {
        public UsersEventHandler() : base("UsersEventHandler") { }


        protected override void OnInit()
        {
            base.OnInit();
            UserInfo.TYPEINFO.Events.Insert.After += Insert_After;
            UserInfo.TYPEINFO.Events.Delete.After += Delete_After;
            UserInfo.TYPEINFO.Events.Update.After += KDAUser_Update_After;
        }

        private void Insert_After(object sender, ObjectEventArgs e)
        {
            if (e.Object is UserInfo user)
            {
                var mapper = DIContainer.Resolve<IMapper>();
                var managerClient = DIContainer.Resolve<IUserManagerClient>();
                var createUser = mapper.Map<User>(user);
                if (string.IsNullOrWhiteSpace(createUser.FullName))
                {
                    var names = user.FullName.Split(' ');
                    if (names.Length > 0)
                    {
                        createUser.FirstName = names[0];
                        if (names.Length > 1)
                        {
                            createUser.LastName = names[1];
                        }
                    }
                }
                var createUserRequest = mapper.Map<CreateUserDto>(createUser);
                var createResult = managerClient.Create(createUserRequest).Result;
                if (!createResult.Success)
                {
                    EventLogProvider.LogEvent(EventType.ERROR, this.GetType().Name, "CREATEOBJ", createResult.ErrorMessages);
                }
            }
        }

        private void Delete_After(object sender, ObjectEventArgs e)
        {
            if (e.Object.TypeInfo.Equals(UserInfo.TYPEINFO))
            {
                var userId = e.Object.GetIntegerValue("UserID", 0);
                if (userId != 0)
                {
                    var userHierarchies = UserHierarchyInfoProvider.GetUserHierarchies()
                        .WhereEquals("ParentUserId", userId)
                        .Or()
                        .WhereEquals("ChildUserId", userId);
                    foreach (var uh in userHierarchies)
                    {
                        uh.Delete();
                    }
                }
            }
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
                    {
                        userBudget = new UserFYBudgetAllocationItem();
                    }
                    userBudget.Year = DateTime.Now.Year.ToString();
                    userBudget.UserID = user.UserID;
                    userBudget.Budget = budget;
                    userBudget.SiteID = SiteContext.CurrentSiteID;
                    if (userBudget.ItemID > 0)
                    {
                        userBudget.Update();
                    }
                    else
                    {
                        userBudget.Insert();
                    }
                }
            }
        }
    }
}