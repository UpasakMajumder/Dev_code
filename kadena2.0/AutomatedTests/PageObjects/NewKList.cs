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
        [FindsBy(How = How.CssSelector, Using = ".drop-zone input")]
        private IWebElement DropZone { get; set; }

        [FindsBy(How = How.Id, Using = "p_lt_WebPartZone3_zoneContent_pageplaceholder_p_lt_WebPartZone2_zoneContent_MailingListUploader_btnSubmit")]
        private IWebElement CreateMailingListBtn { get; set; }
        public NewKList()
        {
            PageFactory.InitElements(Browser.Driver, this);
        }

        /// <summary>
        /// Sends csv location path to the input field
        /// </summary>
        public void SelectMailingList()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            DropZone.SendKeys(path.Substring(6) + "\\TestFiles\\testcsv.csv");
        }

        public MapColumns SubmitMailingList()
        {
            CreateMailingListBtn.ClickElement();
            return new MapColumns();
        }
    }
}
