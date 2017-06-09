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

        public Settings()
        {
            PageFactory.InitElements(Browser.Driver, this);
        }

        public void Open()
        {
            Browser.Driver.Navigate().GoToUrl(TestEnvironment.Url + "/settings");
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


    }
}
