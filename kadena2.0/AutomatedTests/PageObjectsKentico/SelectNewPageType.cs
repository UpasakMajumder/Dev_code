using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutomatedTests.PageObjectsKentico
{
    class SelectNewPageType : BasePage
    {
        [FindsBy(How = How.Id, Using = "m_c_layoutElem_contentview")]
        private IWebElement ContentViewFrame { get; set; }

        [FindsBy(How = How.CssSelector, Using = "#m_c_dt_gridClasses_v a")]
        private IList<IWebElement> PageTypes { get; set; }

        public enum PageTypeOptions
        {
            Product = 0,
            ProductCategory = 1,
            Folder = 2
        }

        public SelectNewPageType()
        {
            PageFactory.InitElements(Browser.Driver, this);
        }

        public void SwitchToContentViewFrame()
        {
            ContentViewFrame.SwitchToIframe();
        }

        /// <summary>
        /// Click one of the page types
        /// </summary>
        /// <param name="pageType"></param>
        public void SelectPageType(PageTypeOptions pageType)
        {
            PageTypes.WaitTillListItemsPresent();
            //this loop added to avoid stale element reference exception or no such element
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    PageTypes[(int)pageType].ClickElement();
                    break;
                }
                catch
                {
                    Thread.Sleep(500);
                }
            }
        }

        /// <summary>
        /// Returns true if there are more than 0 page types
        /// </summary>
        /// <returns></returns>
        public bool AreThereAnyPageTypes()
        {
            SwitchToContentViewFrame();
            return PageTypes.Count > 0;
        }
    }
}
