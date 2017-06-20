using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedTests.PageObjects
{
    class FormSuccess : BasePage
    {
        [FindsBy(How = How.CssSelector, Using = ".submitted")]
        private IWebElement SubmitConfirmation { get; set; }

        public FormSuccess()
        {
            PageFactory.InitElements(Browser.Driver, this);
        }

        public bool IsSubmitConfirmationDisplayed()
        {
            SubmitConfirmation.WaitTillVisible();
            return SubmitConfirmation.IsDisplayed();
        }
    }
}
