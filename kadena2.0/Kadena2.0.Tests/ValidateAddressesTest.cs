using NUnit.Framework;
using CMS.Tests;
using System;
using CMS.DataEngine;
using Kadena.Old_App_Code.Helpers;
using System.Net.Http;

namespace Kadena.Tests
{
    class ValidateAddressesTest : UnitTests
    {
        private string _urlSetting = "KDA_ValidateAddressUrl";
        
        [TestCase("b44486be-e70d-460d-b81a-affa4834e3ed",
            "https://o31vibmca2.execute-api.us-east-1.amazonaws.com/Qa/Api/AddressValidator",
            TestName = "ValidateAddressesSuccess",
            Description = "Test for validation address on correct container."
            )]
        public void ValidateSuccess(string containerId, string url)
        {
            var fileId = ValidateAddress(new Guid(containerId), url);
            Assert.IsNotEmpty(fileId);
            TestContext.WriteLine($"Validated file is {fileId}");
        }

        [TestCase("8F266B1E-0677-4085-BAAB-20F0905F4C8F",
            "https://o31vibmca2.execute-api.us-east-1.amazonaws.com/Qa/Api/AddressValidator",
            TestName = "ValidateAddressesHttpException",
            Description = "Test for validation address on container that doesn't exists."
            )]
        public void ValidateFail(string containerId, string url)
        {
            Assert.Catch(typeof(HttpRequestException), ()=> ValidateAddress(new Guid(containerId), url));
        }

        private string ValidateAddress(Guid containerId, string url)
        {
            Fake<SettingsKeyInfo, SettingsKeyInfoProvider>()
                .WithData(
                new SettingsKeyInfo
                {
                    KeyName = $"{_urlSetting}",
                    KeyValue = url
                }
                );

            return ServiceHelper.ValidateAddresses(containerId);
        }
    }
}
