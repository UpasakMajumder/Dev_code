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

        DeliveryAddress[] GetCustomerAddresses();
        DeliveryMethod[] GetShippingCarriers();

        DeliveryService[] GetShippingOptions();
    }
}
