using AutomatedTests.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedTests.PageObjects
{
    class Checkout
    {
        [FindsBy(How = How.CssSelector, Using = ".PanelShipping select")]
        private IWebElement ShippingCostDropdownElement { get; set; }

        public Checkout()
        {
            PageFactory.InitElements(Browser.Driver, this);
        }
        public void Open()
        {
            Browser.Driver.Navigate().GoToUrl(TestEnvironment.Url + "/checkout");
           
        }

        public SelectElement ShippingCostDropDown()
        {
            return new SelectElement(ShippingCostDropdownElement);
        }

        /// <summary>
        /// Iterates through Shipping options and returns true when it finds an option with estimated shipping price
        /// </summary>
        /// <returns></returns>
        public bool IsShippingCostEstimated()
        {
            if (ShippingCostDropDown().Options.Count == 0)
            {
                return false;
            }

            for (int i = 0; i < ShippingCostDropDown().Options.Count; i++)
            {
                if (ShippingCostDropDown().Options[i].Text.Contains("$"))
                {
                    return true;
                }
            }
            return false;
        }
    }


}
