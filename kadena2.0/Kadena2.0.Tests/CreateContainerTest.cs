using System;
using Kadena.Old_App_Code.Helpers;
using CMS.Tests;
using NUnit.Framework;
using CMS.DataEngine;

namespace Kadena.Tests
{
    class CreateContainerTest : UnitTests
    {
        private string _empty = string.Empty;
        private string _null = null;
        private string _whiteSpace = "   ";
        private string _mailType = "type";
        private string _product = "product";
        private int _validity = 90;
        private string _customerNameSetting = "KDA_CustomerName";
        private string _urlSetting = "KDA_CreateContainerUrl";

        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void ExceptionTest()
        {
            var argExc = Assert.Catch(typeof(ArgumentException)
                                    , () => ServiceHelper.CreateMailingContainer(_empty, _product, _validity))
                as ArgumentException;
            Assert.AreEqual("mailType", argExc.ParamName);

            argExc = Assert.Catch(typeof(ArgumentException)
                                    , () => ServiceHelper.CreateMailingContainer(_mailType, _null, _validity))
                as ArgumentException;
            Assert.AreEqual("product", argExc.ParamName);
            
            var exc = Assert.Catch(typeof(InvalidOperationException)
                , () => ServiceHelper.CreateMailingContainer(_mailType, _product, _validity));
            Assert.AreEqual("CustomerName not specified. Check settings for your site.", exc.Message);

            Fake<SettingsKeyInfo, SettingsKeyInfoProvider>()
                .WithData(
                new SettingsKeyInfo { KeyName = $"{_customerNameSetting}", KeyValue = "actum" },
                new SettingsKeyInfo { KeyName = $"{_urlSetting}", KeyValue = _whiteSpace }
                );
            exc = Assert.Catch(typeof(InvalidOperationException)
                , () => ServiceHelper.CreateMailingContainer(_mailType, _product, _validity));
            Assert.AreEqual("Url for creating container is not in correct format. Check settings for your site.", exc.Message);
        }

        [Test]
        public void NotMicroserviceCallTest()
        {
            Fake<SettingsKeyInfo, SettingsKeyInfoProvider>()
                .WithData(
                new SettingsKeyInfo { KeyName = $"{_customerNameSetting}", KeyValue = "actum" },
                new SettingsKeyInfo { KeyName = $"{_urlSetting}", KeyValue = "http://example.com" }
                );

            var exc = Assert.Catch(typeof(InvalidOperationException)
                , () => ServiceHelper.CreateMailingContainer(_mailType, _product, _validity));
            Assert.AreEqual("Response from microservice is not in correct format.", exc.Message);
        }

        [Test]
        public void MicroserviceCallTest()
        {
            Fake<SettingsKeyInfo, SettingsKeyInfoProvider>()
                .WithData(
                new SettingsKeyInfo { KeyName = $"{_customerNameSetting}", KeyValue = "actum" },
                new SettingsKeyInfo
                {
                    KeyName = $"{_urlSetting}",
                    KeyValue = "https://wejgpnn03e.execute-api.us-east-1.amazonaws.com/Prod/Api/Mailing"
                }
                );

            var containerId = ServiceHelper.CreateMailingContainer(_mailType, _product, _validity);
            Assert.AreNotEqual(Guid.Empty, containerId);
        }
    }
}
