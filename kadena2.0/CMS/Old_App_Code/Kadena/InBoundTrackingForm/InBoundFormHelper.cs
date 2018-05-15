using CMS.CustomTables;
using CMS.CustomTables.Types.KDA;
using CMS.EventLog;
using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using System;
using System.Linq;

namespace Kadena.Old_App_Code.Kadena.InBoundForm
{
    public class InBoundFormHelper
    {
        public static void InsertIBFForm(OrderDTO orderDetails)
        {
            try
            {
                var productItems = orderDetails.Items.ToList();
                foreach (var product in productItems)
                {
                    bool isExist = true;
                    InboundTrackingItem inboundData = CustomTableItemProvider.GetItems<InboundTrackingItem>().WhereEquals("SKUID", product.SKU.KenticoSKUID).FirstOrDefault();
                    if (inboundData == null)
                    {
                        inboundData = new InboundTrackingItem();
                        isExist = false;
                    }
                    inboundData.SKUID = product.SKU.KenticoSKUID;
                    inboundData.QtyOrdered += product.UnitCount;
                    if (!isExist)
                    {
                        inboundData.Insert();
                    }
                    else
                    {
                        inboundData.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("ShoppingCartHelper", "GetOrderTotal", ex.Message);
            }
        }
    }
}