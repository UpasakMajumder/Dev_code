using System;
using CMS.DocumentEngine.Web.UI;
using CMS.DataEngine;
using CMS.Helpers;

public partial class CMSWebParts_Kadena_Membership_Users_UsersFilterControl : CMSAbstractBaseFilterControl
{
    #region "Public properties"

    /// <summary>
    /// Gets or sets the button text.
    /// </summary>
    public string ButtonText
    {
        get
        {
            if (string.IsNullOrEmpty(btnSelect.Text))
            {
                btnSelect.Text = ResHelper.GetString("general.search");
            }
            return btnSelect.Text;
        }
        set
        {
            btnSelect.Text = value;
        }
    }


    /// <summary>
    /// Gets or sets the activity link text.
    /// </summary>
    public string SortActivityLinkText
    {
        get
        {
            if (string.IsNullOrEmpty(lnkSortByActivity.Text))
            {
                lnkSortByActivity.Text = ResHelper.GetString("membersfilter.activity");
            }
            return lnkSortByActivity.Text;
        }
        set
        {
            lnkSortByActivity.Text = value;
        }
    }


    /// <summary>
    /// Gets or sets the user name link text.
    /// </summary>
    public string SortUserNameLinkText
    {
        get
        {
            if (string.IsNullOrEmpty(lnkSortByUserName.Text))
            {
                lnkSortByUserName.Text = ResHelper.GetString("general.username");
            }
            return lnkSortByUserName.Text;
        }
        set
        {
            lnkSortByUserName.Text = value;
        }
    }


    /// <summary>
    /// Gets or sets the New User button text.
    /// </summary>
    public string NewUserButtonText
    {
        get
        {
            if (string.IsNullOrEmpty(ltrNewUser.Text))
            {
                ltrNewUser.Text = ResHelper.GetString("Kadena.Users.NewUser");
            }
            return ltrNewUser.Text;
        }
        set
        {
            ltrNewUser.Text = value;
        }
    }

    #endregion

    private string CurrentQuery => txtValue.Text.Trim();
    private string PreviousQuery
    {
        get => ViewState["FilterCondition"]?.ToString() ?? string.Empty;
        set => ViewState["FilterCondition"] = value;
    }

    private bool HasQueryChanged() => CurrentQuery != PreviousQuery;

    /// <summary>
    /// OnLoad override - check wheter filter is set.
    /// </summary>
    protected override void OnLoad(EventArgs e)
    {
        if (HasQueryChanged())
        {
            Search();
        }

        if ((Request.Form[lnkSortByActivity.UniqueID] == null) && (Request.Form[lnkSortByUserName.UniqueID] == null))
        {
            string orderByPart = ValidationHelper.GetString(ViewState["OrderClause"], string.Empty);
            if (!string.IsNullOrEmpty(orderByPart))
            {
                // Set order by clause and raise OnFilter change event
                OrderBy = orderByPart;
                // Raise event
                RaiseOnFilterChanged();
            }
        }
        lblSortBy.Text = ResHelper.GetString("general.sortby") + ":";
        lnkSortByActivity.Text = SortActivityLinkText;
        lnkSortByUserName.Text = SortUserNameLinkText;
        btnSelect.Text = ButtonText;
        txtValue.Attributes.Add("Placeholder", ResHelper.GetString("Kadena.Users.SearchText"));
        base.OnLoad(e);
    }

    private void Search()
    {
        PreviousQuery = CurrentQuery;
        WhereCondition = GenerateWhereCondition(CurrentQuery);
        RaiseOnFilterChanged();
    }


    protected void lnkSortByUserName_Click(object sender, EventArgs e)
    {
        // Get order by clause from viewstate
        OrderBy = ValidationHelper.GetString(ViewState["OrderClause"], string.Empty);
        // Set new order by clause
        OrderBy = OrderBy.Contains("UserName ASC") ? "UserName DESC" : "UserName ASC";
        // Save new order by clause to viewstate
        ViewState["OrderClause"] = OrderBy;
        // Raise OnFilterChange event
        RaiseOnFilterChanged();
    }


    protected void lnkSortByActivity_Click(object sender, EventArgs e)
    {
        // Get order by clause from viewstate
        OrderBy = ValidationHelper.GetString(ViewState["OrderClause"], string.Empty);
        // Set new order by clause
        OrderBy = OrderBy.Contains("UserActivityPoints DESC") ? "UserActivityPoints ASC" : "UserActivityPoints DESC";
        // Save new order by clause to viewstate
        ViewState["OrderClause"] = OrderBy;
        // Raise OnFilterChange event
        RaiseOnFilterChanged();
    }


    /// <summary>
    /// Generates where condition.
    /// </summary>
    /// <param name="searchPhrase">Phrase to be searched</param>
    /// <returns>Where condition for given phrase.</returns>
    protected static string GenerateWhereCondition(string searchPhrase)
    {
        searchPhrase = SqlHelper.GetSafeQueryString(searchPhrase, false);
        string whereCondition = "(UserName LIKE N'%" + SqlHelper.EscapeLikeText(SqlHelper.EscapeQuotes(searchPhrase)) + "%') OR ";
        whereCondition += "(UserNickName LIKE N'%" + SqlHelper.EscapeLikeText(SqlHelper.EscapeQuotes(searchPhrase)) + "%') OR";
        whereCondition += "(FirstName LIKE N'%" + SqlHelper.EscapeLikeText(SqlHelper.EscapeQuotes(searchPhrase)) + "%') OR";
        whereCondition += "(LastName LIKE N'%" + SqlHelper.EscapeLikeText(SqlHelper.EscapeQuotes(searchPhrase)) + "%') ";
        return whereCondition;
    }
}