using CMS.FormEngine.Web.UI;
using CMS.Membership;
using CMS.UIControls;
using CMS.Base.Web.UI;
using CMS.Base.Web.UI.ActionsConfig;
using CMS.DataEngine;
using CMS.EmailEngine;
using CMS.Helpers;
using CMS.IO;
using CMS.MacroEngine;
using CMS.SiteProvider;
using System;
using System.Web.UI.WebControls;

namespace Kadena.CMSModules.Kadena.Pages.Users
{
    public partial class BlankPassword_Mailing : CMSPage
    {
        private int _siteId;
        private string _urlNewItem = "~/CMSModules/EmailTemplates/Pages/New.aspx";
        private string _urlEditItem = "~/CMSModules/EmailTemplates/Pages/Frameset.aspx";
        private string _templateType = "membershipchangepassword";

        protected void Page_Load(object sender, EventArgs e)
        {
            // Get current user
            var currentUser = MembershipContext.AuthenticatedUser;
            if (currentUser == null)
            {
                return;
            }

            siteSelector.UniSelector.OnSelectionChanged += Site_Changed;
            siteSelector.DropDownSingleSelect.AutoPostBack = true;

            if (RequestHelper.IsPostBack())
            {
                _siteId = ValidationHelper.GetInteger(siteSelector.Value, 0);
            }
            else
            {
                siteSelector.Value = UniSelector.US_ALL_RECORDS;
            }

            usBlankPasswords.WhereCondition = SqlHelper.AddWhereCondition(usBlankPasswords.WhereCondition,
                _siteId > 0 ? $"EmailTemplateSiteId = {_siteId}"
                : "EmailTemplateSiteId is null");
            
            // Filter type
            if (!string.IsNullOrWhiteSpace(_templateType))
            {
                usBlankPasswords.WhereCondition = SqlHelper.AddWhereCondition(usBlankPasswords.WhereCondition, $"EmailTemplateType = '{_templateType}'");
            }

            
            usBlankPasswords.ButtonClear.Visible = false;
            // Initialize header actions
            InitHeaderActions();
        }

        /// <summary>
        /// Handles change in site selection.
        /// </summary>
        protected void Site_Changed(object sender, EventArgs e)
        {
            usBlankPasswords.Value = null;
            usBlankPasswords.Reload(true);
        }

        /// <summary>
        /// Initializes header actions.
        /// </summary>
        private void InitHeaderActions()
        {
            HeaderAction sendAction = new HeaderAction
            {
                Text = GetString("general.send"),
                ButtonStyle = ButtonStyle.Primary,
                CommandName = "send",
                ResourceName = "CMS.Users",
                Permission = "Modify"
            };

            HeaderActions.AddAction(sendAction);
            HeaderActions.ActionPerformed += HeaderActions_ActionPerformed;
        }

        /// <summary>
        /// Handles header actions.
        /// </summary>
        private void HeaderActions_ActionPerformed(object sender, CommandEventArgs e)
        {
            switch (e.CommandName.ToLowerInvariant())
            {
                case "send":
                    Send();
                    break;
            }
        }

        /// <summary>
        /// Sends the email.
        /// </summary>
        protected void Send()
        {
        }
    }
}