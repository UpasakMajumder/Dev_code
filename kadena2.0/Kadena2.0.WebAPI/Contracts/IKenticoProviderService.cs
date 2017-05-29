using CMS.Ecommerce;
using Kadena.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kadena.WebAPI.Contracts
{
    public interface IKenticoProviderService
    {
        DeliveryAddress[] GetCustomerAddresses();
        DeliveryMethod[] GetShippingCarriers();
        DeliveryService[] GetShippingOptions();
    }
}
