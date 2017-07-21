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
    class KSource : BasePage
    {
        [FindsBy(How = How.CssSelector, Using = ".show-table .active")]
        private IList<IWebElement> DisplayedTableRows { get; set; }
        public KSource()
        {
            PageFactory.InitElements(Browser.Driver, this);
        }

        public void Open()
        {
            Browser.Driver.Navigate().GoToUrl($"{TestEnvironment.Url}/k-source");
        }

        /// <summary>
        /// Returns true if there is more than 1 project
        /// </summary>
        /// <returns></returns>
        public bool AreThereAnyProjects()
        {
            WaitForKadenaPageLoad();
            return DisplayedTableRows.Count > 0;
        }
    }
}
