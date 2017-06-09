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
    class SettingsTests : BaseTest
    {
        [Test]
        public void When_UserChangesHisProfile_Expect_ProfileIsUpdated()
        {
            //login
            var login = new Login();
            login.Open();
            login.FillLogin(TestUser.Name, TestUser.Password);
            var dashboard = login.Submit();
            dashboard.WaitForKadenaPageLoad();

            //edit contact form and save
            var settings = new Settings();
            settings.Open();
            settings.WaitUntilSettingsBlockIsDisplayed();
            settings.FillOutForm();
            settings.ClickSaveChangesButton();

            //verifies if values were saved successfully
            Assert.IsTrue(settings.AreValuesInFormFilledOut(), "One of the fields is empty");
        }
    }
}
