using CMS.FormEngine.Web.UI;
using CMS.SiteProvider;
using Kadena.BusinessLogic.Contracts.Approval;
using Kadena.Container.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kadena.CMSFormControls.Kadena
{
    public partial class OrderApproverSelector : FormEngineUserControl
    {
        private IApproverService _approverService;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControl();
                LoadControl();
            }
        }

        private void InitControl()
        {
            _approverService = DIContainer.Resolve<IApproverService>();
        }

        private void LoadControl()
        {
            var users = _approverService.GetApprovers(SiteContext.CurrentSiteID);

            ApproverSelectList.DataSource = users.Select(x => new { UserId = x.UserId, UserDescription = $"{x.UserName} ({x.Email})" });
            ApproverSelectList.DataValueField = "UserId";
            ApproverSelectList.DataTextField = "UserDescription";
            ApproverSelectList.DataBind();

        }

        public override object Value
        {
            get
            {
                return ApproverSelectList.SelectedValue;
            }
            set
            {
                if (value == null) return;
                ApproverSelectList.SelectedValue = value.ToString();
            }
        }

    }
}