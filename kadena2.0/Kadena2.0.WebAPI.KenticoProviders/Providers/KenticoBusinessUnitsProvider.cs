using AutoMapper;
using CMS.CustomTables;
using CMS.Ecommerce;
using CMS.Helpers;
using CMS.SiteProvider;
using Kadena.Models.BusinessUnit;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoBusinessUnitsProvider : IKenticoBusinessUnitsProvider
    {
        private readonly string BusinessUnitsCustomTableName = "KDA.BusinessUnit";
        private readonly string UserBusinessUnitsCustomTableName = "KDA.UserBusinessUnits";
        private readonly IMapper mapper;

        public KenticoBusinessUnitsProvider(IMapper mapper)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public List<BusinessUnit> GetBusinessUnits()
        {
            var businessUnits = CustomTableItemProvider.GetItems(BusinessUnitsCustomTableName)
                                                                                .WhereEquals("SiteID", SiteContext.CurrentSiteID)
                                                                                .And()
                                                                                .WhereTrue("Status");
            return mapper.Map<BusinessUnit[]>(businessUnits).ToList();
        }

        public List<BusinessUnit> GetUserBusinessUnits(int userID)
        {
            var userBusinessUnits = CustomTableItemProvider.GetItems(BusinessUnitsCustomTableName)
                                        .WhereEquals("SiteID", SiteContext.CurrentSiteID)
                                        .WhereIn("ItemID",
                                            CustomTableItemProvider.GetItems(UserBusinessUnitsCustomTableName)
                                                .WhereEquals("UserID", userID)
                                                .Columns("BusinessUnitID"))
                                        .ToList();

            return userBusinessUnits.Count > 0 ? mapper.Map<BusinessUnit[]>(userBusinessUnits).ToList() : null;
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