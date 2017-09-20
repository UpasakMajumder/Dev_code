using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AutomatedTests
{
    /// <summary>
    /// Possibilities how to select element
    /// </summary>
    public enum SelectBy
    {
        /// <summary>
        /// InnerHTML
        /// </summary>
        Text,
        /// <summary>
        /// Value attribute
        /// </summary>
        Value,
        /// <summary>
        /// Index of option (starts with 0)
        /// </summary>
        Index
    }

    /// <summary>
    /// Basic javascript locators
    /// </summary>
    public enum JavaScriptLocator
    {
        /// <summary>
        /// Id attribute
        /// </summary>
        Id,
        /// <summary>
        /// Name attribute
        /// </summary>
        Name,
        /// <summary>
        /// Tag name
        /// </summary>
        Tag
    }

    /// <summary>
    /// Extension methods manipulating IWebElements and IList&lt;IWebElement&gt; 
    /// </summary>
    public static class SetMethods
    {
        /// <summary>
        /// Waits for element and than clears element it
        /// </summary>
        /// <param name="element">Inputs or other clearable elements</param>
        public static void ClearElement(this IWebElement element)
        {
            element.WaitTillVisible().Clear();
        }

        /// <summary>
        /// Waits for element, clears it and inputs desired text
        /// </summary>
        /// <param name="element">Inputs or other clearable elements</param>
        /// <param name="text">Text to be entered</param>
        public static void EnterText(this IWebElement element, string text)
        {
            element.WaitTillVisible().ClearElement();
            element.SendKeys(text);
        }

        /// <summary>
        /// Waits for element and inputs desired text
        /// </summary>
        /// <param name="element">Any HTML text</param>
        /// <param name="text">Text to be entered</param>
        public static void SendText(this IWebElement element, string text)
        {
            element.WaitTillVisible().SendKeys(text);
        }

        /// <summary>
        /// Waits for element and clicks on it
        /// </summary>
        /// <param name="element">Any HTML element</param>
        public static void ClickElement(this IWebElement element)
        {
            element.WaitTillClickable().Click();
        }

        /// <summary>
        /// Waits for element and clicks on it with offset
        /// </summary>
        /// <param name="element">Any HTML element</param>
        /// <param name="x">Amount of pixels to move by X axis</param>
        /// <param name="y">Amount of pixels to move by Y axis</param>
        public static void ClickElement(this IWebElement element, int x, int y)
        {
            element.WaitTillClickable();
            Browser.ActionsBuilder.MoveToElement(element, x, y).Click().Build().Perform();
        }

        /// <summary>
        /// Waits and clicks on one element in list
        /// </summary>
        /// <param name="elements">Any HTML elements</param>
        /// <param name="index">Index of element in list</param>
        public static void ClickElement(this IList<IWebElement> elements, string index)
        {
            elements[Convert.ToInt32(index)].WaitTillClickable().Click();
        }

        /// <summary>
        /// Waits and clicks on one element in list
        /// </summary>
        /// <param name="elements">Any HTML elements</param>
        /// <param name="index">Index of element in list</param>
        public static void ClickElement(this IList<IWebElement> elements, int index)
        {
            elements[index].ClickElement();
        }

        /// <summary>
        /// Clicks on random element in list
        /// </summary>
        /// <param name="elements">Any HTML elements</param>
        public static void ClickRandomElement(this IList<IWebElement> elements)
        {
            IWebElement element = elements[Browser.Random.Next(0, elements.Count - 1)];
            element.ClickElement();
        }

        /// <summary>
        /// Clicks on random element in list with maximum position 
        /// </summary>
        /// <param name="elements">Any HTML elements</param>
        /// <param name="max">Maximum position of element in list</param>
        public static void ClickRandomElement(this IList<IWebElement> elements, int max)
        {
            IWebElement element = elements[Browser.Random.Next(0, elements.Count - (1 + max))];
            element.WaitTillClickable().Click();
        }

        /// <summary>
        /// Clicks on random element in list with minimum and maximum position in list
        /// </summary>
        /// <param name="elements">Any HTML elements</param>
        /// <param name="min">Minimum position of element in list</param>
        /// <param name="max">Maximum position of element in list</param>
        public static void ClickRandomElement(this IList<IWebElement> elements, int min, int max)
        {
            IWebElement element = elements[Browser.Random.Next(min, max)];
            element.WaitTillClickable().Click();
        }

        /// <summary>
        /// Clicks on multiple random elements in list
        /// </summary>
        /// <param name="elements">Any HTML element</param>
        public static void ClickRandomElements(this IList<IWebElement> elements)
        {
            foreach (var item in elements)
            {
                if (Browser.Random.Next(0, 100) % 2 == 0)
                    item.ClickElement();
            }
        }

        /// <summary>
        /// Right clicks on given element
        /// </summary>
        /// <param name="element">Any HTML element</param>
        public static void RightClickElement(this IWebElement element)
        {
            element.WaitTillClickable();
            Browser.ActionsBuilder.ContextClick(element).Build().Perform();
        }

        /// <summary>
        /// Performs doubleclick 
        /// </summary>
        /// <param name="element">Any HTML element</param>
        public static void DoubleClickElement(this IWebElement element)
        {
            Browser.ActionsBuilder.DoubleClick(element).Build().Perform();
        }

        /// <summary>
        /// Selects random option in select box by index
        /// </summary>
        /// <param name="element">Select element</param>
        public static void SelectRandomElement(this IWebElement element)
        {
            element.WaitTillOptionsPresent();
            new SelectElement(element).SelectByIndex(Browser.Random.Next(1, element.FindElements(By.TagName("option")).Count - 1));
        }

        /// <summary>
        /// Selects random element in selectbox by index within limit in selectbox
        /// </summary>
        /// <param name="element">Select element</param>
        /// <param name="min">Minimum index of element</param>
        /// <param name="max">Maximum index of element</param>
        public static void SelectRandomElement(this IWebElement element, int min, int max)
        {
            element.WaitTillOptionsPresent();
            new SelectElement(element).SelectByIndex(Browser.Random.Next(min, max));
        }

        /// <summary>
        /// Selects element by given attribute and value
        /// </summary>
        /// <param name="element">Select element</param>
        /// <param name="by">What way you want to select the option</param>
        /// <param name="value">Value/index of option</param>
        public static void SelectElement(this IWebElement element, SelectBy by, string value)
        {
            element.WaitTillOptionsPresent();
            if (by == SelectBy.Text)
                new SelectElement(element).SelectByText(value);
            if (by == SelectBy.Value)
                new SelectElement(element).SelectByValue(value);
            if (by == SelectBy.Index)
                new SelectElement(element).SelectByIndex(Convert.ToInt32(value));
        }

        /// <summary>
        /// Select element by index
        /// </summary>
        /// <param name="element">Select element</param>
        /// <param name="index">Index of option</param>
        public static void SelectElement(this IWebElement element, int index)
        {
            element.WaitTillOptionsPresent();
            new SelectElement(element).SelectByIndex(index);
        }

        /// <summary>
        /// Clears element and enters short date within given limit
        /// </summary>
        /// <param name="element">Inputs or other clearable element</param>
        /// <param name="minDays">Minimum limit to change date</param>
        /// <param name="maxDays">Maximum limit to change date</param>
        public static void EnterRandomDate(this IWebElement element, int minDays, int maxDays)
        {
            element.EnterText(DateTime.Today.AddDays(Browser.Random.Next(minDays, maxDays)).ToShortDateString());
        }

        /// <summary>
        /// Enters date within given limit
        /// </summary>
        /// <param name="element">Inputs or other clearable element</param>
        /// <param name="minDays">Minimum limit to change date, if zero date can be today</param>
        /// <param name="maxDays">Maximum limit to change date</param>
        /// <param name="clear">Should be element cleared before entering the date?</param>
        public static void EnterRandomDate(this IWebElement element, int minDays, int maxDays, bool clear)
        {
            if (clear)
                element.EnterText(DateTime.Today.AddDays(Browser.Random.Next(minDays, maxDays)).ToShortDateString());
            else
                element.SendKeys(DateTime.Today.AddDays(Browser.Random.Next(minDays, maxDays)).ToShortDateString());
        }

        /// <summary>
        /// Enters path to .jpg or .png image in "/Testing files" directory
        /// This directory must be created manualy and some images must be present there
        /// </summary>
        /// <param name="element">Uploead file/image button</param>
        public static void EnterRandomImage(this IWebElement element)
        {
            var image = Path.GetFullPath(Directory.GetFiles(@"Testing Files").Where(r => r.Contains(".jpg") || r.Contains(".png")).OrderBy(i => Browser.Random.Next()).First());
            element.SendKeys(image);
        }

        /// <summary>
        /// Enters file on relative path
        /// </summary>
        /// <param name="element">Uploead file/image button</param>
        /// <param name="relativePath">Relative path of file</param>
        public static void EnterFile(this IWebElement element, string relativePath)
        {
            var file = Path.GetFullPath(relativePath);
            element.SendKeys(file);
        }

        /// <summary>
        /// Moves cursor to element, position X:0 Y:0
        /// </summary>
        /// <param name="element">Any HTML element</param>
        public static void MoveToMyElement(this IWebElement element)
        {
            element.WaitTillClickable();
            Browser.ActionsBuilder.MoveToElement(element).Perform();
        }

        /// <summary>
        /// Switches driver to this iframe
        /// </summary>
        /// <param name="element">Iframe element</param>
        /// <returns>Element of iframe</returns>
        public static IWebElement SwitchToIframe(this IWebElement element)
        {
            Browser.BaseWait.Until(r => element.IsDisplayed());
            Browser.Driver.SwitchTo().Frame(element);
            return element;
        }

        /// <summary>
        /// Switches driver to this iframe, but first can switch to default
        /// </summary>
        /// <param name="element">Iframe element</param>
        /// <param name="fromDefault">If true switches to default frame first</param>
        /// <returns>Element if iframe</returns>
        public static IWebElement SwitchToIframe(this IWebElement element, bool fromDefault)
        {
            if (fromDefault)
            {
                Browser.Driver.SwitchTo().DefaultContent();
                Browser.Driver.SwitchTo().Frame(element);
            }
            else
                Browser.Driver.SwitchTo().Frame(element);

            return element;
        }

        /// <summary>
        /// Waits for given time
        /// </summary>
        /// <param name="element">Any HTML element</param>
        /// <param name="seconds">Number of seconds System should wait</param>
        /// <returns>Given element</returns>
        public static IWebElement PlainWait(this IWebElement element, int seconds)
        {
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(seconds));
            return element;
        }

        /// <summary>
        /// Waits for browser alert to be displayed
        /// </summary>
        /// <param name="alert">Any HTML element</param>
        /// <returns>Instance of the alert</returns>
        public static IAlert WaitForAlert(this IAlert alert)
        {
            Browser.BaseWait.Until(ExpectedConditions.AlertIsPresent());
            return alert;
        }

        /// <summary>
        /// Waits till the element is clickable
        /// </summary>
        /// <param name="element">Any HTML element</param>
        /// <returns>Given element</returns>
        public static IWebElement WaitTillClickable(this IWebElement element)
        {
            Browser.BaseWait.Until(ExpectedConditions.ElementToBeClickable(element));
            return element;
        }

        /// <summary>
        /// Waits till element disapeares from DOM
        /// </summary>
        /// <param name="element">Any HTML element</param>
        /// <returns>Given element</returns>
        public static IWebElement WaitTillNotPresent(this IWebElement element)
        {
            Browser.BaseWait.Until((d) => !element.IsPresent());
            return element;
        }
        
        /// <summary>
        /// Waits till element is displayed
        /// </summary>
        /// <param name="element">Any HTML element</param>
        /// <returns>Given element</returns>
        public static IWebElement WaitTillVisible(this IWebElement element)
        {
            Browser.BaseWait.Until((d) => element.IsDisplayed());
            return element;
        }

        /// <summary>
        /// Waits till element is not displayed
        /// </summary>
        /// <param name="element">Any HTML element</param>
        /// <returns>Given element</returns>
        public static IWebElement WaitTillNotVisible(this IWebElement element)
        {
            Browser.BaseWait.Until((d) => !element.IsDisplayed());
            return element;
        }

        /// <summary>
        /// Waits there are more then one option in selectbox
        /// </summary>
        /// <param name="element">Any HTML element wraping these options, usually select element</param>
        /// <returns>Given element</returns>
        public static IWebElement WaitTillOptionsPresent(this IWebElement element)
        {
            Browser.BaseWait.Until((d) => element.FindElements(By.TagName("option")).Count > 1);
            return element;
        }

        /// <summary>
        /// Waits till at least one element in list 
        /// </summary>
        /// <param name="elements">Any HTML element</param>
        /// <returns>Given element</returns>
        public static IList<IWebElement> WaitTillListItemsPresent(this IList<IWebElement> elements)
        {
            Browser.BaseWait.Until((d) => elements.Count > 0);
            return elements;
        }

        /// <summary>
        /// Sccrolls view to given element
        /// </summary>
        /// <param name="element">Any HTML element</param>
        /// <returns>Given element</returns>
        public static IWebElement ScrollElementToView(this IWebElement element)
        {
            string js = string.Format("window.scrollTo({0}, {1})", element.Location.X, element.Location.Y);
            Browser.JsExecutor.ExecuteScript(js);

            return element;
        }

        /// <summary>
        /// Scrolls browser view to given element with offset
        /// </summary>
        /// <param name="element">Any HTML element</param>
        /// <param name="offset">Offset of pixels from top on Y axis</param>
        /// <returns>Given element</returns>
        public static IWebElement ScrollElementToView(this IWebElement element, int offset)
        {
            string js = string.Format("window.scrollTo({0}, {1})", element.Location.X, element.Location.Y - offset);
            Browser.JsExecutor.ExecuteScript(js);

            return element;
        }

        /// <summary>
        /// Changes attribute of given element based on "JavaScriptLocator"
        /// Locator must be unique or first in DOM
        /// </summary>
        /// <param name="element">Any HTML element</param>
        /// <param name="locator">Locator available in JS</param>
        /// <param name="attribute">Name of attribute</param>
        /// <param name="value">Desired value</param>
        /// <returns>Given element</returns>
        public static IWebElement SetAttribute(this IWebElement element, JavaScriptLocator locator, string attribute, string value)
        {

            string js = string.Empty;

            switch (locator)
            {
                case JavaScriptLocator.Id:
                    js = string.Format("document.getElementById('{0}').setAttribute('{1}', '{2}')", element.GetAttribute("id"), attribute, value);
                    break;
                case JavaScriptLocator.Name:
                    js = string.Format("document.getElementsByName('{0}').setAttribute('{1}', '{2}')", element.GetAttribute("name"), attribute, value);
                    break;
                case JavaScriptLocator.Tag:
                    js = string.Format("document.getElementsByTagName('{0}').setAttribute('{1}', '{2}')", element.GetElementTag(), attribute, value);
                    break;
            }

            Browser.JsExecutor.ExecuteScript(js);

            return element;
        }
    }
}
