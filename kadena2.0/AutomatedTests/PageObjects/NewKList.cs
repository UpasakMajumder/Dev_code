using AutomatedTests.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedTests.PageObjects
{
    class NewKList : BasePage
    {
        [FindsBy(How = How.CssSelector, Using = ".js-drop-zone-file")]
        private IWebElement DropZone { get; set; }

        [FindsBy(How = How.Id, Using = "p_lt_WebPartZone3_zoneContent_pageplaceholder_p_lt_WebPartZone2_zoneContent_MailingListUploader_btnSubmit")]
        private IWebElement CreateMailingListBtn { get; set; }

        [FindsBy(How = How.ClassName, Using = "js-drop-zone-name-input")]
        private IWebElement NameField { get; set; }

        public NewKList()
        {
            PageFactory.InitElements(Browser.Driver, this);
        }

        /// <summary>
        /// Sends csv location path to the input field
        /// </summary>
        public void SelectMailingList()
        {
            string path = TestEnvironment.TestPath + "\\TestFiles\\testcsv.csv";
            DropZone.SendKeys(path);
        }

        public MapColumns SubmitMailingList()
        {
            CreateMailingListBtn.ClickElement();
            return new MapColumns();
        }

        /// <summary>
        /// Enters name from argument
        /// </summary>
        /// <param name="name"></param>
        public void FillOutMailingListName(string name)
        {
            NameField.EnterText(name);
        }
    }
}
