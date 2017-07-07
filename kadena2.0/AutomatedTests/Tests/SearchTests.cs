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
    class SearchTests : BaseTest
    {
        [Test]
        public void When_Searching_Expect_ResultsAreFound()
        {
            //login
            var login = new Login();
            login.Open();
            login.FillLogin(TestCustomer.Name, TestCustomer.Password);
            var dashboard = login.Submit();
            dashboard.WaitForKadenaPageLoad();
            dashboard.WaitForLoading();

            //type into search input and verify search suggestions
            string productText = "product";
            dashboard.SearchForText(productText);
            Assert.IsFalse(dashboard.searchSuggestionBox.AreThereMoreThanThreeSuggestions());

            //go to SERP and verify if there are results
            var serp = dashboard.searchSuggestionBox.ClickShowAllProducts();
            Assert.IsTrue(serp.AreThereResults());
        }
    }
}
