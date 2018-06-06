using CMS;
using CMS.DataEngine;
using CMS.Ecommerce;
using CMS.EmailEngine;
using CMS.EventLog;
using CMS.SiteProvider;
using System;

[assembly: RegisterModule(typeof(Kadena.Old_App_Code.EventHandlers.ProductCreationHandler))]

namespace Kadena.Old_App_Code.EventHandlers
{
    public class ProductCreationHandler : Module
    {
        public ProductCreationHandler() : base("ProductCreationHandler") { }

        protected override void OnInit()
        {
            base.OnInit();
            SKUInfo.TYPEINFOSKU.Events.Insert.After += Insert_After;
        }

        private void Insert_After(object sender, ObjectEventArgs e)
        {
            if (e.Object is SKUInfo sku)
            {
                var template = EmailTemplateProvider.GetEmailTemplate("KDAProductCreated", SiteContext.CurrentSiteName);
                var resolver = EcommerceResolvers.SKUResolver;
                resolver.SourceObject = sku;
                try
                {
                    EmailSender.SendEmailWithTemplateText(SiteContext.CurrentSiteName, new EmailMessage(), template, resolver, sendImmediately: true);
                }
                catch (Exception ex)
                {
                    EventLogProvider.LogException(this.GetType().Name, "SKUEVENT", ex);
                }
            }
        }
    }
}