using CMS.Ecommerce;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using Kadena.Old_App_Code.Helpers;
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
                if (IsForCurrentUser)
                {
                    if (ECommerceContext.CurrentCustomer != null)
                    {
                        var orderData = ServiceHelper.GetOrderHistoryData(ECommerceContext.CurrentCustomer.CustomerID, 1, NumberOfItemsOnPage);

                        if ((orderData?.ToList().Count ?? 0) > 0)
                        {
                            repOrderList.DataSource = orderData.OrderByDescending(o => o.createDate);
                            repOrderList.DataBind();
                        }
                        else
                        {
                            repOrderList.Visible = false;
                            lblNoOrderItems.Visible = true;
                        }
                    }
                }
                else
                {

                }
            }
        }

        #endregion

        #region Private methods


        #endregion
    }
}