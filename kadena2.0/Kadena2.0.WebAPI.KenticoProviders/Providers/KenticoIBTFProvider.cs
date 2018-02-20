using CMS.CustomTables;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.SiteProvider;
using Kadena.Models.Brand;
using Kadena.Models.IBTF;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoIBTFProvider : IKenticoIBTFProvider
    {
        private readonly string IBTFAdjustmentCustomTableName = "KDA.IBTFAdjustment";
        private readonly string IBTFCustomTableName = "KDA.InboundTracking";

        public void InsertIBTFAdjustmentRecord(IBTFAdjustment inboundAdjustment)
        {
            DataClassInfo customTable = DataClassInfoProvider.GetDataClassInfo(IBTFAdjustmentCustomTableName);
            if (customTable != null)
            {
                CustomTableItem newCustomTableItem = CustomTableItem.New(IBTFAdjustmentCustomTableName);
                newCustomTableItem.SetValue("SKUID", inboundAdjustment.SKUID);
                newCustomTableItem.SetValue("UserID", inboundAdjustment.UserID);
                newCustomTableItem.SetValue("CampaignID", inboundAdjustment.CampaignID);
                newCustomTableItem.SetValue("OrderedQuantity", inboundAdjustment.OrderedQuantity);
                newCustomTableItem.SetValue("OrderedProductPrice", inboundAdjustment.OrderedProductPrice);
                newCustomTableItem.SetValue("SiteName", SiteContext.CurrentSiteName);
                newCustomTableItem.Insert();
            }
        }

        public List<IBTFAdjustment> GetIBTFAdjustmentRecords()
        {
            return CustomTableItemProvider.GetItems(IBTFAdjustmentCustomTableName)
                .WhereEquals("SiteName", SiteContext.CurrentSiteName)
                .Select(x =>
                {
                    return new IBTFAdjustment()
                    {
                        ItemID = ValidationHelper.GetInteger(x["ItemID"], default(int)),
                        SKUID = ValidationHelper.GetInteger(x["SKUID"], default(int)),
                        UserID = ValidationHelper.GetInteger(x["UserID"], default(int)),
                        CampaignID = ValidationHelper.GetInteger(x["CampaignID"], default(int)),
                        OrderedQuantity = ValidationHelper.GetInteger(x["OrderedQuantity"], default(int)),
                        OrderedProductPrice = ValidationHelper.GetDecimal(x["OrderedProductPrice"], default(decimal))
                    };
                }).ToList();
        }

        public List<IBTF> GetIBTFRecords()
        {
            return CustomTableItemProvider.GetItems(IBTFCustomTableName)
                .Select(x =>
                {
                    return new IBTF()
                    {
                        ItemID = ValidationHelper.GetInteger(x["ItemID"], default(int)),
                        SKUID = ValidationHelper.GetInteger(x["SKUID"], default(int)),
                        ActualPrice = ValidationHelper.GetDecimal(x["ActualPrice"], default(decimal))
                    };
                }).ToList();
        }
    }
}