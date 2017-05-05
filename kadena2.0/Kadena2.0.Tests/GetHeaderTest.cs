using NUnit.Framework;
using CMS.Tests;
using Kadena.Old_App_Code.Helpers;
using System;
using CMS.DataEngine;

namespace Kadena.Tests
{
    class GetHeaderTest : UnitTests
    {
        private string _customerNameSetting = "KDA_CustomerName";
        private string _urlSetting = "KDA_GetHeadersUrl";

        [TestCase("5b602141-33fa-4754-9eb4-af275b80087a"
            , "actum"
            , "https://7e67w2v6q8.execute-api.us-east-1.amazonaws.com/Prod/Api/CsvParser/GetHeaders")]
        public void GetHeaderSuccessfulTest(string id, string customerName, string url)
        {
            Fake<SettingsKeyInfo, SettingsKeyInfoProvider>()
            .WithData(
                new SettingsKeyInfo { KeyName = $"{_customerNameSetting}", KeyValue = customerName },
                new SettingsKeyInfo
                {
                    KeyName = $"{_urlSetting}",
                    KeyValue = url
                }
            );

            var headers = ServiceHelper.GetHeaders(new Guid(id));
            Assert.IsNotEmpty(headers);
            TestContext.WriteLine(string.Join(",", headers));
        }
    }
}
