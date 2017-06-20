using CMS.Ecommerce;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using CMS.SiteProvider;
using Kadena.Old_App_Code.Helpers;
using Kadena.Old_App_Code.Kadena.Orders;
using System.Collections.Generic;
using System.Linq;

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

        public string OrderDetailUrl
        {
            get
            {
                return ValidationHelper.GetString(GetValue("OrderDetailUrl"), string.Empty);
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
                IEnumerable<OrderHistoryData> orderData = null;
                if (IsForCurrentUser)
                {
                    if (ECommerceContext.CurrentCustomer != null)
                    {
                        orderData = ServiceHelper.GetOrderHistoryData(ECommerceContext.CurrentCustomer.CustomerID, 1, NumberOfItemsOnPage);
                    }
                }
                else
                {
                    if (SiteContext.CurrentSite != null)
                    {
                    }
                }

                if ((orderData?.ToList().Count ?? 0) > 0)
                {
                    repOrderList.DataSource = orderData;
                    repOrderList.DataBind();
                }
                else
                {
                    repOrderList.Visible = false;
                    lblNoOrderItems.Visible = true;
                }
            }
        }

        #endregion

        #region Private methods


        #endregion
    }
}