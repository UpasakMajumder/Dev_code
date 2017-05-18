﻿using AutomatedTests.PageObjects;
using AutomatedTests.Utilities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace AutomatedTests.Tests
{
    class MailingListTests : BaseTest
    {
        [Test]
        public void When_UploadingMailingList_Expect_MailingListCorrectlyUploaded()
        {
            

            
            //login
            var login = new Login();
            login.Open();
            login.FillLogin(TestUser.Name, TestUser.Password);
            var dashboard = login.Submit();
            dashboard.WaitForKadenaPageLoad();

            //open K-list
            var kList = new KList();
            kList.Open();
            var newKList = kList.ClickAddMailingListBtn();

            //select mailing list and submit it
            newKList.SelectMailingList();
            var mapColumns = newKList.SubmitMailingList();

            //confirm mapping
            var listProcessing = mapColumns.ClickProcessList();

            //verify if the list is being processed
            Assert.True(listProcessing.WasMailingListSubmitted());
            }
        }
    }
}
