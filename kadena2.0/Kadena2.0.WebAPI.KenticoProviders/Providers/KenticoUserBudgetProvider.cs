using AutoMapper;
using CMS.CustomTables;
using CMS.DataEngine;
using CMS.Helpers;
using Kadena.Models.FyBudget;
using Kadena.Models.UserBudget;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.WebAPI.KenticoProviders.Providers
{
    public class KenticoUserBudgetProvider : IkenticoUserBudgetProvider
    {
        private readonly IMapper mapper;

        private readonly string CustomTableClassName = "KDA.UserFYBudgetAllocation";

        private readonly string FiscalYearClassName = "KDA.FiscalYearManagement";

        public KenticoUserBudgetProvider(IMapper mapper)
        {
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }
            this.mapper = mapper;
        }
        public string UpdateUserBudgetAllocation(int itemID, decimal userBudget)
        {
            var userBudgetDetails = CustomTableItemProvider.GetItems(CustomTableClassName).WhereEquals("ItemID", itemID).FirstOrDefault();
            if (userBudgetDetails != null)
            {
                userBudgetDetails.SetValue("Budget", userBudget);
                userBudgetDetails.Update();
                userBudgetDetails.SetValue("UserRemainingBudget", userBudgetDetails.GetValue("UserRemainingBudget", default(decimal)) + (userBudgetDetails.GetValue("Budget", default(decimal)) - userBudgetDetails.GetValue("UserRemainingBudget", default(decimal))));
                userBudgetDetails.Update();
                return userBudgetDetails.GetValue("UserRemainingBudget", string.Empty);
            }

            return string.Empty;
        }

        public List<UserBudgetItem> GetUserBudgetAllocationRecords(int userId, int siteId)
        {
            List<UserBudgetItem> userBudgetItems = new List<UserBudgetItem>();
            var userBudgetDetails = CustomTableItemProvider.GetItems(CustomTableClassName).WhereEquals("UserID", userId).WhereEquals("SiteID", siteId).ToList();
            if (userBudgetDetails.Count > 0)
            {
                foreach (CustomTableItem item in userBudgetDetails)
                {
                    userBudgetItems.Add(new UserBudgetItem()
                    {
                        ItemID = item.ItemID,
                        Budget = item.GetValue("Budget",default(decimal)),
                        Year = item.GetValue("Year", default(string)),
                        UserRemainingBudget = item.GetValue("UserRemainingBudget", default(decimal)),
                        UserID = item.GetValue("UserID", default(int))
                    });
                }
            }
            return userBudgetItems;
        }

        public bool CheckIfYearExists(string year, int userId)
        {
            var userBudgetDetails = CustomTableItemProvider.GetItems(CustomTableClassName).WhereEquals("UserID", userId).WhereEquals("Year", year).ToList();
            return !DataHelper.DataSourceIsEmpty(userBudgetDetails) ? true : false;

        }

        public void UpdateUserBudgetAllocationRecords(int userId, string year, decimal? totalToBeDeducted)
        {
            var userBudgetDetails = CustomTableItemProvider.GetItems(CustomTableClassName).WhereEquals("UserID", userId).WhereEquals("Year", year).FirstOrDefault();
            userBudgetDetails.SetValue("UserRemainingBudget", userBudgetDetails.GetValue("Budget", default(decimal)) - totalToBeDeducted);
            userBudgetDetails.Update();
        }

        public List<FiscalYear> GetFiscalYearRecords()
        {
            List<FiscalYear> fiscalYearsList = new List<FiscalYear>();
            var fiscalYearData = CustomTableItemProvider.GetItems(FiscalYearClassName).ToList();
            if (fiscalYearData.Count > 0)
            {
                foreach (CustomTableItem item in fiscalYearData)
                {
                    fiscalYearsList.Add(new FiscalYear()
                    {
                        ItemID = item.ItemID,
                        Year = item.GetStringValue("Year", string.Empty),
                        StartDate = item.GetDateTimeValue("FiscalYearStartDate", DateTime.Now),
                        EndDate = item.GetDateTimeValue("FiscalYearEndDate", DateTime.Now)
                    });
                }
            }
            return fiscalYearsList;
        }

        public UserBudgetItem CreateUserBudgetWithYear(string year, int siteID, int userId)
        {
            DataClassInfo customTable = DataClassInfoProvider.GetDataClassInfo(CustomTableClassName);
            CustomTableItem newCustomTableItem = CustomTableItem.New(CustomTableClassName);
            if (customTable != null)
            {
                newCustomTableItem.SetValue("UserID", userId);
                newCustomTableItem.SetValue("Year", year);
                newCustomTableItem.SetValue("Budget", default(decimal));
                newCustomTableItem.SetValue("UserRemainingBudget", default(decimal));
                newCustomTableItem.SetValue("SiteID", siteID);
                newCustomTableItem.Insert();
                return new UserBudgetItem()
                {
                    ItemID = newCustomTableItem.ItemID,
                    Budget = newCustomTableItem.GetValue("Budget", default(decimal)),
                    UserID = userId,
                    UserRemainingBudget = newCustomTableItem.GetValue("UserRemainingBudget", default(decimal)),
                    Year = year
                };
            }
            return null;
        }
    }
}
