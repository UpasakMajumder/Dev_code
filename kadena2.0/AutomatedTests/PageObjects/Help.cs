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
    class ContactUs : BasePage
    {
        [FindsBy(How = How.CssSelector, Using = ".contact__submit button")]
        private IWebElement SubmitButton { get; set; }
        [FindsBy(How = How.CssSelector, Using = ".input__error")]
        private IList<IWebElement> ValidationErrors { get; set; }
        [FindsBy(How = How.CssSelector, Using = ".contact__form input")]
        private IList<IWebElement> ContactFormInputFields { get; set; }
        [FindsBy(How = How.TagName, Using = "textarea")]
        private IWebElement ContactFormTextArea { get; set; }

        public ContactUs()
        {
            PageFactory.InitElements(Browser.Driver, this);
        }

        public void Open()
        {
            Browser.Driver.Navigate().GoToUrl($"{TestEnvironment.Url}/help/contact-us");
        }

        public void ClickSubmitButton()
        {
            SubmitButton.ClickElement();
        }

        /// <summary>
        /// Checks if any of the validations are displayed
        /// </summary>
        /// <returns></returns>
        public bool IsValidationErrorDisplayed()
        {
            IList<IWebElement> displayedErrors = ValidationErrors.Where(r => r.IsDisplayed()).ToList();
            return displayedErrors.Count() > 0;
        }

        /// <summary>
        /// Fills out the form
        /// </summary>
        public void FillOutTheContactForm()
        {
            for (int i = 0; i < ContactFormInputFields.Count; i++)
            {
                if (ContactFormInputFields[i].GetText().Contains("mail"))
                {
                    ContactFormInputFields[i].EnterText(TestUser.Name);
                }
                else if (ContactFormInputFields[i].GetText().Contains("phone"))
                {
                    ContactFormInputFields[i].EnterText(Lorem.RandomNumber(10000, 99999).ToString());
                }
                else
                {
                    ContactFormInputFields[i].EnterText(StringHelper.RandomString(5));
                }
            }

            ContactFormTextArea.EnterText(StringHelper.RandomString(5));
        }
    }
}
