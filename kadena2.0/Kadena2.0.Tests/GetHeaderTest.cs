using NUnit.Framework;
using CMS.Tests;
using Kadena.Old_App_Code.Helpers;
using CMS.DataEngine;
using System.Collections.Generic;
using System.Net.Http;

namespace Kadena.Tests
{
    class GetHeaderTest : UnitTests
    {
        private string _urlSetting = "KDA_GetHeadersUrl";

        [TestCase("original-mailing/KDA/6dc57861-f089-400e-b905-c10525823700"
            , "https://7e67w2v6q8.execute-api.us-east-1.amazonaws.com/Qa/Api/CsvParser/GetHeaders"
            , TestName = "GetHeaderCorrectFile", Description = "Testing for successful result from request.")]
        public void CorrectFileCase(string id, string url)
        {
            var headers = CallService(id, url);
            Assert.IsNotEmpty(headers);
            TestContext.WriteLine(string.Join(",", headers));
        }

        [TestCase("46497e0d-696c-4bcb-bc47-5428e344f373"
            , "https://7e67w2v6q8.execute-api.us-east-1.amazonaws.com/Qa/Api/CsvParser/GetHeaders"
            , TestName = "GetHeaderWrongFile"
            , Description = "Testing for exception throw when requested file not exists or empty.")]
        public void WrongFileCase(string id, string url)
        {
            var exception = Assert.Catch(typeof(HttpRequestException), () => CallService(id, url));
            TestContext.WriteLine(exception.Message);
        }

        private IEnumerable<string> CallService(string id, string url)
        {
            Fake<SettingsKeyInfo, SettingsKeyInfoProvider>()
            .WithData(
                new SettingsKeyInfo
                {
                    KeyName = $"{_urlSetting}",
                    KeyValue = url
                }
            );
            return ServiceHelper.GetHeaders(id);
        }
    }
}
