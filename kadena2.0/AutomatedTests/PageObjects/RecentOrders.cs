using AutomatedTests.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedTests.PageObjects
{
    class RecentOrders : BasePage
    {
        [FindsBy(How = How.CssSelector, Using = ".r-recent-orders tr")]
        private IList<IWebElement> Orders { get; set; }

        public RecentOrders()
        {
            PageFactory.InitElements(Browser.Driver, this);
        }

        public void Open()
        {
            Browser.Driver.Navigate().GoToUrl($"{TestEnvironment.Url}/recent-orders");
            WaitForLoading();
        }

        /// <summary>
        /// Returns true if there is at least one order
        /// </summary>
        /// <returns></returns>
        public bool IsThereAnyOrderInTable()
        {
            //not 0, first row is header
            return Orders.Count > 1;
        }


    }
}
