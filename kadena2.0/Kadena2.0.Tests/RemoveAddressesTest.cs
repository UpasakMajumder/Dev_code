using NUnit.Framework;
using CMS.Tests;
using Kadena.Old_App_Code.Helpers;
using CMS.DataEngine;
using System;

namespace Kadena.Tests
{
    class RemoveAddressesTest : UnitTests
    {
        private string _urlSetting = "KDA_DeleteAddressesUrl";

        [TestCase("b44486be-e70d-460d-b81a-affa4834e3ed",
            "02434870-ac23-4cf1-9afc-cfeb8fe7c18a",
            "https://wejgpnn03e.execute-api.us-east-1.amazonaws.com/Qa/api/DeliveryAddress/BulkDelete",
            TestName = "RemoveAddressSuccess",
            Description = "Test for removing specified address from existing container."
            )]
        public void RemoveAddressSuccess(string containerId, string addressId, string url)
        {
            Assert.DoesNotThrow(() => RemoveAddresses(new Guid(containerId), null, url));
        }

        [TestCase("b44486be-e70d-460d-b81a-affa4834e3ed",
            "02434870-ac23-4cf1-9afc-cfeb8fe7c18a",
            "https://wejgpnn03e.execute-api.us-east-1.amazonaws.com/Qa/api/DeliveryAddress/BulkDelete",
            TestName = "RemoveAllAddressesSuccess",
            Description = "Test for removing all addresses from existing container."
            )]
        public void RemoveAllAddressesSuccess(string containerId, string addressId, string url)
        {
            Assert.DoesNotThrow(() => RemoveAddresses(new Guid(containerId), null, url));
        }

        private void RemoveAddresses(Guid containerId, Guid[] ids, string url)
        {
            Fake<SettingsKeyInfo, SettingsKeyInfoProvider>()
                .WithData(
                new SettingsKeyInfo
                {
                    KeyName = $"{_urlSetting}",
                    KeyValue = url
                }
                );

            ServiceHelper.RemoveAddresses(containerId, ids);
        }
    }
}
