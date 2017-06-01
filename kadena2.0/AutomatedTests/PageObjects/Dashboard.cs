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

        
        public Dashboard()
        {
            PageFactory.InitElements(Browser.Driver, this);
            
        }

        public bool IsDashboardDisplayed()
        {
            return dashboard.IsDisplayed();
        }

    }
}
