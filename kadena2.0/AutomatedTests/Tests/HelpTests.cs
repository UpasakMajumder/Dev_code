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
    class HelpTests : BaseTest
    {
        [Test]
        public void When_UserContactsCenveo_Expect_FormIsSubmitted()
        {
            //login
            var login = new Login();
            login.Open();
            login.FillLogin(TestUser.Name, TestUser.Password);
            var dashboard = login.Submit();
            dashboard.WaitForKadenaPageLoad();

            //open contact us page
            var contactUs = new ContactUs();
            contactUs.Open();

            //Verify if validations appear when submitting empty form
            contactUs.ClickSubmitButton();
            Assert.IsTrue(contactUs.IsValidationErrorDisplayed());

            //Fill out the form and submit
            contactUs.FillOutTheContactForm();
            contactUs.ClickSubmitButton();
            var contactFormSuccess = new FormSuccess();
            Assert.IsTrue(contactFormSuccess.IsSubmitConfirmationDisplayed());
        }
    }
}
