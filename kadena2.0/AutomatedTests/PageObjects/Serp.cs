using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedTests.PageObjects
{
    class Serp : BasePage
    {
        [FindsBy(How = How.CssSelector, Using = ".row>div>a")]
        private IList<IWebElement> ProductsResults { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".row")]
        private IList<IWebElement> ResultsRows { get; set; }

        public Serp()
        {
            PageFactory.InitElements(Browser.Driver, this);
        }

        /// <summary>
        /// Returns true if there is at least 1 result
        /// </summary>
        /// <returns></returns>
        public bool AreThereResults()
        {
            WaitForResults();
            return ProductsResults.Count > 0;
        }

        /// <summary>
        /// Waits for at least one row with results
        /// </summary>
        public void WaitForResults()
        {
            Browser.BaseWait.Until(r => ResultsRows.Count > 0);
        }
    }
}
