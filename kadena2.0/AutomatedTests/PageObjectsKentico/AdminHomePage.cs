using AutomatedTests.Utilities;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedTests.PageObjectsKentico
{
    class AdminHomePage : BasePage
    {
        public AdminHomePage()
        {
            PageFactory.InitElements(Browser.Driver, this);
            Browser.Driver.SwitchTo().DefaultContent();
        }

        public void Open()
        {
            Browser.Driver.Navigate().GoToUrl($"{TestEnvironment.Url}/admin");
        }
    }
}
