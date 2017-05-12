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
        [FindsBy(How = How.ClassName, Using = "input__text ")]
        private IWebElement txtUserName { get; set; }

        [FindsBy(How = How.ClassName, Using = "input__password ")]
        private IWebElement txtPassword { get; set; }

        [FindsBy(How = How.ClassName, Using = "login__login-button")]
        private IWebElement btnSubmit { get; set; }

        [FindsBy(How = How.ClassName, Using = "input__toggler")]
        private IWebElement inputToggler { get; set; }

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

        /// <summary>
        /// Verify if password is shown as asterisks. Note: Password is shown as asterisks if type attribute equals "password"
        /// </summary>
        /// <returns></returns>
        public bool IsPasswordShownAsAsterisks()
        {
            string type = txtPassword.GetAttribute("type");
            return (type == "password");            
        }

        public void ClickInputToggler()
        {
            inputToggler.ClickElement();
        }
    }
}
