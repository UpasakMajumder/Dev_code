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
    class ProductDetail : BasePage
    {
        [FindsBy(How = How.ClassName, Using = "btn-action")]
        private IWebElement AddToCart { get; set; }
        public ProductDetail()
        {
            PageFactory.InitElements(Browser.Driver, this);
        }

        public void ClickAddToCart()
        {
            AddToCart.ClickElement();
        }

        public void Open(string categoryName, string productName)
        {
            Browser.Driver.Navigate().GoToUrl(TestEnvironment.Url + "/products/" + categoryName + "/" + productName);
        }
    }
}
