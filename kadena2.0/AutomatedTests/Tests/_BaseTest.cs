using AutomatedTests.PageObjects;
using AutomatedTests.Utilities;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AutomatedTests.Tests
{
    class BaseTest
    {
        string pathAuthCookiesFile = TestEnvironment.TestPath + "\\TestFiles\\authCookies.data";

        [OneTimeSetUp]
        public void BeforeAllTests()
        {
            Log.StartOfFixture();
            Browser.CreateDriver();
        }

        [SetUp]
        public void BeforeTest()
        {
            Log.StartOfTest();
        }

        [TearDown]
        public void AfterTest()
        {
            Log.EndOfTest();
            if (TestEnvironment.IsTestFailed())
                AutomatedTests.Utilities.Screenshot.TakeScreenshot();
        }

        [OneTimeTearDown]
        public void AfterAllTests()
        {
            Log.EndOfFixture();
            Browser.QuitDriver();
        }

        /// <summary>
        /// If user is not logged in, logs into the website
        /// </summary>
        /// <returns></returns>
        public void InitializeTest()
        {
            var dashboard = new Dashboard();

            //verify if you are logged in
            if (dashboard.IsLogoutButtonDisplayed())
            {
                return;
            }
            else
            {
                //if not logged in, login to Kadena
                var login = new Login();
                login.Open();

                //if last change of the cookie file is longer than 10 minutes, login with credentials
                //and save cookies to file again
                if (AgeOfLastChangeOfCookieFile() > 10)
                {
                    login.FillLogin(TestCustomer.Name, TestCustomer.Password);
                    dashboard = login.Submit();
                    dashboard.WaitForKadenaPageLoad();
                    dashboard.WaitForRecentOrders();
                    SaveAuthenticationCookiesToFile();
                }
                else
                {
                    AuthenticateUsingACookie();
                }                
            }
        }

        /// <summary>
        /// Logs in to the website as an admin
        /// </summary>
        /// <returns></returns>
        public Dashboard InitializeAdminTest()
        {
            var login = new Login();
            login.Open();
            login.FillLogin(TestUser.Name, TestUser.Password);
            var dashboard = login.Submit();
            dashboard.WaitForKadenaPageLoad();
            return dashboard;
        }

        /// <summary>
        /// Switches to default content
        /// </summary>
        public void EndAdminTest()
        {
            Browser.Driver.SwitchTo().DefaultContent();
        }

       
        /// <summary>
        /// Read the file with authentication cookies and add them with webdriver
        /// </summary>
        public void AuthenticateUsingACookie()
        {            
            string[] filePathAuth = File.ReadLines(pathAuthCookiesFile).ToArray();

            foreach (string line in filePathAuth)
            {
                string[] cookieArray = line.Split(',');
                Cookie cookie = new Cookie(cookieArray[0], cookieArray[1], cookieArray[2], cookieArray[3], System.DateTime.MaxValue);
                Browser.Driver.Manage().Cookies.AddCookie(cookie);
            }                       
        }

        /// <summary>
        /// Looks for cookies needed for authentication and saves them to a file
        /// </summary>
        public void SaveAuthenticationCookiesToFile()
        {
            IList<Cookie> cookies = Browser.Driver.Manage().Cookies.AllCookies;
            IList<Cookie> authenticationCookies = cookies.Where(c => c.Name == ".ASPXFORMSAUTH" || c.Name == "ASP.NET_SessionId").ToList();          
           
            foreach (var cookie in authenticationCookies)
            {
                File.AppendAllLines(pathAuthCookiesFile, new[] { $"{cookie.Name},{cookie.Value},{cookie.Domain},{cookie.Path},{cookie.Expiry},{cookie.Secure}" });
            }
        }

        /// <summary>
        /// Returns age of the last change of the file with authentication cookies in minutes
        /// </summary>
        /// <returns></returns>
        public int AgeOfLastChangeOfCookieFile()
        {
            DateTime cookieFileCreated = File.GetLastWriteTime(pathAuthCookiesFile);
            TimeSpan difference = DateTime.Now - cookieFileCreated;
            return (int)difference.TotalMinutes;
        }
    }
}
