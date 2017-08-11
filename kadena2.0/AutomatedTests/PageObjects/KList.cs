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

        [FindsBy(How = How.CssSelector, Using = ".mailing-lists tr:nth-child(2) td")]
        private IList<IWebElement> MailingListsFirstRowColumns { get; set; }

        public int NumberOfErrors
        {
            get
            {
                return int.Parse(MailingListsFirstRowColumns[3].GetText());
            }            
        }

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
            return MailingListsFirstRowColumns[0].GetText().Contains(name);
        }

        /// <summary>
        /// Checks the number of errors in the first row until it is higher than 0
        /// </summary>
        /// <returns></returns>
        public bool WereAddressesValidated()
        {
            for (int i = 0; i < 25; i++)
            {
                if (AreThereAnyErrorsInFirstList())
                {
                    return true;
                }

                //sleep for ten seconds and refresh browser
                Thread.Sleep(10000);
                Browser.Refresh();
            }
            return false;
        }

        /// <summary>
        /// Returns true if number of errors is higher than 0
        /// </summary>
        /// <returns></returns>
        public bool AreThereAnyErrorsInFirstList()
        {
            int numberOfErrors;
            if (!int.TryParse(MailingListsFirstRowColumns[3].GetText(), out numberOfErrors))
            {
                return false;
            }
            return numberOfErrors > 0;           
        }

        /// <summary>
        /// Clicks on view button on first list
        /// </summary>
        /// <returns></returns>
        public ListDetail OpenFirstList()
        {
            MailingListsFirstRowColumns[4].FindElement(By.CssSelector("a")).ClickElement();
            return new ListDetail();
        }
    }
}

