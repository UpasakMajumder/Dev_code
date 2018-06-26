using CMS.Base;
using CMS.Base.Web.UI;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using CMS.Search;
using CMS.WebAnalytics;
using System;
using System.Web;
using System.Web.UI;

public partial class CMSWebParts_Kadena_Search_KDASqlSearchBox : CMSAbstractWebPart
{
    // Result page url
    protected string mResultPageUrl = RequestContext.CurrentURL;

    private PagesActivityLogger mPagesActivityLogger = new PagesActivityLogger();

    #region "Public properties"

    /// <summary>
    /// Gets or sets the value that indicates whether image button is displayed instead of regular button.
    /// </summary>
    public bool ShowImageButton
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ShowImageButton"), false);
        }
        set
        {
            SetValue("ShowImageButton", value);
        }
    }

    /// <summary>
    /// Gets or sets an Image button URL.
    /// </summary>
    public string ImageUrl
    {
        get
        {
            return ResolveUrl(ValidationHelper.GetString(GetValue("ImageUrl"), ""));
        }
        set
        {
            SetValue("ImageUrl", value);
        }
    }

    /// <summary>
    /// Gets or sets the value that indicates whether search label is displayed.
    /// </summary>
    public bool ShowSearchLabel
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ShowSearchLabel"), lblSearch.Visible);
        }
        set
        {
            SetValue("ShowSearchLabel", value);
            lblSearch.Visible = value;
        }
    }

    /// <summary>
    /// Gets or sets the search label text.
    /// </summary>
    public string SearchLabelText
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("SearchLabelText"), ResHelper.LocalizeString("{$WebPart.SearchBox.Label$}"));
        }
        set
        {
            SetValue("SearchLabelText", value);
            lblSearch.Text = value;
        }
    }

    /// <summary>
    /// Gets or sets the search button text.
    /// </summary>
    public string SearchButtonText
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("SearchButtonText"), ResHelper.LocalizeString("{$WebPart.SearchBox.Button$}"));
        }
        set
        {
            SetValue("SearchButtonText", value);
        }
    }

    /// <summary>
    /// Gets or sets the search label Css class.
    /// </summary>
    public string SearchLabelCssClass
    {
        get
        {
            return ValidationHelper.GetString(GetValue("SearchLabelCssClass"), "");
        }
        set
        {
            SetValue("SearchLabelCssClass", value);
            lblSearch.CssClass = value;
        }
    }

    /// <summary>
    /// Gets or sets search text box CSS class.
    /// </summary>
    public string SearchTextboxCssClass
    {
        get
        {
            return ValidationHelper.GetString(GetValue("SearchTextboxCssClass"), "");
        }
        set
        {
            SetValue("SearchTextboxCssClass", value);
            txtWord.CssClass = value;
        }
    }

    /// <summary>
    /// Gets or sets the search button CSS class.
    /// </summary>
    public string SearchButtonCssClass
    {
        get
        {
            return ValidationHelper.GetString(GetValue("SearchButtonCssClass"), "");
        }
        set
        {
            SetValue("SearchButtonCssClass", value);
        }
    }

    /// <summary>
    /// Gets or sets the search results page URL.
    /// </summary>
    public string SearchResultsPageUrl
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("SearchResultsPageUrl"), mResultPageUrl);
        }
        set
        {
            SetValue("SearchResultsPageUrl", value);
            mResultPageUrl = value;
        }
    }

    /// <summary>
    ///  Gets or sets the Search mode.
    /// </summary>
    public SearchModeEnum SearchMode
    {
        get
        {
            return ValidationHelper.GetString(GetValue("SearchMode"), "").ToEnum<SearchModeEnum>();
        }
        set
        {
            SetValue("SearchMode", value.ToStringRepresentation());
        }
    }

    /// <summary>
    /// Gets or sets the Skin ID.
    /// </summary>
    public override string SkinID
    {
        get
        {
            return base.SkinID;
        }
        set
        {
            base.SkinID = value;

            // Set SkinID properties
            if (PageCycle < PageCycleEnum.Initialized)
            {
                lblSearch.SkinID = value;
                txtWord.SkinID = value;
            }
        }
    }

    #endregion "Public properties"

    /// <summary>
    /// Page load.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        SetupControl();
    }

    /// <summary>
    /// Initializes the control properties.
    /// </summary>
    protected void SetupControl()
    {
        if (!StopProcessing)
        {
            txtWord.Attributes.Add("placeholder", CMS.Helpers.ResHelper.GetString("KDA.Common.SearchWaterMarkText"));

            // Set label visibility according to WAI standards
            if (!ShowSearchLabel)
            {
                lblSearch.Style.Add("display", "none");
            }

            // Set text properties
            lblSearch.Text = SearchLabelText;

            // Set class properties
            lblSearch.CssClass = SearchLabelCssClass;
            txtWord.CssClass = SearchTextboxCssClass;

            // Set result page
            mResultPageUrl = SearchResultsPageUrl;

            // Set SkinID properties
            if (!StandAlone && (PageCycle < PageCycleEnum.Initialized) && (ValidationHelper.GetString(Page.StyleSheetTheme, string.Empty) == string.Empty))
            {
                string skinId = SkinID;
                lblSearch.SkinID = skinId;
                txtWord.SkinID = skinId;
            }
        }

        if (IsPostBack)
        {
            Search();
        }
    }

    /// <summary>
    /// Applies given stylesheet skin.
    /// </summary>
    public override void ApplyStyleSheetSkin(Page page)
    {
        string skinId = SkinID;
        lblSearch.SkinID = skinId;
        txtWord.SkinID = skinId;

        base.ApplyStyleSheetSkin(page);
    }

    /// <summary>
    /// Runs the search.
    /// </summary>
    private void Search()
    {
        string url = SearchResultsPageUrl;

        if (url.StartsWithCSafe("~"))
        {
            url = ResolveUrl(url.Trim());
        }

        if (url.Contains("?"))
        {
            url = URLHelper.RemoveParameterFromUrl(url, "searchtext");
            url = URLHelper.RemoveParameterFromUrl(url, "searchMode");
        }

        url = URLHelper.AddParameterToUrl(url, "searchtext", HttpUtility.UrlEncode(txtWord.Text));
        url = URLHelper.AddParameterToUrl(url, "searchMode", SearchMode.ToString());

        // Log "internal search" activity
        mPagesActivityLogger.LogInternalSearch(txtWord.Text, DocumentContext.CurrentDocument);

        URLHelper.Redirect(UrlResolver.ResolveUrl(url.Trim()));
    }
}