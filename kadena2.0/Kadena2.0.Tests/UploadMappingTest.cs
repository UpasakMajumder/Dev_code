using CMS.DataEngine;
using CMS.Tests;
using NUnit.Framework;
using Kadena.Old_App_Code.Helpers;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Kadena.Tests
{
    class UploadMappingTest : UnitTests
    {
        private const string _urlSetting = "KDA_UploadMappingUrl";

        [TestCase("5b602141-33fa-4754-9eb4-af275b80087a",
            "399c95a3-ce5d-46d9-ad1f-1f195ce95596",
            "actum",
            "https://wejgpnn03e.execute-api.us-east-1.amazonaws.com/Qa/Api/DeliveryAddress",
            TestName = "UploadMappingFail",
            Description = "Testing incorrect parameters passed to microservice.")]
        public void IncorrectMappingExceptionCase(string fileId, string containerId, string customerName, string url)
        {
            var mapping = new Dictionary<string, int>()
            {
                { "Title", -1 },
                { "LastName", 2},
                { "Address1", 3},
                { "Address2", 4},
                { "City", 5 },
                { "State", 6},
                { "Zip", 7}
            };

            Assert.Catch(typeof(HttpRequestException)
                , () => CallService(fileId, new Guid(containerId), customerName, url, mapping));
        }

        [TestCase("actum/original-mailing/cc122b9c-f0cc-43f8-a9a8-b5a429976844",
            "399c95a3-ce5d-46d9-ad1f-1f195ce95596",
            "actum",
            "https://wejgpnn03e.execute-api.us-east-1.amazonaws.com/Qa/Api/DeliveryAddress",
            TestName = "UploadMappingCorrectMapping",
            Description = "Testing for successful call of service.")]
        public void CorrectMappingCase(string fileId, string containerId, string customerName, string url)
        {
            var mapping = new Dictionary<string, int>()
            {
                { "Title", 0 },
                { "FirstName", 1},
                { "LastName", 2},
                { "Address1", 3},
                { "Address2", 4},
                { "City", 5 },
                { "State", 6},
                { "Zip", 7}
            };

            CallService(fileId, new Guid(containerId), customerName, url, mapping);
        }

        private void CallService(string fileId, Guid containerId, string customerName, string url, Dictionary<string, int> mapping)
        {
            Fake<SettingsKeyInfo, SettingsKeyInfoProvider>()
            .WithData(
                new SettingsKeyInfo
                {
                    KeyName = $"{_urlSetting}",
                    KeyValue = url
                }
            );

            ServiceHelper.UploadMapping(fileId, containerId, mapping);
        }
    }
}