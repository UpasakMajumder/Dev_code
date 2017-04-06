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
			if(TestEnvironment.IsTestFailed())
				Screenshot.TakeScreenshot();
        }

        [OneTimeTearDown]
        public void AfterAllTests()
        {
		    Log.EndOfFixture();
            Browser.QuitDriver();
        }
    }
}
