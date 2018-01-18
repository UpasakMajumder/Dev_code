using CMS.DataEngine;
using CMS.DocumentEngine;
using Kadena.Models.Product;
using Kadena.Old_App_Code.EventHandlers;
using Kadena.Old_App_Code.Kadena;
using Kadena.WebAPI.KenticoProviders;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.Container.Default;
using Kadena2.WebAPI.KenticoProviders.Classes;
using System;

[assembly: CMS.RegisterModule(typeof(ProductEventHandler))]

namespace Kadena.Old_App_Code.EventHandlers
{
    public class ProductEventHandler : Module
    {
        public virtual IKenticoProductsProvider ProductsProvider { get; set; } = new KenticoProductsProvider( MapperBuilder.MapperInstance );
        public virtual IKenticoLogger Logger { get; set; } = ContainerBuilder.Resolve<IKenticoLogger>();

        public ProductEventHandler() : base(nameof(ProductEventHandler))
        {
        }

        protected override void OnInit()
        {
            base.OnInit();

            DocumentEvents.Insert.After += CopyProductSKUFieldsToSKU_EventHandler;
            DocumentEvents.Update.After += CopyProductSKUFieldsToSKU_EventHandler;
        }

        public void CopyProductSKUFieldsToSKU_EventHandler(object sender, DocumentEventArgs e)
        {
            var product = GetProductFromNode(e.Node);
            CopyProductSKUFieldsToSKU(product);
        }

        protected virtual ProductClass GetProductFromNode(TreeNode node)
        {
            return new ProductClassWrapper(node).ToProduct();
        }

        protected virtual void CopyProductSKUFieldsToSKU(ProductClass product)
        {
            if (product == null)
            {
                return;
            }

            try
            {
                var sku = new Sku
                {
                    SkuId = product.NodeSKUID,
                    NeedsShipping = product.SKUNeedsShipping,
                    Weight = product.SKUWeight
                };
                ProductsProvider.UpdateSku(sku);
            }
            catch (Exception ex)
            {
                Logger.LogException(nameof(ProductEventHandler), ex);
            }
        }
    }
}