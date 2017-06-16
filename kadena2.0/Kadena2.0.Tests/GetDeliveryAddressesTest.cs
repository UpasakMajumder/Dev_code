using NUnit.Framework;
using CMS.Tests;
using System;
using CMS.DataEngine;
using Kadena.Old_App_Code.Helpers;
using System.Net.Http;
using System.Collections.Generic;
using Kadena.Old_App_Code.Kadena.MailingList;

namespace Kadena.Tests
{
    class GetDeliveryAddressesTest : UnitTests
    {
        private string _urlSetting = "KDA_GetMailingAddressesUrl";

        [TestCase("b44486be-e70d-460d-b81a-affa4834e3ed",
            "https://wejgpnn03e.execute-api.us-east-1.amazonaws.com/Qa/api/DeliveryAddress/ByContainer",
            TestName = "GetAddressesSuccess",
            Description = "Test for getting addresses on existing container."
            )]
        public void GetAddressesSuccess(string containerId, string url)
        {
            var addresses = GetAddresses(new Guid(containerId), url);
            Assert.IsNotNull(addresses);
        }

        private IEnumerable<MailingAddressData> GetAddresses(Guid containerId, string url)
        {
            Fake<SettingsKeyInfo, SettingsKeyInfoProvider>()
                .WithData(
                new SettingsKeyInfo
                {
                    KeyName = $"{_urlSetting}",
                    KeyValue = url
                }
                );

            return ServiceHelper.GetMailingAddresses(containerId);
        }
    }
}
