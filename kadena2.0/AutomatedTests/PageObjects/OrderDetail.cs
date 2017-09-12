using AutomatedTests.Utilities;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedTests.PageObjects
{
    class OrderDetail : BasePage
    {


        public OrderDetail()
        {
            PageFactory.InitElements(Browser.Driver, this);
        }

        /// <summary>
        /// Opens order detail page of specific order
        /// </summary>
        /// <param name="orderID"></param>
        public void Open(string orderID)
        {
            Browser.Driver.Navigate().GoToUrl($"{TestEnvironment.Url}/recent-orders/order-detail?orderID=" + orderID);
        }


    }
}
