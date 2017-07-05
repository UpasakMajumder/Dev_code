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

        [Test]
        public void When_ChangingPassword_Expect_PasswordIsValidated()
        {
            //login
            var login = new Login();
            login.Open();
            login.FillLogin(TestCustomer.Name, TestCustomer.Password);
            var dashboard = login.Submit();
            dashboard.WaitForKadenaPageLoad();

            //Switch to password tab and try to submit password which is
            //not strong enough
            var settings = new Settings();
            settings.Open();
            settings.SelectTab(Settings.Tabs.Password);
            settings.SubmitNotStrongPassword(TestCustomer.Password);
            Assert.IsTrue(settings.IsPasswordErrorDisplayed());
        }

        [Test]
        public void When_ChangingAddress_Expect_AddressIsChanged()
        {
            //login
            var login = new Login();
            login.Open();
            login.FillLogin(TestCustomer.Name, TestCustomer.Password);
            var dashboard = login.Submit();
            dashboard.WaitForKadenaPageLoad();

            //Go to addresses, change the first address and verify if it was successfully changed
            var settings = new Settings();
            settings.Open();
            settings.SelectTab(Settings.Tabs.Addresses);
            var address = settings.ChangeFirstAddress();
            Assert.IsTrue(settings.WasFirstAddressChangedCorrectly(address));
        }
    }
}
