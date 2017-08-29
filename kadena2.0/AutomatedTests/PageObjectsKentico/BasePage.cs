using AutomatedTests.PageObjectsKentico.BasePageComponents;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedTests.PageObjectsKentico
{
    class BasePage
    {
        [FindsBy(How=How.Id, Using = "cms-applist-toggle")]
        private IWebElement ApplistIcon { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".icon-user")]
        private IWebElement UserMenuIcon { get; set; }

        [FindsBy(How = How.Id, Using = "m_c_layoutElem_h_Header_userMenu_lnkSignOut")]
        private IWebElement SignOutBtn { get; set; }

        public ApplicationsMenu applicationsMenu;

        public BasePage()
        {
            this.applicationsMenu = new ApplicationsMenu();
            PageFactory.InitElements(Browser.Driver, this);
        }
        
        /// <summary>
        /// Clicks applist icon to display list of applications
        /// </summary>
        public void ClickApplistIcon()
        {
            ApplistIcon.ClickElement();
        }

        public void LogoutFromKentico()
        {
            Browser.Driver.SwitchTo().DefaultContent();
            UserMenuIcon.ClickElement();
            SignOutBtn.ClickElement();
        }

    }
}
