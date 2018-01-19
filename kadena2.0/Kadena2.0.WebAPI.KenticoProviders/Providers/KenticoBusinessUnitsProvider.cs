using CMS.CustomTables;
using CMS.DataEngine;
using CMS.Ecommerce;
using CMS.Helpers;
using CMS.SiteProvider;
using Kadena.Models.BusinessUnit;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoBusinessUnitsProvider : IKenticoBusinessUnitsProvider
    {
        private readonly string BusinessUnitsCustomTableName = "KDA.BusinessUnit";
        private readonly string UserBusinessUnitsCustomTableName = "KDA.UserBusinessUnits";

        public List<BusinessUnit> GetBusinessUnits()
        {
            ObjectQuery<CustomTableItem> businessUnits = CustomTableItemProvider.GetItems(BusinessUnitsCustomTableName)
                                                                                .WhereEquals("SiteID", SiteContext.CurrentSiteID)
                                                                                .And()
                                                                                .WhereTrue("Status");
            return businessUnits.Select(x => CreateBusinessUnit(x)).ToList();
        }

        public List<BusinessUnit> GetUserBusinessUnits(int userID)
        {
            ObjectQuery<CustomTableItem> userBusinessUnits = CustomTableItemProvider.GetItems(UserBusinessUnitsCustomTableName)
                                                                                    .WhereEquals("UserID", userID)
                                                                                    .Columns("BusinessUnitID");
            if (userBusinessUnits.TypedResult.Items.Count > 0)
            {
                return userBusinessUnits.Select(x => CreateBusinessUnit(GetBusinessUnitItem(x))).ToList();
            }
            else
            {
                return null;
            }
        }

        private CustomTableItem GetBusinessUnitItem(CustomTableItem businessUnitItemID)
        {
            int itemID = businessUnitItemID.GetIntegerValue("BusinessUnitID", 0);
            return CustomTableItemProvider.GetItem(itemID, BusinessUnitsCustomTableName);
        }

        private BusinessUnit CreateBusinessUnit(CustomTableItem businessUnitItem)
        {
            if (businessUnitItem == null)
            {
                return null;
            }
            else
            {
                return new BusinessUnit()
                {
                    ItemID = businessUnitItem.ItemID,
                    SiteID = businessUnitItem.GetIntegerValue("SiteID", 0),
                    BusinessUnitName = businessUnitItem.GetStringValue("BusinessUnitName", string.Empty),
                    BusinessUnitNumber = businessUnitItem.GetIntegerValue("BusinessUnitNumber", 0),
                    Status = businessUnitItem.GetBooleanValue("Status", false)
                };
            }
        }

        public string GetDistributorBusinessUnit(int distributorID)
        {
            string businessUnit = string.Empty;
            if (distributorID > 0)
            {
                AddressInfo address = AddressInfoProvider.GetAddressInfo(distributorID);
                if (address != null)
                {
                    long businessUnitNumber = ValidationHelper.GetLong(address.GetValue("BusinessUnit"), default(long));
                    CustomTableItem businessUnitItem = CustomTableItemProvider.GetItems(BusinessUnitsCustomTableName, "BusinessUnitNumber=" + businessUnitNumber).FirstOrDefault();
                    businessUnit = businessUnitItem != null ? businessUnitItem.GetStringValue("BusinessUnitName", string.Empty) : string.Empty;
                }
            }
            return businessUnit;
        }
    }
}