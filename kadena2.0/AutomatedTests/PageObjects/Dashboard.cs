using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using AutomatedTests.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;

namespace AutomatedTests.PageObjects
{
    class Dashboard : BasePage
    {
        [FindsBy(How = How.Id, Using = "dashboard")]
        private IWebElement dashboard { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".row a")]
        private IList<IWebElement> NewProducts { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".show-table")]
        private IWebElement RecentOrders { get; set; }

        public Dashboard()
        {
            PageFactory.InitElements(Browser.Driver, this);
        }

        /// <summary>
        /// Opens dashboard if you are on different page.
        /// </summary>
        public void Open()
        {
            if (!Browser.GetUrl().Contains("dashboard"))
            {
                Browser.Driver.Navigate().GoToUrl(TestEnvironment.Url);
            }
        }

        public bool IsDashboardDisplayed()
        {
            return dashboard.IsDisplayed();
        }

        /// <summary>
        /// Iterates through new products and when it finds one which can be addet to cart, it is added to cart
        /// </summary>
        /// <returns></returns>
        public ProductDetail SelectProductYouCanAddToCart()
        {
            //Select product, check if it has add to cart button, if not, go back and select another one. 
            //Repeat until product with add to cart button is found.
            for (int i = 0; i < NewProducts.Count; i++)
            {
                NewProducts[i].ClickElement();
                var productDetail = new ProductDetail();

                if (productDetail.IsAddToCartButtonDisplayed())
                {
                    productDetail.ClickAddToCart();
                    productDetail.AcceptItemIsAddedAlert();
                    return productDetail;
                }
                else
                {
                    Browser.Driver.Navigate().Back();
                }
            }

            throw new Exception("There is no product you can add to cart");
        }

        /// <summary>
        /// Waits until recent orders are on the page
        /// </summary>
        public void WaitForRecentOrders()
        {
            Browser.BaseWait.Until(r => RecentOrders.IsPresent());
        }
    }
}
