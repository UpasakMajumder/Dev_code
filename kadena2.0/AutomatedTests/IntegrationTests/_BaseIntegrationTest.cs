using AutomatedTests.Utilities;
using NUnit.Framework;

namespace AutomatedTests.IntegrationTests
{
    class BaseIntegrationTest
    {
        [OneTimeSetUp]
        public void BeforeAllTests()
        {
			Log.StartOfFixture();
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
        }

        [OneTimeTearDown]
        public void AfterAllTests()
        {
		    Log.EndOfFixture();
        }
    }
}
