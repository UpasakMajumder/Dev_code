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
    class KSourceTests : BaseTest
    {
        [Test]
        public void When_KSourceIsOPened_Expect_ListOfProjects()
        {
            //login
            InitializeTest();
            var dashboard = new Dashboard();

            //open Ksource and check if there are projects in the table
            var kSource = new KSource();
            kSource.Open();
            Assert.IsTrue(kSource.AreThereAnyProjects());
        }
    }
}
