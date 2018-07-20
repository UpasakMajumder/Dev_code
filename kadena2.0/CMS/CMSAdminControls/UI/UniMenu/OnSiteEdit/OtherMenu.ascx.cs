using System;
using System.Web.UI.WebControls;

using CMS.Membership;
using CMS.PortalEngine;
using CMS.UIControls;

using MenuItem = CMS.UIControls.UniMenuConfig.Item;
using SubMenuItem = CMS.UIControls.UniMenuConfig.SubItem;

public partial class CMSAdminControls_UI_UniMenu_OnSiteEdit_OtherMenu : CMSUserControl
{
    #region "Methods"

    /// <summary>
    /// Returns the localized string of the control's hierarchically highest parent that has the <see cref="ResourcePrefix"/> property and its translation defined. 
    /// </summary>
    /// <remarks>
    /// <para>
    /// If no parent but the control has the property defined, returns the control's localized string.
    /// </para>
    /// <para>
    /// If none of them have the <see cref="ResourcePrefix"/> property defined, returns the default resource string translation.
    /// </para>
    /// </remarks>
    /// <param name="stringName">String to localize.</param>
    /// <param name="culture">Culture to be used for localization.</param>
    /// <seealso cref="ResourcePrefix"/>
    public override string GetString(string stringName, string culture = null)
    {
        string cultureCode = culture ?? (PortalContext.ViewMode.IsEditLive() ? MembershipContext.AuthenticatedUser.PreferredUICultureCode : null);

        return base.GetString(stringName, cultureCode);
    }


    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        var cui = CurrentUser;

        if (cui != null)
        {
            if (cui.IsAuthorizedPerUIElement("CMS.OnSiteEdit", "OnSiteHighlight"))
            {
                // Highlight button
                MenuItem highlightItem = new MenuItem();
                highlightItem.CssClass = "BigButton";
                highlightItem.ImageAlign = ImageAlign.Top;
                highlightItem.IconClass = "icon-square-dashed-line";
                highlightItem.OnClientClick = "OEHighlightToggle(event, this);";
                highlightItem.Text = GetString("onsiteedit.highlight");
                highlightItem.Tooltip = GetString("onsiteedit.highlighttooltip");
                highlightItem.ImageAltText = GetString("onsiteedit.highlight");

                otherMenu.Buttons.Add(highlightItem);
            }

            if (cui.IsAuthorizedPerUIElement("CMS.OnSiteEdit", "OnSiteHidden"))
            {
                // Hidden button
                MenuItem hiddenItem = new MenuItem();
                hiddenItem.CssClass = "BigButton OnSiteHiddenButton";
                hiddenItem.ImageAlign = ImageAlign.Top;
                hiddenItem.IconClass = "icon-eye-slash";
                hiddenItem.Text = GetString("general.hidden");
                hiddenItem.Tooltip = GetString("onsiteedit.hiddentooltip");
                hiddenItem.ImageAltText = GetString("general.hidden");

                // Add temporary empty sub menu item to ensure generating of the sub menu functions
                SubMenuItem epmtyItem = new SubMenuItem();
                hiddenItem.SubItems.Add(epmtyItem);

                otherMenu.Buttons.Add(hiddenItem);
            }
        }
    }

    #endregion
}
