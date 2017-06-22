using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public BasePage()
        {
            PageFactory.InitElements(Browser.Driver, this);
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

    }
}
