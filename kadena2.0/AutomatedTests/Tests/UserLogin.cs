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
        public void LogUserIns()
        {
            var login = new Login();
            login.Open();
            login.FillLogin(TestUser.Name, TestUser.Password);
            var dashboard = login.Submit();
        }

        [Test]
        public void TestingTests()
        {

        }
    }
}
