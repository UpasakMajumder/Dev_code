using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedTests.PageObjects
{
    class ListDetail : BasePage
    {
        [FindsBy(How = How.ClassName, Using = "processed-list__table-heading--error")]
        private IWebElement BadAddressesComponent { get; set; }

        [FindsBy(How = How.Id, Using = "p_lt_WebPartZone3_zoneContent_pageplaceholder_p_lt_WebPartZone2_zoneContent_Kadena_AddressViewer_btnUseOnlyGoodAddresses")]
        private IWebElement UseOnlyCorrectAddressesBtn { get; set; }

        public ListDetail()
        {
            PageFactory.InitElements(Browser.Driver, this);
        }

        /// <summary>
        /// Verifies if component for bad addresses is displayed
        /// </summary>
        /// <returns></returns>
        public bool AreThereBadAddresses()
        {
            try
            {
                BadAddressesComponent.WaitTillVisible();
            }
            catch
            {
                return false;
            }
            return BadAddressesComponent.IsDisplayed();
        }

        /// <summary>
        /// Clicks button use only correct addresses
        /// </summary>
        public void UseOnlyCorrectAddresses()
        {
            UseOnlyCorrectAddressesBtn.ClickElement();
        }
    }
}
