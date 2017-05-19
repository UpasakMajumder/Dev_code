using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedTests.PageObjects
{
    class ListProcessing : BasePage
    {
        [FindsBy(How = How.ClassName, Using = "submitted")]
        private IWebElement SubmitConfirmation { get; set; }
        public ListProcessing()
        {
            PageFactory.InitElements(Browser.Driver, this);
        }

        public bool WasMailingListSubmitted()
        {
            return SubmitConfirmation.Displayed;
        }
    }
}
