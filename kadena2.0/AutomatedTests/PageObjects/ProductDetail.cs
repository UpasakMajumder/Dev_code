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
        [FindsBy(How = How.CssSelector, Using = ".add-to-cart .btn-action")]
        private IWebElement AddToCart { get; set; }

        public ProductDetail()
        {
            PageFactory.InitElements(Browser.Driver, this);
        }

        /// <summary>
        /// Adds product to cart
        /// </summary>
        public void ClickAddToCart()
        {
            AddToCart.ClickElement();
        }

        /// <summary>
        /// Navigates to the product you specify by its category and name
        /// </summary>
        /// <param name="categoryName">name of existing category</param>
        /// <param name="productName">name of existing product</param>
        public void Open(string categoryName, string productName)
        {
            Browser.Driver.Navigate().GoToUrl($"{TestEnvironment.Url}/products/{categoryName}/{productName}");
        }

        public void AcceptItemIsAddedAlert()
        {
            Browser.AcceptAlert();
        }

        /// <summary>
        /// Checks if add to cart button is displayed
        /// </summary>
        /// <returns></returns>
        public bool IsAddToCartButtonDisplayed()
        {
            try
            {
                //it is needed to look for text because Open Template In Design button has same selector
                return AddToCart.GetText().Contains("Add to");
            }
            catch
            {
                AddToCart.WaitTillVisible();
                return AddToCart.GetText().Contains("Add to");
            }
        }
    }
}
