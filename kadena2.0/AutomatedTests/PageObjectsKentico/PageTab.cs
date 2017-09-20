using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedTests.PageObjectsKentico
{
    class PageTab : BasePage
    {
        [FindsBy(How = How.Id, Using = "m_c_plc_lt_ctl00_HorizontalTabs_l_c")]
        private IWebElement PageTabFrame { get; set; }

        [FindsBy(How = How.Id, Using = "m_c_layoutElem_contentview")]
        private IWebElement ContentViewFrame { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".cms-edit-menu")]
        private IWebElement CmsEditMenu { get; set; }

        public PageTab()
        {
            PageFactory.InitElements(Browser.Driver, this);
            ContentViewFrame.SwitchToIframe();
            PageTabFrame.SwitchToIframe();
            CmsEditMenu.WaitTillVisible();
        }
    }
}
