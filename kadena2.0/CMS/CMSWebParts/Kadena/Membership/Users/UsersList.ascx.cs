using System;

using System.Web.UI;
using System.Web.UI.WebControls;


using CMS.DocumentEngine.Web.UI;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using CMS.PortalEngine;
using CMS.Base;
using CMS.Base.Web.UI;
using CMS.Membership;


using System.Configuration;
using System.Text;
using System.Web;
using System.Web.Security;

using CMS.Activities.Loggers;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.EventLog;
using CMS.MembershipProvider;
using CMS.SiteProvider;
using CMS.CustomTables.Types.KDA;
using CMS.CustomTables;
using System.Linq;

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
            return ValidationHelper.GetString(GetValue("WhereCondition"), srcUsers.WhereCondition);
        }
        set
        {
            SetValue("WhereCondition", value);
            srcUsers.WhereCondition = value;
        }
    }


    /// <summary>
    /// Gets or sets ORDER BY condition.
    /// </summary>
    public string OrderBy
    {
        get
        {
            return ValidationHelper.GetString(GetValue("OrderBy"), srcUsers.OrderBy);
        }
        set
        {
            SetValue("OrderBy", value);
            srcUsers.OrderBy = value;
        }
    }


    /// <summary>
    /// Gets or sets top N selected documents.
    /// </summary>
    public int SelectTopN
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("SelectTopN"), srcUsers.TopN);
        }
        set
        {
            SetValue("SelectTopN", value);
            srcUsers.TopN = value;
        }
    }


    /// <summary>
    /// Gets or sets selected columns.
    /// </summary>
    public string Columns
    {
        get
        {
            return ValidationHelper.GetString(GetValue("Columns"), srcUsers.SelectedColumns);
        }
        set
        {
            SetValue("Columns", value);
            srcUsers.SelectedColumns = value;
        }
    }


    /// <summary>
    /// Gets or sets the source filter name.
    /// </summary>
    public string FilterName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("FilterName"), srcUsers.SourceFilterName);
        }
        set
        {
            SetValue("FilterName", value);
            srcUsers.SourceFilterName = value;
        }
    }


    /// <summary>
    /// Gets or sets site name.
    /// </summary>
    public string SiteName
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("SiteName"), srcUsers.SiteName);
        }
        set
        {
            SetValue("SiteName", value);
            srcUsers.SiteName = value;
        }
    }


    /// <summary>
    /// Gets or sets the select only approved users.
    /// </summary>
    public bool SelectOnlyApproved
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("SelectOnlyApproved"), srcUsers.SelectOnlyApproved);
        }
        set
        {
            SetValue("SelectOnlyApproved", value);
            srcUsers.SelectOnlyApproved = value;
        }
    }


    /// <summary>
    /// Gets or sets the select hidden users.
    /// </summary>
    public bool SelectHidden
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("SelectHidden"), srcUsers.SelectHidden);
        }
        set
        {
            SetValue("SelectHidden", value);
            srcUsers.SelectHidden = value;
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
            srcUsers.CacheItemName = value;
        }
    }


    /// <summary>
    /// Cache dependencies, each cache dependency on a new line.
    /// </summary>
    public override string CacheDependencies
    {
        get
        {
            return ValidationHelper.GetString(base.CacheDependencies, srcUsers.CacheDependencies);
        }
        set
        {
            base.CacheDependencies = value;
            srcUsers.CacheDependencies = value;
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
            srcUsers.CacheMinutes = value;
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
        filterUsers.InitDataProperties(srcUsers);


        // Connects repeater with data source
        repUsers.DataSource = srcUsers.DataSource;
        repUsers.DataBind();
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
            int UserID = QueryHelper.GetInteger("userid", 0);
            UserInfo User = null;

            if (UserID > 0)
                User = UserInfoProvider.GetUserInfo(UserID);

            if (UserID != 0)
            {
                if (User == null)
                {
                    User = new UserInfo();
                    formElem.Mode = CMS.Base.Web.UI.FormModeEnum.Insert;
                    formElem.AlternativeFormFullName = NewUserAlternativeForm;
                }
                else
                    formElem.AlternativeFormFullName = EditUserAlternativeForm;

                formElem.Info = User;

                pnlUserForm.Visible = true;
                pnlListView.Visible = false;

                formElem.SubmitButton.Visible = false;
            }
            else
            {
                pnlUserForm.Visible = false;
                pnlListView.Visible = true;

                repUsers.DataBindByDefault = false;
                pagerElem.PageControl = repUsers.ID;

                filterUsers.Visible = ShowFilterControl;
                filterUsers.OnFilterChanged += filterUsers_OnFilterChanged;
                srcUsers.OnFilterChanged += filterUsers_OnFilterChanged;

                // Basic control properties
                repUsers.HideControlForZeroRows = HideControlForZeroRows;
                repUsers.ZeroRowsText = ZeroRowsText;

                // Data source properties
                srcUsers.WhereCondition = WhereCondition;
                srcUsers.OrderBy = OrderBy;
                srcUsers.TopN = SelectTopN;
                srcUsers.SelectedColumns = Columns;
                srcUsers.SiteName = SiteName;
                srcUsers.FilterName = filterUsers.ID;
                srcUsers.SourceFilterName = FilterName;
                srcUsers.CacheItemName = CacheItemName;
                srcUsers.CacheDependencies = CacheDependencies;
                srcUsers.CacheMinutes = CacheMinutes;
                srcUsers.SelectOnlyApproved = SelectOnlyApproved;
                srcUsers.SelectHidden = SelectHidden;

                // Init data properties
                filterUsers.InitDataProperties(srcUsers);


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


                // Connects repeater with data source
                repUsers.DataSource = srcUsers.DataSource;
                repUsers.DataBind();

                filterUsers.NewUserButtonText = NewUserButtonText;
            }
        }
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
        srcUsers.ClearCache();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/" + CurrentDocument.DocumentUrlPath);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        UserInfo User = formElem.Info as UserInfo;

        if (User == null || (User != null && User.UserID <= 0))
        {
            User = new UserInfo()
            {
                FirstName = ValidationHelper.GetString(formElem.GetFieldValue("FirstName"), string.Empty),
                LastName = ValidationHelper.GetString(formElem.GetFieldValue("LastName"), string.Empty),
                Email = ValidationHelper.GetString(formElem.GetFieldValue("Email"), string.Empty),
                UserName = ValidationHelper.GetString(formElem.GetFieldValue("Email"), string.Empty),
                Enabled = true
            };

            if (!string.IsNullOrEmpty(User.Email))
            {

                User.UserSettings.UserPhone = ValidationHelper.GetString(formElem.GetFieldValue("UserMobile"), string.Empty);
                User.UserSettings.SetValue("UserMobile", ValidationHelper.GetString(formElem.GetFieldValue("UserMobile"), string.Empty));
                User.UserSettings.SetValue("UserTitle", ValidationHelper.GetString(formElem.GetFieldValue("UserTitle"), string.Empty));
                User.UserSettings.SetValue("UserCountry", ValidationHelper.GetInteger(formElem.GetFieldValue("UserCountry"), 0));
                User.UserSettings.SetValue("UserState", ValidationHelper.GetString(formElem.GetFieldValue("UserState"), string.Empty));
                User.UserSettings.SetValue("UserCity", ValidationHelper.GetString(formElem.GetFieldValue("UserCity"), string.Empty));
                User.UserSettings.SetValue("UserAddress", ValidationHelper.GetString(formElem.GetFieldValue("UserAddress"), string.Empty));
                User.UserSettings.SetValue("UserDivisionID", ValidationHelper.GetInteger(formElem.GetFieldValue("UserDivisionID"), 0));
                User.UserSettings.SetValue("SalesManagerID", ValidationHelper.GetInteger(formElem.GetFieldValue("SalesManagerID"), 0));
                User.UserSettings.SetValue("TradeMarketingManagerID", ValidationHelper.GetInteger(formElem.GetFieldValue("TradeMarketingManagerID"), 0));
                User.UserSettings.SetValue("UserFax", ValidationHelper.GetString(formElem.GetFieldValue("UserFax"), string.Empty));
                User.UserSettings.SetValue("FYBudget", ValidationHelper.GetString(formElem.GetFieldValue("FYBudget"), string.Empty));
                User.UserSettings.SetValue("PasswordHint", ValidationHelper.GetString(formElem.GetFieldValue("PasswordHint"), string.Empty));

                string Password = ValidationHelper.GetString(formElem.GetFieldValue("UserPassword"), string.Empty);
                UserInfoProvider.SetUserInfo(User);
                UserInfoProvider.SetPassword(User.UserName, Password);
                UserInfoProvider.AddUserToSite(User.UserName, CurrentSiteName);


                if (User != null && User.UserID > 0)
                {
                    //storing the busienss units data while creating user 

                    string BusinessUnits = ValidationHelper.GetString(formElem.GetFieldValue("BusinessUnit"), string.Empty);
                    if (User != null && User.UserID != 0 && !string.IsNullOrEmpty(BusinessUnits))
                        BindBusinessUnitsToUser(BusinessUnits, User.UserID);
                    Response.Redirect("~/" + CurrentDocument.DocumentUrlPath + "?userid=" + User.UserID);
                }
            }
            else
                lblNewUserError.Visible = true;
        }
        else
        {
            formElem.SaveData("", true);
            //storing the busienss units data while updating user details
            string BusinessUnits = ValidationHelper.GetString(formElem.GetFieldValue("BusinessUnit"), string.Empty);
            if (User != null && User.UserID != 0 && !string.IsNullOrEmpty(BusinessUnits))
                BindBusinessUnitsToUser(BusinessUnits, User.UserID);
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
            foreach (var s in delimitBuinessUnits)
            {
                if (s != string.Empty)
                {
                    if (IsBusinessUnitExisted(ValidationHelper.GetInteger(s, 0), UserID))
                    {
                        UserBusinessUnitsItem newBu = new UserBusinessUnitsItem();
                        newBu.UserID = UserID;
                        newBu.BusinessUnitID = ValidationHelper.GetInteger(s, 0);
                        newBu.Insert();
                    }
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
            var buData = CustomTableItemProvider.GetItems<UserBusinessUnitsItem>().WhereEquals("UserID", UserID).And().WhereEquals("BusinessUnitID", BusinessUnitID).Columns("BusinessUnitID").FirstOrDefault();
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
            var buData = CustomTableItemProvider.GetItems<UserBusinessUnitsItem>().WhereEquals("UserID", UserID).Columns("ItemID").ToList();
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