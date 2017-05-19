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
    class KList : BasePage
    {
        [FindsBy(How = How.ClassName, Using = "content-header__btn")]
        private IWebElement AddMailingListBtn { get; set; }
        public KList()
        {
            PageFactory.InitElements(Browser.Driver, this);
        }

        public void Open()
        {
            Browser.Driver.Navigate().GoToUrl(TestEnvironment.Url + "/k-list");
        }

        public NewKList ClickAddMailingListBtn()
        {
            AddMailingListBtn.ClickElement();
            return new NewKList();
        }
    }
}
