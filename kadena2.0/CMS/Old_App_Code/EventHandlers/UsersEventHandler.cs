using CMS;
using CMS.DataEngine;
using CMS.Membership;

[assembly: RegisterModule(typeof(Kadena.Old_App_Code.EventHandlers.UsersEventHandler))]

namespace Kadena.Old_App_Code.EventHandlers
{
  public class UsersEventHandler : Module
  {
    public UsersEventHandler() : base("UsersEventHandler") { }


    protected override void OnInit()
    {
      base.OnInit();
      UserInfo.TYPEINFO.Events.Delete.After += Delete_After;
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
  }
}