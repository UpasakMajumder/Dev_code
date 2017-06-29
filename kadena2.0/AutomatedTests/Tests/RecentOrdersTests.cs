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
    class RecentOrdersTests : BaseTest
    {
        [Test]
        public void When_RecentOrdersOpened_Expect_OrdersDisplayed()
        {
            //login
            var login = new Login();
            login.Open();
            login.FillLogin(TestCustomer.Name, TestCustomer.Password);
            var dashboard = login.Submit();
            dashboard.WaitForKadenaPageLoad();

            //Open recent orders
            var recentOrders = new RecentOrders();
            recentOrders.Open();

            //Are There any orders
            Assert.IsTrue(recentOrders.IsThereAnyOrderInTable());
        }
    }
}
