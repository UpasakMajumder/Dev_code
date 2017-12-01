using CMS.Base;
using Kadena.BusinessLogic.Contracts;
using System.Web.Http.Filters;

namespace Kadena.WebAPI.Infrastructure.Filters
{
    public class RecalculateShoppingCartAttribute : ActionFilterAttribute
    {
        public virtual IShoppingCartService ShoppingCartService { get; set; }

        public override bool AllowMultiple => false;

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            // CMSThread is supposed to copy the current context and make it available in the new thread
            var t = new CMSThread(new System.Threading.ThreadStart(() => 
            {
                ShoppingCartService.GetDeliveryAndTotals().Wait();

            }), new ThreadSettings() { UseEmptyContext = false });

            t.Start();
        }
    }
}