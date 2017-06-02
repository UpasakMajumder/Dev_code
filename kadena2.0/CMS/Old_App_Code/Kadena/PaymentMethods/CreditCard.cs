using CMS;
using CMS.Ecommerce.Web.UI;
using Kadena.Old_App_Code.Kadena.PaymentMethods;

[assembly: RegisterCustomClass("CreditCard", typeof(CreditCard))]

namespace Kadena.Old_App_Code.Kadena.PaymentMethods
{
    public class CreditCard : CMSPaymentGatewayProvider
    {
    }
}