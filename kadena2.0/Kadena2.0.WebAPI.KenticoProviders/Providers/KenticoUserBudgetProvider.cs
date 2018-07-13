using AutoMapper;
using CMS.CustomTables;
using CMS.Ecommerce;
using CMS.SiteProvider;
using Kadena.Models.FyBudget;
using Kadena.Models.UserBudget;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.WebAPI.KenticoProviders.Providers
{
    public class KenticoUserBudgetProvider : IKenticoUserBudgetProvider
    {
        private readonly IMapper mapper;
        private readonly IKenticoLogger logger;
        private readonly string CustomTableClassName = "KDA.UserFYBudgetAllocation";

        private readonly string FiscalYearClassName = "KDA.FiscalYearManagement";

        public KenticoUserBudgetProvider(IMapper mapper, IKenticoLogger logger)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public string UpdateUserBudgetAllocation(int itemID, decimal userBudget)
        {
            var userBudgetDetails = CustomTableItemProvider.GetItems(CustomTableClassName).WhereEquals("ItemID", itemID).FirstOrDefault();
            if (userBudgetDetails != null)
            {
                var oldBudget = userBudgetDetails.GetValue("Budget", default(decimal));
                var newBudget = userBudget;
                userBudgetDetails.SetValue("Budget", newBudget);
                userBudgetDetails.Update();
                userBudgetDetails.SetValue("UserRemainingBudget", userBudgetDetails.GetValue("UserRemainingBudget", default(decimal)) + (newBudget - oldBudget));
                userBudgetDetails.Update();
                return CurrencyInfoProvider.GetFormattedPrice(userBudgetDetails.GetValue("UserRemainingBudget", default(double)), SiteContext.CurrentSiteID, true);
            }

            return string.Empty;
        }

        public List<UserBudgetItem> GetUserBudgetAllocationRecords(int userId, int siteId)
        {
            var userBudgetItems = new List<UserBudgetItem>();
            var userBudgetDetails = CustomTableItemProvider.GetItems(CustomTableClassName)
                .WhereEquals("UserID", userId)
                .WhereEquals("SiteID", siteId)
                .ToList()
                .Select(MapToUserBudgetItem)
                .ToList();
            return userBudgetDetails;
        }

        private UserBudgetItem MapToUserBudgetItem(CustomTableItem item) => new UserBudgetItem()
        {
            ItemID = item.ItemID,
            Budget = item.GetValue("Budget", default(decimal)),
            UserID = item.GetValue("UserID", 0),
            UserRemainingBudget = item.GetValue("UserRemainingBudget", default(decimal)),
            Year = item.GetValue("Year", default(string))
        };

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

        public UserBudgetItem GetOrCreateUserBudgetWithYear(string year, int siteID, int userId)
        {
            var userBudget = CustomTableItemProvider.GetItems(CustomTableClassName)
                .WhereEquals("UserID", userId)
                .WhereEquals("SiteID", siteID)
                .WhereEquals("Year", year)
                .FirstOrDefault();

            if (userBudget == null)
            {
                userBudget = CustomTableItem.New(CustomTableClassName);
                userBudget.SetValue("UserID", siteID);
                userBudget.SetValue("Year", year);
                userBudget.SetValue("Budget", 0);
                userBudget.SetValue("UserRemainingBudget", 0);
                userBudget.SetValue("SiteID", siteID);
                userBudget.Insert();
            }

            return MapToUserBudgetItem(userBudget);
        }

        public void AdjustUserRemainingBudget(string year, int userID, decimal adjustment)
        {
            var userBudgetDetails = CustomTableItemProvider
                .GetItems(CustomTableClassName)
                .WhereEquals("UserID", userID)
                .WhereEquals("Year", year)
                .FirstOrDefault();

            if (userBudgetDetails != null)
            {
                userBudgetDetails.SetValue("UserRemainingBudget", userBudgetDetails.GetValue("UserRemainingBudget", default(decimal)) + (adjustment));
                userBudgetDetails.Update();
            }
            else
            {
                logger.LogInfo(
                    nameof(KenticoUserBudgetProvider),
                    nameof(AdjustUserRemainingBudget),
                    $"User budget allocation for user {userID} and year {year} not found.");
            }
        }
    }
}
