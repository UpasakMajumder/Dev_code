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
    class ListDetail : BasePage
    {
        [FindsBy(How = How.ClassName, Using = "processed-list__table-heading--error")]
        private IWebElement BadAddressesComponent { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".processed-list__table-block:nth-child(2) button")]
        private IWebElement UseOnlyCorrectAddressesBtn { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".processed-list__table-block:nth-child(1) button")]
        private IWebElement CorrectErrorsBtn { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".processed-list__table-dialog tr:nth-child(2) td input,select")]
        private IList<IWebElement> CorrectAddressFirstRowColumns { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".dialog__footer button:nth-child(2)")]
        private IWebElement ConfirmChangedAddresses { get; set; }

        public ListDetail()
        {
            PageFactory.InitElements(Browser.Driver, this);
        }

        /// <summary>
        /// Verifies if component for bad addresses is displayed
        /// </summary>
        /// <returns></returns>
        public bool AreThereBadAddresses()
        {
            try
            {
                BadAddressesComponent.WaitTillVisible();
            }
            catch
            {
                return false;
            }
            return BadAddressesComponent.IsDisplayed();
        }

        /// <summary>
        /// Clicks button use only correct addresses
        /// </summary>
        public void UseOnlyCorrectAddresses()
        {
            UseOnlyCorrectAddressesBtn.ClickElement();
            WaitForLoading();
        }

        /// <summary>
        /// Clicks correct errors button
        /// </summary>
        public void CorrectErrors()
        {
            CorrectErrorsBtn.ClickElement();
        }

        /// <summary>
        /// Enters new (correct) address to the first row and saves changes
        /// </summary>
        public void CorrectFirstRow()
        {
            var address = Lorem.RandomUSAddress();
            CorrectAddressFirstRowColumns[0].EnterText(address.Name);
            CorrectAddressFirstRowColumns[1].EnterText(address.AddressLine1);
            CorrectAddressFirstRowColumns[3].EnterText(address.City);
            CorrectAddressFirstRowColumns[4].SelectElement(SelectBy.Text, address.State);
            CorrectAddressFirstRowColumns[5].EnterText(address.Zip);

            ConfirmChangedAddresses.ClickElement();
            WaitForLoading();
        }
    }
}
