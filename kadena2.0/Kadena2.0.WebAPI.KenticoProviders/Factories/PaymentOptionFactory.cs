using Kadena.Models;
using CMS.Ecommerce;
using System.Linq;

namespace Kadena2.WebAPI.KenticoProviders.Factories
{
    public static class PaymentOptionFactory
    {
        public static PaymentMethod CreateMethod(PaymentOptionInfo p)
        {
            if (p == null)
            {
                return null;
            }

            return new PaymentMethod()
            {
                Id = p.PaymentOptionID,
                Checked = false,
                Disabled = !p.PaymentOptionEnabled,
                Icon = p.GetStringValue("IconResource", string.Empty),
                DisplayName = p.PaymentOptionDisplayName,
                ClassName = p.PaymentOptionName,
                IsUnpayable = p.GetBooleanValue("IsUnpayable", false)
            };
        }

        public static PaymentMethod[] CreateMethods(PaymentOptionInfo[] ps)
        {
            return ps.Select(p => CreateMethod(p)).ToArray();
        }
    }
}
