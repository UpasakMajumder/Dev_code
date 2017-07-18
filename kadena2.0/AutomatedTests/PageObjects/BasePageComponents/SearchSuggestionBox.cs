using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedTests.PageObjects.BasePageComponents
{
    class SearchSuggestionBox
    {
        [FindsBy(How = How.CssSelector, Using = ".search__results a")]
        private IList<IWebElement> ProductSuggestions { get; set; }

        [FindsBy(How = How.ClassName, Using = "search__dropdown")]
        private IWebElement SuggestionBox { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".search__dropdown>div>div:first-child .search__header a")]
        private IWebElement ShowAllProductsBtn { get; set; }

        public SearchSuggestionBox()
        {
            PageFactory.InitElements(Browser.Driver, this);
        }

        /// <summary>
        /// Returns true if there are more than three suggestions for products
        /// </summary>
        /// <returns></returns>
        public bool AreThereMoreThanThreeSuggestions()
        {
            WaitForSuggestionBox();
            if (ProductSuggestions.Count == 0)
            {
                throw new Exception("There are no suggestions for products");
            }
            return ProductSuggestions.Count > 3;
        }

        public void WaitForSuggestionBox()
        {
            SuggestionBox.WaitTillVisible();
        }

        /// <summary>
        /// Clicks Show All Products
        /// </summary>
        /// <returns></returns>
        public Serp ClickShowAllProducts()
        {
            ShowAllProductsBtn.ClickElement();
            return new Serp();
        }
    }
}
