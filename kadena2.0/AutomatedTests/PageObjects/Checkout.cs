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
using System.Threading;

namespace AutomatedTests.PageObjects
{
    class Checkout : BasePage
    {
        [FindsBy(How = How.CssSelector, Using = "#r-shopping-cart .shopping-cart__block:first-child label")]
        private IList<IWebElement> DeliveryAddresses { get; set; }

        [FindsBy(How = How.CssSelector, Using = "#r-shopping-cart .shopping-cart__block:nth-child(2) label")]
        private IList<IWebElement> DeliveryMethods { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".isActive ~ div .input__wrapper:not(.input__wrapper--disabled)")]
        private IWebElement DeliverableMethodForSelectedCarrier { get; set; }

        [FindsBy(How = How.ClassName, Using = "summary-table__amount")]
        private IList<IWebElement> SummaryTableTotals { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".r-spinner")]
        private IWebElement Spinner { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".alert .p-info")]
        private IWebElement ShoppingCartEmptyInfo { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".cart-product")]
        private IList<IWebElement> ProductsInCart { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".cart-product__action button")]
        private IList<IWebElement> RemoveProductButtons { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".input__wrapper>.input__wrapper>input[name=paymentMethod]")]
        private IWebElement PurchaseOrderInputField { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".shopping-cart__block>button")]
        private IWebElement SubmitOrderBtn { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".icon-shipping")]
        private IList<IWebElement> ShippingOptionsIcons { get; set; }

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
            WaitForLoading();
        }

        /// <summary>
        /// Iterates through Shipping options and returns true when it finds an option with estimated shipping price
        /// </summary>
        /// <returns></returns>
        public bool AreShippingCostEstimated()
        {
            WaitForShippingOptions();
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
            WaitForShippingOptions();
            for (int i = 0; i < DeliveryMethods.Count; i++)
            {
                if (DeliveryMethods[i].Text.Contains("$"))
                {
                    DeliveryMethods[i].ClickElement();

                    //clicks the method. In case it is not yet clickable, waits two seconds and does it again.
                    //TODO make it better, without the try catch
                    try
                    {
                        DeliverableMethodForSelectedCarrier.ClickElement();
                    }
                    catch (System.Reflection.TargetInvocationException)
                    {
                        Thread.Sleep(2000);
                        DeliverableMethodForSelectedCarrier.ClickElement();
                    }
                    break;
                }
            }
            WaitForLoading();
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

        /// <summary>
        /// Returns true if tax value is higher than 0
        /// </summary>
        /// <returns></returns>
        public bool IsTaxEstimated()
        {
            ParseSummaryTableValues();
            return tax > 0;
        }

        /// <summary>
        /// Empty the cart if it is not empty
        /// </summary>
        public void EmptyTheCart()
        {
            Browser.BaseWait.Until(r => ProductsInCart.Count > 0 || ShoppingCartEmptyInfo.IsPresent());

            if (ShoppingCartEmptyInfo.IsPresent())
            {
                return;
            }
            if (RemoveProductButtons.Count == 0)
            {
                throw new Exception("There are no remove buttons");
            }
            else
            {
                //click on each remove button and wait each time while the page is recalculated
                foreach (var item in RemoveProductButtons)
                {
                    item.Click();
                    WaitForLoading();
                }
            }
        }

        /// <summary>
        /// Fills out random number to Purchase Order field
        /// </summary>
        public void FillOutPurchaseOrderNumber()
        {
            PurchaseOrderInputField.EnterText(Lorem.RandomNumber(10000, 99999).ToString());
        }

        /// <summary>
        /// Clicks Place Order Button
        /// </summary>
        public void PlaceOrder()
        {
            SubmitOrderBtn.ClickElement();
        }

        /// <summary>
        /// Waits until there is at least one shipping icon
        /// </summary>
        public void WaitForShippingOptions()
        {
            Browser.BaseWait.Until(r => ShippingOptionsIcons.Count > 0);
            ShippingOptionsIcons[0].WaitTillClickable();
        }


    }
}