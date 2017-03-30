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

        public BasePage()
        {
            PageFactory.InitElements(Browser.Driver, this);
        }

    }
}
