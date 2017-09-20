using AutomatedTests.PageObjects.BasePageComponents;
using AutomatedTests.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutomatedTests.PageObjects
{
    class BasePage
    {
        [FindsBy(How = How.TagName, Using = "html")]
        public IWebElement html { get; set; }

        [FindsBy(How = How.ClassName, Using = "main-container")]
        public IWebElement MainContainer { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".r-spinner")]
        private IWebElement Spinner { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".search__input input")]
        private IWebElement SearchBar { get; set; }

        [FindsBy(How = How.Id, Using = "js-logout")]
        private IWebElement LogoutBtn { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".js-cart-preview>a")]


        [FindsBy(How = How.CssSelector, Using = ".cart-preview__empty")]
        private IWebElement EmptyCartPreview { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".cart-preview__product")]
        private IList<IWebElement> CartPreviewProducts { get; set; }

        private IWebElement ShoppingCartBtn
        {
            get
            {
                return Browser.Driver.FindElement(By.CssSelector(".js-cart-preview>a"));
            }
        }

        public SearchSuggestionBox searchSuggestionBox;

        public BasePage()
        {
            PageFactory.InitElements(Browser.Driver, this);
            this.searchSuggestionBox = new SearchSuggestionBox();
        }

        /// <summary>
        /// Waits for main container to be displayed
        /// </summary>
        public void WaitForKadenaPageLoad()
        {
            MainContainer.WaitTillVisible();
        }

        /// <summary>
        /// Waits until spinner appears and disappears
        /// </summary>
        public void WaitForLoading()
        {
            Spinner.WaitTillVisible();
            Spinner.WaitTillNotVisible();
        }

        public void SearchForText(string text)
        {
            SearchBar.EnterText(text);
        }

        public bool IsLogoutButtonDisplayed()
        {
            try
            {
                return LogoutBtn.IsDisplayed();
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        /// <summary>
        /// Moves cursor to shopping cart button to display cart preview
        /// </summary>
        public void MoveToShoppingCartBtn()
        {
            //try catch in loop to avoid stale element exception
            for (int i = 0; i < 30; i++)
            {
                try
                {
                    ShoppingCartBtn.MoveToMyElement();
                    return;
                }
                catch
                {
                    Thread.Sleep(1000);
                }
            }
            throw new Exception();
        }

        /// <summary>
        /// Returns true if component indicating that the cart is empty is displayed
        /// </summary>
        /// <returns></returns>
        public bool IsShoppingCartPreviewEmpty()
        {
            try
            {
                return EmptyCartPreview.IsDisplayed();
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        /// <summary>
        /// Returns true if there is at least one product in cart preview
        /// </summary>
        /// <returns></returns>
        public bool IsThereProductInCartPreview()
        {
            return CartPreviewProducts.Count > 0;
        }
    }
}
