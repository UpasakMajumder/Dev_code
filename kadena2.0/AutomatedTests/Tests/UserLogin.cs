using AutomatedTests.PageObjects;
using AutomatedTests.Utilities;
using NUnit.Framework;

namespace AutomatedTests.Tests
{
    class UserLogin : BaseTest
    {
        [Test]
        public void LogUserIn()
        {
            var login = new Login();
            login.Open();
            login.FillLogin(TestUser.Name, TestUser.Password);

            //verify if password is shown as asterisks by default
            Assert.IsTrue(login.IsPasswordShownAsAsterisks());

            //click the toggler to show password and verify if asterisks are not there anymore
            login.ClickInputToggler();
            Assert.IsFalse(login.IsPasswordShownAsAsterisks());

            //click login button
            var dashboard = login.Submit();
            Assert.IsTrue(dashboard.IsDashboardDisplayed());
        }
    }
}
