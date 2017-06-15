using AutomatedTests.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedTests.PageObjects
{
    class Settings : BasePage
    {
        [FindsBy(How = How.ClassName, Using = "settings")]
        private IWebElement SettingsBlock { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".j-contant-person-form input")]
        private IList<IWebElement> ContactPersonFormFields { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".j-contant-person-form .j-submit-button")]
        private IWebElement SaveChangesButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".css-tabs__list>li")]
        private IList<IWebElement> TabsButtons { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".j-password-change-form input")]
        private IList<IWebElement> PasswordFormFields { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".j-password-change-form .j-submit-button")]
        private IWebElement ChangePasswordButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".j-general-error-label")]
        private IWebElement PasswordError { get; set; }

        public enum Tabs
        {
            MyAccount = 0,
            Password = 1,
            EmailNotification = 2,
            Addresses = 3
        }

        public Settings()
        {
            PageFactory.InitElements(Browser.Driver, this);
        }

        public void Open()
        {
            Browser.Driver.Navigate().GoToUrl($"{TestEnvironment.Url}/settings");
        }

        public void WaitUntilSettingsBlockIsDisplayed()
        {
            SettingsBlock.WaitTillVisible();
        }

        /// <summary>
        /// Fills out form for user profile. Enters strings into first two fields and integers in the other two
        /// </summary>
        public void FillOutForm()
        {

            for (int i = 0; i < 4; i++)
            {
                if (i < 2)
                {
                    //edit first name and last name
                    ContactPersonFormFields[i].EnterText(StringHelper.RandomString(5));
                }
                else
                {
                    //edit phone numbers
                    ContactPersonFormFields[i].EnterText(Lorem.RandomNumber(10000, 99999).ToString());
                }
            }
        }

        public void ClickSaveChangesButton()
        {
            SaveChangesButton.ClickElement();
        }

        /// <summary>
        /// Refreshes the page and checks if fields contain empty string. 
        /// </summary>
        /// <returns>True if no field is String.Empty</returns>
        public bool AreValuesInFormFilledOut()
        {
            Browser.Refresh();
            foreach (var item in ContactPersonFormFields)
            {
                if (item.GetText() == String.Empty)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Selects tab you specify
        /// </summary>
        /// <param name="tab"></param>
        public void SelectTab(Tabs tab)
        {
            if (TabsButtons.Count == 0)
            {
                throw new Exception("There are no tabs on the settings page");
            }

            TabsButtons[(int)tab].ClickElement();
        }

        /// <summary>
        /// Fill out the form and click change password button
        /// </summary>
        /// <param name="oldPassword">Current password of the logged-in user</param>
        public void SubmitNotStrongPassword(string oldPassword)
        {
            if (PasswordFormFields.Count == 0)
            {
                throw new Exception("There are no fields");
            }

            //validation verifies if there are are at least 8 characters
            string newPassword = StringHelper.RandomString(7);

            for (int i = 0; i < PasswordFormFields.Count; i++)
            {
                if (i == 0)
                {
                    PasswordFormFields[i].EnterText(oldPassword);
                }
                else
                {
                    PasswordFormFields[i].EnterText(newPassword);
                }

            }

            ChangePasswordButton.ClickElement();
        }

        public bool IsPasswordErrorDisplayed()
        {
            PasswordError.WaitTillVisible();
            return PasswordError.IsDisplayed();
        }
    }
}
