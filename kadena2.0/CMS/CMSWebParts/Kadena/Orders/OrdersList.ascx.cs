using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kadena.CMSWebParts.Kadena.Orders
{
  public partial class OrdersList : CMSAbstractWebPart
  {
    #region Public properties

    public int NumberOfItemsOnPage
    {
      get
      {
        return ValidationHelper.GetInteger(GetValue("NumberOfItemsOnPage"), 5);
      }
    }

    public bool IsForCurrentUser
    {
      get
      {
        return ValidationHelper.GetBoolean(GetValue("IsForCurrentUser"), false);
      }
    }

    #endregion

    #region Public methods

    public override void OnContentLoaded()
    {
      base.OnContentLoaded();
      SetupControl();
    }

    protected void SetupControl()
    {
      if (!StopProcessing)
      {
      
      }
    }

    #endregion

    #region Private methods


    #endregion
  }
}