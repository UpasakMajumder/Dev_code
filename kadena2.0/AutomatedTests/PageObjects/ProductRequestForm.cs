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
    class ProductRequestForm : BasePage
    {
        [FindsBy(How = How.ClassName, Using = "j-description-input")]
        private IWebElement Description { get; set; }

        [FindsBy(How = How.ClassName, Using = "js-drop-zone-file")]
        private IWebElement DropZone { get; set; }

        [FindsBy(How = How.ClassName, Using = "j-submit-button")]
        private IWebElement SubmitBtn { get; set; }

        [FindsBy(How = How.ClassName, Using = "j-description-error-message")]
        private IWebElement DescriptionValidationError { get; set; }

        public ProductRequestForm()
        {
            PageFactory.InitElements(Browser.Driver, this);
        }

        /// <summary>
        /// Enters test message into description field
        /// </summary>
        public void FillOutDescription()
        {
            Description.SendText(Lorem.ThisIsTestMessage());
        }

        /// <summary>
        /// Sends filepath to test file to the input field
        /// </summary>
        public void SelectAttachment()
        {
            string path = TestEnvironment.TestPath + "\\TestFiles\\testpicture.jpg";
            DropZone.SendKeys(path);
        }

        /// <summary>
        /// Clicks Submit button
        /// </summary>
        public void SubmitForm()
        {
            SubmitBtn.ClickElement();
        }
            
        public bool IsDescriptionValidationDisplayed()
        {
            DescriptionValidationError.WaitTillVisible();
            return DescriptionValidationError.IsDisplayed();
        }

    }
}
