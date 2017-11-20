using CMS;
using CMS.Ecommerce.Web.UI;
using Kadena.Old_App_Code.Kadena.PaymentMethods;

[assembly: RegisterCustomClass("PurchaseOrder", typeof(PurchaseOrder))]

namespace Kadena.Old_App_Code.Kadena.PaymentMethods
{
    public class PurchaseOrder : CMSPaymentGatewayProvider
    {
    }
}