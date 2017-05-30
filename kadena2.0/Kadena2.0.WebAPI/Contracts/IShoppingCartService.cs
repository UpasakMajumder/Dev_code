using CMS.Ecommerce;
using Kadena.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kadena.WebAPI.Contracts
{
    public interface IShoppingCartService
    {
        CheckoutPage GetCheckoutPage();
        List<PaymentMethod> OrderPaymentMethods(PaymentMethod[] methods);

        CheckoutPage SelectShipipng(int id);

        CheckoutPage SelectAddress(int id);
    }
}
