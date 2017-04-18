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
        private const string _urlNewItem = "~/CMSModules/EmailTemplates/Pages/New.aspx";
        private const string _urlEditItem = "~/CMSModules/EmailTemplates/Pages/Frameset.aspx";
        private const string _templateType = "membershipchangepassword";

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

            

            SetUpTemplateSelector();
            
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
            SetUpTemplateSelector();
            pnlTemplate.Update();
        }

        private void SetUpTemplateSelector()
        {
            usBlankPasswords.WhereCondition = SqlHelper.AddWhereCondition(usBlankPasswords.WhereCondition,
                _siteId > 0 ? $"EmailTemplateSiteId = {_siteId}"
                : "EmailTemplateSiteId is null");

            // Filter type
            if (!string.IsNullOrWhiteSpace(_templateType))
            {
                usBlankPasswords.WhereCondition = SqlHelper.AddWhereCondition(usBlankPasswords.WhereCondition, $"EmailTemplateType = '{_templateType}'");
            }

            if (MembershipContext.AuthenticatedUser.IsAuthorizedPerResource("CMS.EmailTemplates", "Modify"))
            {
                string templateParameter = null;
                if (!string.IsNullOrWhiteSpace(_templateType))
                {
                    templateParameter = "&templatetype=" + URLHelper.URLEncode(_templateType);
                }

                string siteParameter = null;
                if (_siteId > 0)
                {
                    siteParameter = $"&siteid={_siteId}";
                }
                if (!string.IsNullOrWhiteSpace(_urlEditItem))
                {
                    string url = $"{_urlEditItem}?name=##ITEMID##&tabmode=1&editonlycode=1{siteParameter}{templateParameter}";
                    url = URLHelper.AddParameterToUrl(url, "hash", QueryHelper.GetHash($"?editonlycode=1{siteParameter}{templateParameter}"));
                    usBlankPasswords.EditItemPageUrl = url;
                }
                if (!string.IsNullOrWhiteSpace(_urlNewItem))
                {
                    string url = $"{_urlNewItem }?editonlycode=1{siteParameter}{templateParameter}";
                    url = URLHelper.AddParameterToUrl(url, "hash", QueryHelper.GetHash($"?editonlycode=1{templateParameter}"));
                    usBlankPasswords.NewItemPageUrl = url;
                }
            }
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
            // Check "modify" permission
            if (!MembershipContext.AuthenticatedUser.IsAuthorizedPerResource("CMS.Users", "Modify"))
            {
                RedirectToAccessDenied("CMS.Users", "Modify");
            }

            // Check template name
            var templateName = usBlankPasswords.Value.ToString();
            if (string.IsNullOrWhiteSpace(templateName))
            {
                ShowError(GetString("Kadena.Email.TemplateNotSelected"));
                return;
            }

            EmailTemplateInfo eti = EmailTemplateProvider.GetEmailTemplate(templateName, _siteId);
            // Validate first
            if (string.IsNullOrEmpty(eti.TemplateFrom))
            {
                ShowError(GetString("Kadena.Email.CorrectFromField"));
                return;
            }

            var users = UserInfoProvider.GetUsers().WhereEmpty("UserPassword");
            if (_siteId > 0)
                users = users.And().WhereIn("UserID",
                    UserSiteInfoProvider.GetUserSites()
                    .WhereEquals("SiteID", _siteId)
                    .Column("UserID"));

            foreach (var ui in users)
            {
                if (!string.IsNullOrWhiteSpace(ui.Email))
                {
                    EmailMessage msg = new EmailMessage();
                    msg.EmailFormat = EmailFormatEnum.Both;
                    msg.From = eti.TemplateFrom; //make sure this is specified in the template settings
                    msg.Recipients = ui.Email;
                    EmailSender.SendEmailWithTemplateText(siteSelector.SiteName, msg, eti, null, false); //if send immeditaley is true, e-mail queue is not used
                }
            }
        }
    }
}