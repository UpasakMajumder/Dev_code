using CMS.DataEngine;
using CMS.DocumentEngine;
using Kadena.Models.Product;
using Kadena.Old_App_Code.EventHandlers;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Container.Default;
using Kadena2.WebAPI.KenticoProviders.Classes;
using System;
using Kadena.BusinessLogic.Contracts;

[assembly: CMS.RegisterModule(typeof(ProductEventHandler))]

namespace Kadena.Old_App_Code.EventHandlers
{
    public class ProductEventHandler : Module
    {
        public virtual IKenticoSkuProvider SkuProvider { get; set; } = DIContainer.Resolve<IKenticoSkuProvider>() ?? throw new NullReferenceException(nameof(IKenticoSkuProvider));
        public virtual IKenticoLogger Logger { get; set; } = DIContainer.Resolve<IKenticoLogger>();
        public virtual INewProductNotificationService NewProductNotificationService { get; set; } = DIContainer.Resolve<INewProductNotificationService>() ?? throw new NullReferenceException(nameof(INewProductNotificationService));

        public ProductEventHandler() : base(nameof(ProductEventHandler))
        {
        }

        protected override void OnInit()
        {
            base.OnInit();

            DocumentEvents.Insert.After += CopyProductSKUFieldsToSKU_EventHandler;
            DocumentEvents.Insert.After += CopyCampaignsProductSKUFieldsToSKU_EventHandler;
            DocumentEvents.Insert.After += SendNewProductNotificationEmail_EventHandler;
            DocumentEvents.Update.After += CopyProductSKUFieldsToSKU_EventHandler;
            DocumentEvents.Update.After += CopyCampaignsProductSKUFieldsToSKU_EventHandler;
        }

        public void SendNewProductNotificationEmail_EventHandler(object sender, DocumentEventArgs e)
        {
            var product = GetProductFromNode(e.Node);
            if (product != null)
            {
                NewProductNotificationService.SendNotification(product.NodeSKUID);
            }
        }

        public void CopyProductSKUFieldsToSKU_EventHandler(object sender, DocumentEventArgs e)
        {
            var product = GetProductFromNode(e.Node);
            if (product != null)
            {
                CopyProductSKUFieldsToSKU(product);
            }
        }

        public void CopyCampaignsProductSKUFieldsToSKU_EventHandler(object sender, DocumentEventArgs e)
        {
            var product = GetCampaignsProductFromNode(e.Node);
            if (product != null)
            {
                CopyProductSKUFieldsToSKU(product);
            }
        }

        protected virtual IProductClass GetProductFromNode(TreeNode node)
        {
            return new ProductNodeWrapper(node).ToProduct();
        }

        protected virtual IProductClass GetCampaignsProductFromNode(TreeNode node)
        {
            return new CampaignsProductNodeWrapper(node).ToProduct();
        }

        protected virtual void CopyProductSKUFieldsToSKU(IProductClass product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            try
            {
                var sku = new Sku
                {
                    SkuId = product.NodeSKUID,
                    NeedsShipping = product.SKUNeedsShipping,
                    Weight = product.SKUWeight
                };
                SkuProvider.UpdateSkuMandatoryFields(sku);
            }
            catch (Exception ex)
            {
                Logger.LogException(nameof(ProductEventHandler), ex);
            }
        }
    }
}