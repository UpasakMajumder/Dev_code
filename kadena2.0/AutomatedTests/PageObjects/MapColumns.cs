using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedTests.PageObjects
{
    class MapColumns : BasePage
    {
        [FindsBy(How = How.CssSelector, Using = ".btn-group>button:nth-child(2)")]
        private IWebElement ProcessMyListBtn { get; set; }

        public MapColumns()
        {
            PageFactory.InitElements(Browser.Driver, this);
        }

        public ListProcessing ClickProcessList()
        {
            ProcessMyListBtn.ClickElement();
            return new ListProcessing();
        }

    }
}
