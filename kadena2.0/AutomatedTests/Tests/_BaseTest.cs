using AutomatedTests.PageObjects;
using AutomatedTests.Utilities;
using NUnit.Framework;

namespace AutomatedTests.Tests
{
    class BaseTest
    {

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
                Screenshot.TakeScreenshot();
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
        public Dashboard InitializeTest()
        {
            var dashboard = new Dashboard();

            //verify if you are logged in
            if (dashboard.IsLogoutButtonDisplayed())
            {
                return dashboard;
            }
            else
            {
                //if not logged in, login to Kadena
                var login = new Login();
                login.Open();
                login.FillLogin(TestCustomer.Name, TestCustomer.Password);
                dashboard = login.Submit();
                dashboard.WaitForKadenaPageLoad();
                return dashboard;
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
    }
}
