using System;
using Kadena.Old_App_Code.Helpers;
using CMS.Tests;
using NUnit.Framework;
using CMS.DataEngine;
using CMS.SiteProvider;

namespace Kadena.Tests
{
    class ServiceHelperTest : UnitTests
    {
        private string _empty = string.Empty;
        private string _null = null;
        private string _whiteSpace = "   ";
        private string _mailType = "type";
        private string _product = "product";
        private string _validity = "validity";
        private string _site = "Kadena";
        private string _customerName = "actum";
        private string _customerNameSetting = "KDA_CustomerName";
        private string _createContainerUrl = "http://";
        private string _createContainerUrlSetting = "KDA_CreateContainerUrl";

        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void ExceptionTest()
        {
            var argExc = Assert.Catch(typeof(ArgumentException), () => ServiceHelper.CreateMailingContainer(_empty, _product, _validity)) as ArgumentException;
            Assert.AreEqual("mailType", argExc.ParamName);
            argExc = Assert.Catch(typeof(ArgumentException), () => ServiceHelper.CreateMailingContainer(_mailType, _null, _validity)) as ArgumentException;
            Assert.AreEqual("product", argExc.ParamName);
            argExc = Assert.Catch(typeof(ArgumentException), () => ServiceHelper.CreateMailingContainer(_mailType, _product, _whiteSpace)) as ArgumentException;
            Assert.AreEqual("validity", argExc.ParamName);

            var exc = Assert.Catch(typeof(InvalidOperationException)
                , () => ServiceHelper.CreateMailingContainer(_mailType, _product, _validity));
            Assert.AreEqual("CustomerName not specified. Check settings for your site.", exc.Message);
            Fake<SettingsKeyInfo, SettingsKeyInfoProvider>()
                .WithData(
                new SettingsKeyInfo { KeyName = $"{_customerNameSetting}", KeyValue = "actum" },
                new SettingsKeyInfo { KeyName = $"{_createContainerUrlSetting}", KeyValue = _whiteSpace }
                );
            exc = Assert.Catch(typeof(InvalidOperationException)
                , () => ServiceHelper.CreateMailingContainer(_mailType, _product, _validity));
            Assert.AreEqual("Mailing service is not in correct format. Check settings for your site.", exc.Message);
        }


        [Test]
        public void NoExceptionTest()
        {
            Fake<SettingsKeyInfo, SettingsKeyInfoProvider>()
                .WithData(
                new SettingsKeyInfo { KeyName = $"{_customerNameSetting}", KeyValue = "actum" },
                new SettingsKeyInfo { KeyName = $"{_createContainerUrlSetting}", KeyValue = "http://example.com" }
                );
            
            ServiceHelper.CreateMailingContainer(_mailType, _product, _validity);
        }
    }
}
