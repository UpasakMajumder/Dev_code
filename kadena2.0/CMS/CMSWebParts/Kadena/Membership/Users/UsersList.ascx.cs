using System;
using System.Web.UI.WebControls;
using CMS.DocumentEngine.Web.UI;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using CMS.Base;
using CMS.Membership;
using CMS.EventLog;
using CMS.CustomTables.Types.KDA;
using CMS.CustomTables;
using System.Linq;
using System.Collections.Generic;
using CMS.DataEngine;
using CMS.SiteProvider;

public partial class CMSWebParts_Kadena_Membership_Users_UsersList : CMSAbstractWebPart
{
    #region "Public properties"

    /// <summary>
    /// Gets or sets whether use filter control.
    /// </summary>
    public bool ShowFilterControl
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ShowFilterControl"), true);
        }
        set
        {
            SetValue("ShowFilterControl", value);
        }
    }


    /// <summary>
    /// Gets or sets WHERE condition.
    /// </summary>
    public string WhereCondition
    {
        get
        {
            return ValidationHelper.GetString(GetValue("WhereCondition"), repUsers.WhereCondition);
        }
        set
        {
            SetValue("WhereCondition", value);
            repUsers.WhereCondition = value;
        }
    }


    /// <summary>
    /// Gets or sets ORDER BY condition.
    /// </summary>
    public string OrderBy
    {
        get
        {
            return ValidationHelper.GetString(GetValue("OrderBy"), repUsers.OrderBy);
        }
        set
        {
            SetValue("OrderBy", value);
            repUsers.OrderBy = value;
        }
    }


    /// <summary>
    /// Gets or sets top N selected documents.
    /// </summary>
    public int SelectTopN
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("SelectTopN"), repUsers.TopN);
        }
        set
        {
            SetValue("SelectTopN", value);
            repUsers.TopN = value;
        }
    }


    /// <summary>
    /// Gets or sets selected columns.
    /// </summary>
    public string Columns
    {
        get
        {
            return ValidationHelper.GetString(GetValue("Columns"), repUsers.SelectedColumns);
        }
        set
        {
            SetValue("Columns", value);
            repUsers.SelectedColumns = value;
        }
    }

    /// <summary>
    /// Gest or sest the cache item name.
    /// </summary>
    public override string CacheItemName
    {
        get
        {
            return base.CacheItemName;
        }
        set
        {
            base.CacheItemName = value;
            repUsers.CacheItemName = value;
        }
    }


    /// <summary>
    /// Cache dependencies, each cache dependency on a new line.
    /// </summary>
    public override string CacheDependencies
    {
        get
        {
            return ValidationHelper.GetString(base.CacheDependencies, repUsers.CacheDependencies);
        }
        set
        {
            base.CacheDependencies = value;
            repUsers.CacheDependencies = value;
        }
    }


    /// <summary>
    /// Gets or sets the cache minutes.
    /// </summary>
    public override int CacheMinutes
    {
        get
        {
            return base.CacheMinutes;
        }
        set
        {
            base.CacheMinutes = value;
            repUsers.CacheMinutes = value;
        }
    }


    /// <summary>
    /// Gets or sets New User Button Text.
    /// </summary>
    public string NewUserButtonText
    {
        get
        {
            return ValidationHelper.GetString(GetValue("NewUserButtonText"), ResHelper.GetString("Kadena.Users.NewUser"));
        }
        set
        {
            SetValue("NewUserButtonText", value);
        }
    }

    /// <summary>
    /// Gets or sets New User Alternative Form
    /// </summary>
    public string NewUserAlternativeForm
    {
        get
        {
            return ValidationHelper.GetString(GetValue("NewUserAlternativeForm"), string.Empty);
        }
        set
        {
            SetValue("NewUserAlternativeForm", value);
        }
    }

    /// <summary>
    /// Gets or sets Edit User Alternative Form
    /// </summary>
    public string EditUserAlternativeForm
    {
        get
        {
            return ValidationHelper.GetString(GetValue("EditUserAlternativeForm"), string.Empty);
        }
        set
        {
            SetValue("EditUserAlternativeForm", value);
        }
    }

    /// <summary>
    /// Gets or sets New User's role
    /// </summary>
    public string NewUserRole
    {
        get
        {
            return ValidationHelper.GetString(GetValue("NewUserRole"), string.Empty);
        }
        set
        {
            SetValue("NewUserRole", value);
        }
    }


    #endregion


    #region "Basic repeater properties"

    /// <summary>
    /// Gets or sets AlternatingItemTemplate property.
    /// </summary>
    public string AlternatingItemTransformationName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("AlternatingItemTransformationName"), "");
        }
        set
        {
            SetValue("AlternatingItemTransformationName", value);
        }
    }


    /// <summary>
    /// Gets or sets FooterTemplate property.
    /// </summary>
    public string FooterTransformationName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("FooterTransformationName"), "");
        }
        set
        {
            SetValue("FooterTransformationName", value);
        }
    }


    /// <summary>
    /// Gets or sets HeaderTemplate property.
    /// </summary>
    public string HeaderTransformationName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("HeaderTransformationName"), "");
        }
        set
        {
            SetValue("HeaderTransformationName", value);
        }
    }


    /// <summary>
    /// Gets or sets ItemTemplate property.
    /// </summary>
    public string TransformationName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("TransformationName"), "");
        }
        set
        {
            SetValue("TransformationName", value);
        }
    }


    /// <summary>
    /// Gets or sets SeparatorTemplate property.
    /// </summary>
    public string SeparatorTransformationName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("SeparatorTransformationName"), "");
        }
        set
        {
            SetValue("SeparatorTransformationName", value);
        }
    }


    /// <summary>
    /// Gets or sets HideControlForZeroRows property.
    /// </summary>
    public bool HideControlForZeroRows
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("HideControlForZeroRows"), false);
        }
        set
        {
            SetValue("HideControlForZeroRows", value);
            repUsers.HideControlForZeroRows = value;
        }
    }

    public string SqlQueryName
    {
        get => ValidationHelper.GetString(GetValue(nameof(SqlQueryName)), string.Empty);
        set
        {
            SetValue(nameof(SqlQueryName), value);
            repUsers.QueryName = value;
        }
    }

    /// <summary>
    /// Gets or sets ZeroRowsText property.
    /// </summary>
    public string ZeroRowsText
    {
        get
        {
            return ValidationHelper.GetString(GetValue("ZeroRowsText"), "");
        }
        set
        {
            SetValue("ZeroRowsText", value);
            repUsers.ZeroRowsText = value;
        }
    }

    #endregion


    #region "UniPager properties"

    /// <summary>
    /// Gets or sets the value that indicates whether pager should be hidden for single page.
    /// </summary>
    public bool HidePagerForSinglePage
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("HidePagerForSinglePage"), pagerElem.HidePagerForSinglePage);
        }
        set
        {
            SetValue("HidePagerForSinglePage", value);
            pagerElem.HidePagerForSinglePage = value;
        }
    }


    /// <summary>
    /// Gets or sets the number of records to display on a page.
    /// </summary>
    public int PageSize
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("PageSize"), pagerElem.PageSize);
        }
        set
        {
            SetValue("PageSize", value);
            pagerElem.PageSize = value;
        }
    }


    /// <summary>
    /// Gets or sets the number of pages displayed for current page range.
    /// </summary>
    public int GroupSize
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("GroupSize"), pagerElem.GroupSize);
        }
        set
        {
            SetValue("GroupSize", value);
            pagerElem.GroupSize = value;
        }
    }


    /// <summary>
    /// Gets or sets the pager mode ('querystring' or 'postback').
    /// </summary>
    public string PagingMode
    {
        get
        {
            return ValidationHelper.GetString(GetValue("PagingMode"), "postback");
        }
        set
        {
            if (value != null)
            {
                SetValue("PagingMode", value);
                switch (value.ToLowerCSafe())
                {
                    case "postback":
                        pagerElem.PagerMode = UniPagerMode.PostBack;
                        break;
                    default:
                        pagerElem.PagerMode = UniPagerMode.Querystring;
                        break;
                }
            }
        }
    }


    /// <summary>
    /// Gets or sets the querysting parameter.
    /// </summary>
    public string QueryStringKey
    {
        get
        {
            return ValidationHelper.GetString(GetValue("QueryStringKey"), pagerElem.QueryStringKey);
        }
        set
        {
            SetValue("QueryStringKey", value);
            pagerElem.QueryStringKey = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether first and last item template are displayed dynamically based on current view.
    /// </summary>
    public bool DisplayFirstLastAutomatically
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("DisplayFirstLastAutomatically"), pagerElem.DisplayFirstLastAutomatically);
        }
        set
        {
            SetValue("DisplayFirstLastAutomatically", value);
            pagerElem.DisplayFirstLastAutomatically = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether first and last item template are displayed dynamically based on current view.
    /// </summary>
    public bool DisplayPreviousNextAutomatically
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("DisplayPreviousNextAutomatically"), pagerElem.DisplayPreviousNextAutomatically);
        }
        set
        {
            SetValue("DisplayPreviousNextAutomatically", value);
            pagerElem.DisplayPreviousNextAutomatically = value;
        }
    }

    #endregion


    #region "UniPager Template properties"

    /// <summary>
    /// Gets or sets the pages template.
    /// </summary>
    public string PagesTemplate
    {
        get
        {
            return ValidationHelper.GetString(GetValue("Pages"), "");
        }
        set
        {
            SetValue("Pages", value);
        }
    }


    /// <summary>
    /// Gets or sets the current page template.
    /// </summary>
    public string CurrentPageTemplate
    {
        get
        {
            return ValidationHelper.GetString(GetValue("CurrentPage"), "");
        }
        set
        {
            SetValue("CurrentPage", value);
        }
    }


    /// <summary>
    /// Gets or sets the separator template.
    /// </summary>
    public string SeparatorTemplate
    {
        get
        {
            return ValidationHelper.GetString(GetValue("PageSeparator"), "");
        }
        set
        {
            SetValue("PageSeparator", value);
        }
    }


    /// <summary>
    /// Gets or sets the first page template.
    /// </summary>
    public string FirstPageTemplate
    {
        get
        {
            return ValidationHelper.GetString(GetValue("FirstPage"), "");
        }
        set
        {
            SetValue("FirstPage", value);
        }
    }


    /// <summary>
    /// Gets or sets the last page template.
    /// </summary>
    public string LastPageTemplate
    {
        get
        {
            return ValidationHelper.GetString(GetValue("LastPage"), "");
        }
        set
        {
            SetValue("LastPage", value);
        }
    }


    /// <summary>
    /// Gets or sets the previous page template.
    /// </summary>
    public string PreviousPageTemplate
    {
        get
        {
            return ValidationHelper.GetString(GetValue("PreviousPage"), "");
        }
        set
        {
            SetValue("PreviousPage", value);
        }
    }


    /// <summary>
    /// Gets or sets the next page template.
    /// </summary>
    public string NextPageTemplate
    {
        get
        {
            return ValidationHelper.GetString(GetValue("NextPage"), "");
        }
        set
        {
            SetValue("NextPage", value);
        }
    }


    /// <summary>
    /// Gets or sets the previous group template.
    /// </summary>
    public string PreviousGroupTemplate
    {
        get
        {
            return ValidationHelper.GetString(GetValue("PreviousGroup"), "");
        }
        set
        {
            SetValue("PreviousGroup", value);
        }
    }


    /// <summary>
    /// Gets or sets the next group template.
    /// </summary>
    public string NextGroupTemplate
    {
        get
        {
            return ValidationHelper.GetString(GetValue("NextGroup"), "");
        }
        set
        {
            SetValue("NextGroup", value);
        }
    }


    /// <summary>
    /// Gets or sets the layout template.
    /// </summary>
    public string LayoutTemplate
    {
        get
        {
            return ValidationHelper.GetString(GetValue("PagerLayout"), "");
        }
        set
        {
            SetValue("PagerLayout", value);
        }
    }


    /// <summary>
    /// Gets or sets the direct page template.
    /// </summary>
    public string DirectPageTemplate
    {
        get
        {
            return ValidationHelper.GetString(GetValue("DirectPage"), "");
        }
        set
        {
            SetValue("DirectPage", value);
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether paging is enabled.
    /// </summary>
    public bool EnablePaging
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("EnablePaging"), true);
        }
        set
        {
            SetValue("EnablePaging", value);
        }
    }

    #endregion


    /// <summary>
    /// Content loaded event handler.
    /// </summary>
    public override void OnContentLoaded()
    {
        base.OnContentLoaded();
        SetupControl();
    }


    /// <summary>
    /// Reloads data.
    /// </summary>
    public override void ReloadData()
    {
        base.ReloadData();
        SetupControl();
    }


    /// <summary>
    /// OnFilterChanged - init data properties and rebind repeater.
    /// </summary>
    protected void filterUsers_OnFilterChanged()
    {
        repUsers.WhereCondition = SqlHelper.AddWhereCondition(WhereCondition, filterUsers.WhereCondition);
        repUsers.ReloadData(true);
    }


    /// <summary>
    /// Initializes the control properties.
    /// </summary>
    protected void SetupControl()
    {
        if (StopProcessing)
        {
            // Do nothing
        }
        else
        {
            int userID = QueryHelper.GetInteger("userid", 0);
            UserInfo user = null;

            if (userID > 0)
                user = UserInfoProvider.GetUserInfo(userID);

            if (userID != 0)
            {
                if (user == null)
                {
                    user = new UserInfo();
                    formElem.Mode = CMS.Base.Web.UI.FormModeEnum.Insert;
                    formElem.AlternativeFormFullName = NewUserAlternativeForm;
                    user.Enabled = true;
                }
                else
                    formElem.AlternativeFormFullName = EditUserAlternativeForm;
                hdnUserRole.Value = GetUserRole(user);
                formElem.Info = user;

                pnlUserForm.Visible = true;
                pnlListView.Visible = false;

                formElem.SubmitButton.Visible = false;
            }
            else
            {
                pnlUserForm.Visible = false;
                pnlListView.Visible = true;

                filterUsers.Visible = ShowFilterControl;
                filterUsers.OnFilterChanged += filterUsers_OnFilterChanged;
                filterUsers.NewUserButtonText = NewUserButtonText;

                // Basic control properties
                repUsers.QueryName = SqlQueryName;
                repUsers.HideControlForZeroRows = HideControlForZeroRows;
                repUsers.ZeroRowsText = ZeroRowsText;
                repUsers.WhereCondition = WhereCondition;
                repUsers.OrderBy = OrderBy;

                #region "Repeater template properties"

                // Apply transformations if they exist
                if (!String.IsNullOrEmpty(TransformationName))
                {
                    repUsers.ItemTemplate = TransformationHelper.LoadTransformation(this, TransformationName);
                }
                if (!String.IsNullOrEmpty(AlternatingItemTransformationName))
                {
                    repUsers.AlternatingItemTemplate = TransformationHelper.LoadTransformation(this, AlternatingItemTransformationName);
                }
                if (!String.IsNullOrEmpty(FooterTransformationName))
                {
                    repUsers.FooterTemplate = TransformationHelper.LoadTransformation(this, FooterTransformationName);
                }
                if (!String.IsNullOrEmpty(HeaderTransformationName))
                {
                    repUsers.HeaderTemplate = TransformationHelper.LoadTransformation(this, HeaderTransformationName);
                }
                if (!String.IsNullOrEmpty(SeparatorTransformationName))
                {
                    repUsers.SeparatorTemplate = TransformationHelper.LoadTransformation(this, SeparatorTransformationName);
                }

                #endregion

                // UniPager properties
                pagerElem.PageControl = repUsers.ID;
                pagerElem.PageSize = PageSize;
                pagerElem.GroupSize = GroupSize;
                pagerElem.QueryStringKey = QueryStringKey;
                pagerElem.DisplayFirstLastAutomatically = DisplayFirstLastAutomatically;
                pagerElem.DisplayPreviousNextAutomatically = DisplayPreviousNextAutomatically;
                pagerElem.HidePagerForSinglePage = HidePagerForSinglePage;
                pagerElem.Enabled = EnablePaging;                

                switch (PagingMode.ToLowerCSafe())
                {
                    case "querystring":
                        pagerElem.PagerMode = UniPagerMode.Querystring;
                        break;

                    default:
                        pagerElem.PagerMode = UniPagerMode.PostBack;
                        break;
                }


                #region "UniPager template properties"

                // UniPager template properties
                if (!String.IsNullOrEmpty(PagesTemplate))
                {
                    pagerElem.PageNumbersTemplate = TransformationHelper.LoadTransformation(pagerElem, PagesTemplate);
                }

                if (!String.IsNullOrEmpty(CurrentPageTemplate))
                {
                    pagerElem.CurrentPageTemplate = TransformationHelper.LoadTransformation(pagerElem, CurrentPageTemplate);
                }

                if (!String.IsNullOrEmpty(SeparatorTemplate))
                {
                    pagerElem.PageNumbersSeparatorTemplate = TransformationHelper.LoadTransformation(pagerElem, SeparatorTemplate);
                }

                if (!String.IsNullOrEmpty(FirstPageTemplate))
                {
                    pagerElem.FirstPageTemplate = TransformationHelper.LoadTransformation(pagerElem, FirstPageTemplate);
                }

                if (!String.IsNullOrEmpty(LastPageTemplate))
                {
                    pagerElem.LastPageTemplate = TransformationHelper.LoadTransformation(pagerElem, LastPageTemplate);
                }

                if (!String.IsNullOrEmpty(PreviousPageTemplate))
                {
                    pagerElem.PreviousPageTemplate = TransformationHelper.LoadTransformation(pagerElem, PreviousPageTemplate);
                }

                if (!String.IsNullOrEmpty(NextPageTemplate))
                {
                    pagerElem.NextPageTemplate = TransformationHelper.LoadTransformation(pagerElem, NextPageTemplate);
                }

                if (!String.IsNullOrEmpty(PreviousGroupTemplate))
                {
                    pagerElem.PreviousGroupTemplate = TransformationHelper.LoadTransformation(pagerElem, PreviousGroupTemplate);
                }

                if (!String.IsNullOrEmpty(NextGroupTemplate))
                {
                    pagerElem.NextGroupTemplate = TransformationHelper.LoadTransformation(pagerElem, NextGroupTemplate);
                }

                if (!String.IsNullOrEmpty(DirectPageTemplate))
                {
                    pagerElem.DirectPageTemplate = TransformationHelper.LoadTransformation(pagerElem, DirectPageTemplate);
                }

                if (!String.IsNullOrEmpty(LayoutTemplate))
                {
                    pagerElem.LayoutTemplate = TransformationHelper.LoadTransformation(pagerElem, LayoutTemplate);
                }

                #endregion
            }
        }
    }

    private string GetUserRole(UserInfo user)
    {
        string userRole = string.Empty;
        if (user != null)
        {
            string adminRoles = SettingsKeyInfoProvider.GetValue("KDA_AdminRoles", SiteContext.CurrentSiteID);
            if (!string.IsNullOrWhiteSpace(adminRoles))
            {
                string[] roles = adminRoles.Split(';');
                foreach (string role in roles)
                {
                    if (user.IsInRole(role, SiteContext.CurrentSiteName))
                    {
                        userRole = role;
                        break;
                    }
                }
            }
        }
        return userRole;
    }


    /// <summary>
    /// OnPreRender override.
    /// </summary>
    protected override void OnPreRender(EventArgs e)
    {
        Visible = repUsers.HasData() || !HideControlForZeroRows;
        base.OnPreRender(e);
    }


    /// <summary>
    /// Clears cache.
    /// </summary>
    public override void ClearCache()
    {
        repUsers.ClearCache();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/" + CurrentDocument.DocumentUrlPath);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (formElem.ValidateData())
            {
                UserInfo user = formElem.Info as UserInfo;
                string BusinessUnits = ValidationHelper.GetString(formElem.GetFieldValue("BusinessUnit"), string.Empty);
                if (user == null || (user != null && user.UserID <= 0))
                {
                    CreateNewUser(user);
                    Response.Redirect("~/" + CurrentDocument.DocumentUrlPath);
                }
                else
                {
                    if (user != null && user.UserID != 0 && !string.IsNullOrEmpty(BusinessUnits))
                    {
                        BindBusinessUnitsToUser(BusinessUnits, user.UserID);
                        DeleteUserRoles(user.UserID);
                        NewUserRole = ValidationHelper.GetString(formElem.GetFieldValue("UserRole"), string.Empty);
                        if (!string.IsNullOrEmpty(NewUserRole))
                        {
                            UserInfoProvider.AddUserToRole(user.UserName, NewUserRole, CurrentSiteName);
                        }
                    }
                    formElem.SaveData("~/" + CurrentDocument.DocumentUrlPath);
                }
            }
            else
            {
                formElem.ShowValidationErrorMessage = true;
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("UsersList", "UserSave", ex);
        }
    }
    /// <summary>
    /// deletes all the roles assigned to particular user
    /// </summary>
    /// <param name="userID"></param>
    private void DeleteUserRoles(int userID)
    {
        try
        {
            var userRoleData = UserRoleInfoProvider.GetUserRoles()
                .WhereEquals("UserID", userID)
                .ToList();
            if (!DataHelper.DataSourceIsEmpty(userRoleData))
            {
                userRoleData.ForEach(x =>
                {
                    UserRoleInfoProvider.DeleteUserRoleInfo(x);
                });
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("UsersList", "DeleteUserRoles", ex);
        }
    }

    /// <summary>
    /// Creates new user
    /// </summary>
    /// <param name="user"></param>
    private void CreateNewUser(UserInfo user)
    {
        List<string> stringUserSettingKeys = new List<string>() {
            "UserMobile",
            "UserTitle",
            "UserState",
            "UserCity",
            "UserAddress",
            "UserFax",
            "FYBudget",
            "PasswordHint",
            "UserCompanyName",
            "UserZipCode"
        };
        List<string> intUserSettingKeys = new List<string>() {
            "UserCountry",
            "UserDivisionID",
            "SalesManagerID",
            "TradeMarketingManagerID"
        };
        user = new UserInfo()
        {
            FirstName = ValidationHelper.GetString(formElem.GetFieldValue("FirstName"), string.Empty),
            LastName = ValidationHelper.GetString(formElem.GetFieldValue("LastName"), string.Empty),
            Email = ValidationHelper.GetString(formElem.GetFieldValue("Email"), string.Empty),
            UserName = ValidationHelper.GetString(formElem.GetFieldValue("Email"), string.Empty),
            Enabled = ValidationHelper.GetBoolean(formElem.GetFieldValue("UserEnabled"), false),
            UserSettings = {
                UserPhone = ValidationHelper.GetString(formElem.GetFieldValue("UserMobile"), string.Empty)
            }
        };
        foreach (string stringUserSettingKey in stringUserSettingKeys)
        {
            user.UserSettings.SetValue(stringUserSettingKey, ValidationHelper.GetString(formElem.GetFieldValue(stringUserSettingKey), string.Empty));
        }
        foreach (string intUserSettingKey in intUserSettingKeys)
        {
            user.UserSettings.SetValue(intUserSettingKey, ValidationHelper.GetInteger(formElem.GetFieldValue(intUserSettingKey), 0));
        }
        string Password = ValidationHelper.GetString(formElem.GetFieldValue("UserPassword"), string.Empty);
        UserInfoProvider.SetUserInfo(user);
        UserInfoProvider.SetPassword(user.UserName, Password);
        UserInfoProvider.AddUserToSite(user.UserName, CurrentSiteName);
        NewUserRole = ValidationHelper.GetString(formElem.GetFieldValue("UserRole"), string.Empty);
        if (!string.IsNullOrEmpty(NewUserRole))
        {
            UserInfoProvider.AddUserToRole(user.UserName, NewUserRole, CurrentSiteName);
        }
        if (user != null)
        {
            string BusinessUnits = ValidationHelper.GetString(formElem.GetFieldValue("BusinessUnit"), string.Empty);
            BindBusinessUnitsToUser(BusinessUnits, user.UserID);
        }
    }

    /// <summary>
    /// Inserts user related business units 
    /// </summary>
    /// <param name="BusinessUnits">all busieness units</param>
    /// <param name="UserID">user id</param>
    private void BindBusinessUnitsToUser(string BusinessUnits, int UserID)
    {
        try
        {
            DeleteUserBusinessUnits(UserID);
            var delimitBuinessUnits = BusinessUnits.Split(';');
            foreach (var businessUnitID in delimitBuinessUnits)
            {
                if (!string.IsNullOrEmpty(businessUnitID) && IsBusinessUnitExisted(ValidationHelper.GetInteger(businessUnitID, 0), UserID))
                {
                    UserBusinessUnitsItem newBu = new UserBusinessUnitsItem()
                    {
                        UserID = UserID,
                        BusinessUnitID = ValidationHelper.GetInteger(businessUnitID, 0)
                    };
                    newBu.Insert();
                }
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("UsersList.ascx.cs", "BindBusinessUnitsToUser()", ex);
        }
    }

    /// <summary>
    /// Checks whether the specifc business unit is assigned to the user
    /// </summary>
    /// <param name="BusinessUnitID">business unit item id</param>
    /// <param name="UserID">user id</param>
    /// <returns>true/false</returns>
    private bool IsBusinessUnitExisted(int BusinessUnitID, int UserID)
    {
        try
        {
            var buData = CustomTableItemProvider.GetItems<UserBusinessUnitsItem>()
                        .WhereEquals("UserID", UserID)
                        .And()
                        .WhereEquals("BusinessUnitID", BusinessUnitID)
                        .Columns("BusinessUnitID")
                        .FirstOrDefault();
            if (DataHelper.DataSourceIsEmpty(buData)) return true;
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("UsersList.ascx.cs", "IsBusinessUnitExisted()", ex);
        }
        return false;
    }

    /// <summary>
    /// Deletes all the user related busienss units
    /// </summary>
    /// <param name="UserID"></param>
    /// <returns></returns>
    private bool DeleteUserBusinessUnits(int UserID)
    {
        try
        {
            var buData = CustomTableItemProvider.GetItems<UserBusinessUnitsItem>()
                        .WhereEquals("UserID", UserID)
                        .Columns("ItemID")
                        .ToList();
            if (!DataHelper.DataSourceIsEmpty(buData))
            {
                buData.ForEach(b =>
                {
                    b.Delete();
                });
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("UsersList.ascx.cs", "DeleteUserBusinessUnits()", ex);
        }
        return false;
    }
}