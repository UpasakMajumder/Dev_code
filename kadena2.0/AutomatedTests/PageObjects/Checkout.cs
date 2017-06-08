using AutomatedTests.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace AutomatedTests.PageObjects
{
    class Checkout
    {
        [FindsBy(How = How.CssSelector, Using = "#r-shopping-cart .shopping-cart__block:first-child label")]
        private IList<IWebElement> DeliveryAddresses { get; set; }
        [FindsBy(How = How.CssSelector, Using = "#r-shopping-cart .shopping-cart__block:nth-child(2) label")]
        private IList<IWebElement> DeliveryMethods { get; set; }
        [FindsBy(How = How.CssSelector, Using = ".isActive ~ div .input__wrapper:not(.input__wrapper--disabled)")]
        private IWebElement DeliverableMethodForSelectedCarrier { get; set; }
        [FindsBy(How = How.ClassName, Using = "summary-table__amount")]
        private IList<IWebElement> SummaryTableTotals { get; set; }

        private decimal summary;
        private decimal shipping;
        private decimal tax;
        private decimal subtotal;
        private decimal total;
        public Checkout()
        {
            PageFactory.InitElements(Browser.Driver, this);
        }

        /// <summary>
        /// Navigates to checkout page
        /// </summary>
        public void Open()
        {
            Browser.Driver.Navigate().GoToUrl(TestEnvironment.Url + "/checkout");
        }

        /// <summary>
        /// Select one of the available delivery addresses by index.
        /// </summary>
        /// <param name="index"></param>
        public void SelectAddress(int index)
        {
            Browser.BaseWait.Until(r => DeliveryAddresses.Count > 0);
            DeliveryAddresses[index].ClickElement();
        }

        /// <summary>
        /// Iterates through Shipping options and returns true when it finds an option with estimated shipping price
        /// </summary>
        /// <returns></returns>
        public bool AreShippingCostEstimated()
        {
            if (DeliveryAddresses.Count == 0)
            {
                return false;
            }

            for (int i = 0; i < DeliveryMethods.Count; i++)
            {
                if (DeliveryMethods[i].Text.Contains("$"))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Iterates through shipping methods, finds one with estimated price and then clicks on the valid nested option
        /// </summary>
        public void SelectEstimatedCarrier()
        {
            for (int i = 0; i < DeliveryMethods.Count; i++)
            {
                if (DeliveryMethods[i].Text.Contains("$"))
                {
                    DeliveryMethods[i].ClickElement();
                    DeliverableMethodForSelectedCarrier.ClickElement();
                    break;
                }
            }
        }

        /// <summary>
        /// Parses strings from table to decimal variables
        /// </summary>
        private void ParseSummaryTableValues()
        {
            summary = decimal.Parse(SummaryTableTotals[0].GetText().Substring(2));
            shipping = decimal.Parse(SummaryTableTotals[1].GetText().Substring(2));
            subtotal = decimal.Parse(SummaryTableTotals[2].GetText().Substring(2));
            tax = decimal.Parse(SummaryTableTotals[3].GetText().Substring(2));
            total = decimal.Parse(SummaryTableTotals[4].GetText().Substring(2));
        }

        /// <summary>
        /// Returns true if subtotal equals sum of summary and shipping
        /// </summary>
        /// <returns></returns>
        public bool IsSubTotalCorrect()
        {
            ParseSummaryTableValues();
            if (shipping == 0)
            {
                throw new Exception("Shipping equals 0");
            }
            
            return subtotal == summary + shipping;
        }

        /// <summary>
        /// Returns true if subtotal equals sum of summary, shipping and tax
        /// </summary>
        /// <returns></returns>
        public bool isTotalCorrect()
        {
            ParseSummaryTableValues();
            return total == summary + shipping + tax;
        }


    }


}
