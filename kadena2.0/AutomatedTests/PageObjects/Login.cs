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
    class Login : BasePage
    {
        [FindsBy(How = How.Id, Using = "p_lt_ctl00_pageplaceholder_p_lt_WebPartZone1_zoneContent_LogonForm_Login1_UserName")]
        private IWebElement txtUserName { get; set; }

        [FindsBy(How = How.Id, Using = "p_lt_ctl00_pageplaceholder_p_lt_WebPartZone1_zoneContent_LogonForm_Login1_Password")]
        private IWebElement txtPassword { get; set; }

        [FindsBy(How = How.Id, Using = "p_lt_ctl00_pageplaceholder_p_lt_WebPartZone1_zoneContent_LogonForm_Login1_LoginButton")]
        private IWebElement btnSubmit { get; set; }

        public Login()
        {
            PageFactory.InitElements(Browser.Driver, this);
        }

        public void Open()
        {
            Browser.GoToUrl(TestEnvironment.Url);
        }

        public void FillLogin(string name, string pass)
        {
            txtUserName.EnterText(name);
            txtPassword.EnterText(pass);

            Log.WriteLine("User '{0}' filled into login", name);
        }

        public Dashboard Submit()
        {
            btnSubmit.ClickElement();
            return new Dashboard();
        }
    }
}
