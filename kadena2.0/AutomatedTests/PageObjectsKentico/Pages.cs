using AutomatedTests.PageObjectsKentico;
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
    class PagesContentTree : BasePage
    {
        [FindsBy(How = How.Id, Using = "m_c_layoutElem_cmsdesktop")]
        private IWebElement EditorFrame { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".TreeContextMenu")]
        private IWebElement ContextMenu { get; set; }

        public PagesContentTree()
        {
            EditorFrame.SwitchToIframe();
            PageFactory.InitElements(Browser.Driver, this);
        }

        private IWebElement EditorFrame1()
        {
            return Browser.Driver.FindElement(By.Id("m_c_layoutElem_cmsdesktop"));

        }
        /// <summary>
        /// Right click item from content tree
        /// </summary>
        /// <param name="itemName">Literal name of item as it is in Kentico</param>
        public void RightClickItem(string itemName)
        {
            Browser.Driver.SwitchTo().DefaultContent();
            EditorFrame1().SwitchToIframe();
            Browser.Driver.FindElement(By.XPath("//span[text() = '" + itemName + "']")).RightClickElement();
        }

        /// <summary>
        /// Clicks on item in context menu
        /// </summary>
        /// <param name="action">Literal name of action as it is in Kentico</param>
        public void SelectActionFromContextMenu(string action, string itemName)
        {
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    RightClickItem(itemName);
                    Browser.Driver.FindElement(By.XPath("//span[text() = '" + action + "']")).ClickElement();

                    //check if click was successful and page types are displayed
                    SelectNewPageType selectNewPageType = new SelectNewPageType();
                    if (selectNewPageType.AreThereAnyPageTypes())
                    {
                        break;
                    }
                }
                catch
                {
                    Thread.Sleep(500);
                }
            }

        }
    }
}
