using AutomatedTests.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutomatedTests.PageObjects
{
    class KList : BasePage
    {
        [FindsBy(How = How.ClassName, Using = "content-header__btn")]
        private IWebElement AddMailingListBtn { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".mailing-lists tr")]
        private IList<IWebElement> MailingListsTableRows { get; set; }

        public KList()
        {
            PageFactory.InitElements(Browser.Driver, this);
        }

        public void Open()
        {
            Browser.Driver.Navigate().GoToUrl(TestEnvironment.Url + "/k-list");
        }

        public NewKList ClickAddMailingListBtn()
        {
            AddMailingListBtn.ClickElement();
            return new NewKList();
        }

        /// <summary>
        /// Verifies if the first line on the page contains the name of mailing list you provide
        /// </summary>
        /// <param name="name">name of mailing list</param>
        /// <returns></returns>
        public bool IsMailingListOnThePage(string name)
        {           
            return MailingListsTableRows[1].GetText().Contains(name);
        }

        /// <summary>
        /// Checks the number of errors in the first row until it is higher than 0
        /// </summary>
        /// <returns></returns>
        public bool WereAddressesValidated()
        {
            for (int i = 0; i < 20; i++)
            {
                IList<IWebElement> firstRowColumns = MailingListsTableRows[1].FindElements(By.CssSelector("td"));
                int numberOfErrors = int.Parse(firstRowColumns[3].GetText());
                
                if (numberOfErrors > 0)
                {
                    return true;
                }

                //sleep for ten seconds and refresh browser
                Thread.Sleep(10000);
                Browser.Refresh();
            }
            return false;
        }
    }
}

