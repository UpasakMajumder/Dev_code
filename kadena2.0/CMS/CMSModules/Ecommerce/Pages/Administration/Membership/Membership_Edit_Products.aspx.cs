using System;
using System.Data;

using CMS.Base;
using CMS.Ecommerce;
using CMS.Helpers;
using CMS.Membership;
using CMS.SiteProvider;
using CMS.UIControls;


public partial class CMSModules_Ecommerce_Pages_Administration_Membership_Membership_Edit_Products : CMSMembershipPage
{
    #region "Variables"

    private int membershipID;
    
    #endregion


    #region "Page events"

    protected void Page_Load(object sender, EventArgs e)
    {
        // Get query parameters
        membershipID = QueryHelper.GetInteger("membershipid", 0);
        
        // Get membership
        MembershipInfo mi = MembershipInfoProvider.GetMembershipInfo(membershipID);

        EditedObject = mi;

        // Check permissions
        if (!MembershipContext.AuthenticatedUser.CheckPrivilegeLevel(UserPrivilegeLevelEnum.Admin))
        {
            if (mi != null)
            {
                if (mi.MembershipSiteID != SiteContext.CurrentSiteID)
                {
                    RedirectToAccessDenied(GetString("general.actiondenied"));
                }
            }
        }

        // Get products associated with this membership
        productsUniGridElem.WhereCondition = string.Format("(SKUMembershipGUID = '{0}') AND ((SKUSiteID = '{1}') OR SKUSiteID IS NULL)", mi.MembershipGUID, mi.MembershipSiteID);
    }

    #endregion


    #region "Methods"

    protected object productsUniGridElem_OnExternalDataBound(object sender, string sourceName, object parameter)
    {
        DataRowView row;

        switch (sourceName.ToLowerInvariant())
        {
            case "skuprice":
                row = (DataRowView)parameter;

                // Get information needed to format SKU price
                double value = ValidationHelper.GetDouble(row["SKUPrice"], 0);
                int siteId = ValidationHelper.GetInteger(row["SKUSiteID"], 0);

                // Return formatted SKU price
                return CurrencyInfoProvider.GetFormattedPrice(value, siteId);

            case "skuvalidity":
                row = (DataRowView)parameter;

                // Get information needed to format SKU validity
                ValidityEnum validity = DateTimeHelper.GetValidityEnum(ValidationHelper.GetString(row["SKUValidity"], null));
                int validFor = ValidationHelper.GetInteger(row["SKUValidFor"], 0);
                DateTime validUntil = ValidationHelper.GetDateTime(row["SKUValidUntil"], DateTimeHelper.ZERO_TIME);

                // Return formatted SKU validity
                return DateTimeHelper.GetFormattedValidity(validity, validFor, validUntil);

            case "skuisproductoption":
                row = (DataRowView)parameter;

                var skuInfo = SKUInfoProvider.GetSKUInfo(ValidationHelper.GetInteger(row["SKUID"], 0));
                if (skuInfo != null)
                {
                    if (skuInfo.IsProductOption)
                    {
                        return ResHelper.GetString("general.yes");
                    }
                }

                return ResHelper.GetString("general.no");

            case "skuproductoptioncategoryname":
                row = (DataRowView)parameter;

                var resultCategoryName = "";
                var optionCategoryID = ValidationHelper.GetInteger(row["SKUOptionCategoryID"], 0);

                if (optionCategoryID > 0)
                {
                    var optionCategory = OptionCategoryInfoProvider.GetOptionCategoryInfo(optionCategoryID);
                    if (optionCategory != null)
                    {
                        resultCategoryName = optionCategory.CategoryDisplayName;
                    }
                }

                return resultCategoryName;
        }
        return null;
    }

    
    protected DataSet productsUniGridElem_OnDataReload(string completeWhere, string currentOrder, int currentTopN, string columns, int currentOffset, int currentPageSize, ref int totalRecords)
    {
        var query = SKUInfoProvider.GetSKUs()
                                   .Where(completeWhere)
                                   .OrderBy(currentOrder)
                                   .TopN(currentTopN);
        
        query.Offset = currentOffset;
        query.MaxRecords = currentPageSize;

        var results = query.Result;
        totalRecords = query.TotalRecords;

        return results;
    }

    #endregion
}