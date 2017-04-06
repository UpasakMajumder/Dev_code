using AutomatedTests.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Events;
using OpenQA.Selenium.Support.UI;
using System;

namespace AutomatedTests
{
    /// <summary>
    /// Everything connected with driver
    /// </summary>
    public class Browser
    {
        /// <summary>
        /// Browser window
        /// </summary>
        public static IWebDriver Driver { get; set; }
        /// <summary>
        /// Event firing extention of driver
        /// </summary>
        private static EventFiringWebDriver Edr { get; set; }
        /// <summary>
        /// Allows actions in driver
        /// </summary>
        public static Actions ActionsBuilder { get; set; }
        /// <summary>
        /// Wait for driver actions
        /// </summary>
        public static WebDriverWait BaseWait { get; set; }
        /// <summary>
        /// Represents a pseudo-random number generator, a device that produces a sequence of numbers that meet certain statistical requirements for randomness.
        /// </summary>
        public static Random Random { get; set; }
        /// <summary>
        /// JavaScript Executor 
        /// </summary>
        public static IJavaScriptExecutor JsExecutor { get; set; }

        /// <summary>
        /// Creates Driver (Chrome by default) and Event Firing Driver, creates rules for exceptions and events, wait set to 20s by default
        /// Also populates wait and builder for driver
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="waitsec"></param>
        public static void CreateDriver(int waitsec = 20)
        {
            Driver = new ChromeDriver();
            Log.WriteLine("Driver created ({0})", Driver.GetType().Name);
            Edr = new EventFiringWebDriver(Driver);
            Log.WriteLine("Event firing driver created");

            Edr.ExceptionThrown += Edr_ExceptionThrown;
            Edr.Navigating += Edr_Navigating;
            Edr.ElementClicking += Edr_ElementClicking;
            Edr.ElementValueChanged += Edr_ElementValueChanged;

            Driver = Edr;
            Log.WriteLine("Event firing driver added to driver");

            BaseWait = new WebDriverWait(Driver, TimeSpan.FromSeconds(waitsec));
            ActionsBuilder = new Actions(Driver);
            Random = new Random();
            JsExecutor = (IJavaScriptExecutor)Driver;
        }

        /// <summary>
        /// Is fired when value of element is changed 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Object representing the element</param>
        private static void Edr_ElementValueChanged(object sender, WebElementEventArgs e)
        {
            try
            {
                string text = e.Element.GetText();
                if (string.IsNullOrEmpty(text))
                    Log.WriteLine("// Cleared elements value or no text put in: {0}", e.Element.GetAttribute("outerHTML"));
                else
                    Log.WriteLine("// Changed value: '{0}' of element '{1}'", text, e.Element.GetAttribute("outerHTML"));

            }
            catch (Exception ex) when (ex is StaleElementReferenceException || ex is NoSuchElementException)
            {
                Log.WriteLine("// Element is no longer present in DOM and can't be logged ({0})", ex.Message);
            }
        }

        /// <summary>
        /// Is fired when you click on something
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Object representing the element</param>
        private static void Edr_ElementClicking(object sender, WebElementEventArgs e)
        {
            string elementText = string.Empty;

            if (!string.IsNullOrEmpty(e.Element.GetText()))
                elementText = e.Element.GetText();
            else if (!string.IsNullOrEmpty(e.Element.GetAttribute("value")))
                elementText = e.Element.GetAttribute("value");

            Log.WriteLine("// Clicked on element: '{0}' ({1})", elementText, e.Element.GetAttribute("outerHTML"));
        }

        /// <summary>
        /// Is fired when you navigate to some URL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Object representing the browser</param>
        private static void Edr_Navigating(object sender, WebDriverNavigationEventArgs e)
        {
            Log.WriteLine("// Navigating to URL: " + e.Url);
        }

        /// <summary>
        /// Is fired when exeption in test is thrown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Object representing the browser</param>
        private static void Edr_ExceptionThrown(object sender, WebDriverExceptionEventArgs e)
        {
            Log.WriteLine("---- Exception in test, message: " + e.ThrownException.Message);
        }

        /// <summary>
        /// Closes driver - profile is not deleted from temp, window is just closed
        /// </summary>
        public static void CloseDriver()
        {
            if (Driver != null)
            {
                Driver.Manage().Cookies.DeleteAllCookies();
                foreach (var handle in Driver.WindowHandles)
                {
                    Driver.SwitchTo().Window(handle);
                    Driver.Close();
                }

                Driver = null;
            }
        }

        /// <summary>
        /// Quits driver (closes window and deletes profile from temp), closes logs
        /// </summary>
        public static void QuitDriver()
        {
            try
            {
                AcceptAlert();
            }
            catch (Exception)
            {
                Log.WriteLine("No alert blocking driver");
            }

            if (Driver != null)
            {
                Driver.Quit();
                Driver = null;
                Log.WriteLine("Driver closed and profile deleted");
            }

            Log.CloseTracers();
        }

        /// <summary>
        /// Deletes all cookies
        /// </summary>
        public static void ClearCookies()
        {
            Driver.Manage().Cookies.DeleteAllCookies();
            Log.WriteLine("Cookies were cleared");
        }

        /// <summary>
        /// Gets string URL from driver
        /// </summary>
        /// <returns>Current URL</returns>
        public static string GetUrl()
        {
            return Driver.Url;
        }

        /// <summary>
        /// Gets source code of current page
        /// </summary>
        /// <returns>Whole source, innetHTML and outerHTML</returns>
        public static string GetSource()
        {
            return Driver.PageSource;
        }

        /// <summary>
        /// Accepts brower JS alert
        /// </summary>
        public static void AcceptAlert()
        {
            Driver.SwitchTo().Alert().Accept();
        }

        /// <summary>
        /// Send keys to JS alert
        /// </summary>
        /// <param name="keys">Text to be inserted</param>
        public static void SendKeysAlert(string keys)
        {
            Driver.SwitchTo().Alert().SendKeys(keys);
        }

        /// <summary>
        /// Navigates to URL
        /// </summary>
        /// <param name="url">URL to navigate to</param>
        public static void GoToUrl(string url)
        {
            Driver.Navigate().GoToUrl(url);
        }

        /// <summary>
        /// Navigates back one entry in history
        /// </summary>
        public static void GoBack()
        {
            Driver.Navigate().Back();
        }

        /// <summary>
        /// Reloads current page
        /// </summary>
        public static void Refresh()
        {
            Driver.Navigate().Refresh();
        }

        /// <summary>
        /// Switches browser back to default content (out of any iframe)
        /// </summary>
        public static void SwitchToDefault()
        {
            Driver.SwitchTo().DefaultContent();
        }

        /// <summary>
        /// Gets cookie by name
        /// </summary>
        /// <param name="name">Name of the cookie</param>
        /// <returns>Returns cookie as object by its name</returns>
        public static Cookie GetCookie(string name)
        {
            return Driver.Manage().Cookies.GetCookieNamed(name);
        }

        /// <summary>
        /// Deletes all cookies from browser
        /// </summary>
        public static void ClearAllCookies()
        {
            Driver.Manage().Cookies.DeleteAllCookies();
        }

        /// <summary>
        /// Maximizes browser window
        /// </summary>
        public static void Maximize()
        {
            Driver.Manage().Window.Maximize();
        }
    }
}
