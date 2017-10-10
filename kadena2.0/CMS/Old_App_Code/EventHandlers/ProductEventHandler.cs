using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Ecommerce;
using CMS.EventLog;
using Kadena.Old_App_Code.EventHandlers;
using System;

[assembly: CMS.RegisterModule(typeof(ProductEventHandler))]

namespace Kadena.Old_App_Code.EventHandlers
{
    public class ProductEventHandler : Module
    {
        public ProductEventHandler() : base(nameof(ProductEventHandler))
        {
        }

        protected override void OnInit()
        {
            base.OnInit();

            DocumentEvents.Insert.After += CopyProductSKUFieldsToSKU;
            DocumentEvents.Update.After += CopyProductSKUFieldsToSKU;
        }

        private void CopyProductSKUFieldsToSKU(object sender, DocumentEventArgs e)
        {
            if (e.Node.ClassName != "KDA.Product")
            {
                return;
            }

            var productNode = e.Node;
            var nodeSkuId = productNode.GetIntegerValue("NodeSKUID", 0);
            if (nodeSkuId == 0)
            {
                return;
            }

            var productSkuWeight = productNode.GetDoubleValue("SKUWeight", 0);
            var productSkuNeedsShipping = productNode.GetBooleanValue("SKUNeedsShipping", false);

            try
            {
                var sku = SKUInfoProvider.GetSKUInfo(nodeSkuId);
                sku.SKUWeight = productSkuWeight;
                sku.SKUNeedsShipping = productSkuNeedsShipping;
                sku.Update();
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException(nameof(ProductEventHandler), "EXCEPTION", ex, additionalMessage: "");
            }
        }
    }
}