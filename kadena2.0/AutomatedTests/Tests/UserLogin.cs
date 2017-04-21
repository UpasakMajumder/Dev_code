using AutomatedTests.PageObjects;
using AutomatedTests.Utilities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedTests.Tests
{
    class UserLogin : BaseTest
    {
        [Test]
        public void LogUserInWithCorrectCredentials()
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

        [Test]
        public void LoginWithIncorrectCredentials()
        {
            var login = new Login();
            login.Open();

            //enter no username and password
            login.FillLogin(String.Empty, String.Empty);
            login.SubmitInvalid();
            Assert.IsTrue(login.IsErrorMessageDisplayed());

            //enter correct username, no password
            Browser.Refresh();
            login.FillLogin(TestUser.Name, String.Empty);
            login.SubmitInvalid();
            Assert.IsTrue(login.IsErrorMessageDisplayed());
        }
    }
}
