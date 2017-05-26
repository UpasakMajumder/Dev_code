using Kadena.WebAPI.Contracts;
using CMS.Ecommerce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kadena.WebAPI.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        public string Test()
        {
            return ECommerceContext.CurrentCustomer.ToString();
        }
    }
}