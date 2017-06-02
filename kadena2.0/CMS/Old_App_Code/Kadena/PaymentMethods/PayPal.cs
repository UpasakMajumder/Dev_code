using CMS;
using CMS.Ecommerce.Web.UI;
using Kadena.Old_App_Code.Kadena.PaymentMethods;

[assembly: RegisterCustomClass("CreditCard", typeof(PayPal))]

namespace Kadena.Old_App_Code.Kadena.PaymentMethods
{
    public class PayPal : CMSPaymentGatewayProvider
    {
    }
}