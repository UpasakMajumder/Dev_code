﻿using CMS.CustomTables;
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
            var businessUnits = CustomTableItemProvider.GetItems(BusinessUnitsCustomTableName)
                                                                                .WhereEquals("SiteID", SiteContext.CurrentSiteID)
                                                                                .And()
                                                                                .WhereTrue("Status");
            return businessUnits.Select(x => CreateBusinessUnit(x)).ToList();
        }

        public List<BusinessUnit> GetUserBusinessUnits(int userID)
        {
            var userBusinessUnits = CustomTableItemProvider.GetItems(BusinessUnitsCustomTableName)
                                        .WhereEquals("SiteID", SiteContext.CurrentSiteID)
                                        .WhereIn("ItemID", 
                                            CustomTableItemProvider.GetItems(UserBusinessUnitsCustomTableName)
                                                .WhereEquals("UserID", userID)
                                                .Columns("BusinessUnitID"));

            if (userBusinessUnits.TypedResult.Items.Count > 0)
            {
                return userBusinessUnits.Select(x => CreateBusinessUnit(x)).ToList();
            }
            else
            {
                return null;
            }
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

        public string GetDistributorBusinessUnitNumber(int distributorID)
        {
            string businessUnit = string.Empty;
            if (distributorID > 0)
            {
                AddressInfo address = AddressInfoProvider.GetAddressInfo(distributorID);
                if (address != null)
                {
                    long businessUnitNumber = ValidationHelper.GetLong(address.GetValue("BusinessUnit"), default(long));
                    CustomTableItem businessUnitItem = CustomTableItemProvider.GetItems(BusinessUnitsCustomTableName, "BusinessUnitNumber=" + businessUnitNumber).FirstOrDefault();
                    businessUnit = businessUnitItem != null ? businessUnitItem.GetStringValue("BusinessUnitNumber", string.Empty) : string.Empty;
                }
            }
            return businessUnit;
        }

        public string GetBusinessUnitName(long businessUnitNumber)
        {
            if (businessUnitNumber == 0) return string.Empty;

            var businessUnitItem = CustomTableItemProvider
                .GetItems(BusinessUnitsCustomTableName)
                .WhereEquals("BusinessUnitNumber", businessUnitNumber)
                .FirstOrDefault();
            return businessUnitItem != null
                ? businessUnitItem.GetStringValue("BusinessUnitName", string.Empty)
                : string.Empty;
        }
    }
}