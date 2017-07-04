using System;
using Kadena.Old_App_Code.Helpers;
using CMS.Tests;
using NUnit.Framework;
using CMS.DataEngine;
using CMS.SiteProvider;

namespace Kadena.Tests
{
    class CreateContainerTest : IntegrationTests
    {
        private string _mailType = "type";
        private string _product = "product";
        private int _validity = 90;
        private string _urlSetting = "KDA_CreateContainerUrl";

        [SetUp]
        public void Init()
        {
            SiteContext.CurrentSite = SiteInfoProvider.GetSiteInfo(1);
        }

        [Test]
        public void CreateContainerEmptyProduct()
        {
            var name = "CreateContainerEmptyProductTest";
            var argExc = Assert.Catch(typeof(ArgumentException)
                                    , () => ServiceHelper.CreateMailingContainer(name, _mailType, string.Empty, _validity))
                as ArgumentException;
            Assert.AreEqual("product", argExc.ParamName);

            argExc = Assert.Catch(typeof(ArgumentException)
                                    , () => ServiceHelper.CreateMailingContainer(name, _mailType, null, _validity))
                as ArgumentException;
            Assert.AreEqual("product", argExc.ParamName);

            argExc = Assert.Catch(typeof(ArgumentException)
                                    , () => ServiceHelper.CreateMailingContainer(name, _mailType, "    ", _validity))
                as ArgumentException;
            Assert.AreEqual("product", argExc.ParamName);
        }

        [Test]
        public void CreateContainerEmptyMailType()
        {
            var name = "CreateContainerEmptyMailTypeTest";
            var argExc = Assert.Catch(typeof(ArgumentException)
                                    , () => ServiceHelper.CreateMailingContainer(name, string.Empty, _product, _validity))
                as ArgumentException;
            Assert.AreEqual("mailType", argExc.ParamName);

            argExc = Assert.Catch(typeof(ArgumentException)
                                    , () => ServiceHelper.CreateMailingContainer(name, null, _product, _validity))
                as ArgumentException;
            Assert.AreEqual("mailType", argExc.ParamName);

            argExc = Assert.Catch(typeof(ArgumentException)
                                    , () => ServiceHelper.CreateMailingContainer(name, "   ", _product, _validity))
                as ArgumentException;
            Assert.AreEqual("mailType", argExc.ParamName);
        }

        [TestCase("http://",
            TestName = "CreateContainerIncorrectUrl",
            Description = "Test for exception throw upon requesting url not designed for this task."
            )]
        public void CreateContainerIncorrectUrl(string url)
        {
            var exc = Assert.Catch(typeof(InvalidOperationException)
                , () => CreateMailingContainer("CreateContainerIncorrectUrlTest", _mailType, _product, _validity, url));
            Assert.AreEqual("Url for creating container is not in correct format. Check settings for your site.", exc.Message);
        }

        [TestCase("http://example.com",
            TestName = "CreateContainerFail",
            Description = "Test for exception throw upon requesting url not designed for this task."
            )]
        public void NotMicroserviceCallTest(string url)
        {
            var exc = Assert.Catch(typeof(InvalidOperationException)
                , () => CreateMailingContainer("CreateContainerFailTest", _mailType, _product, _validity, url));
            Assert.AreEqual("Response from microservice is not in correct format.", exc.Message);
        }

        [TestCase("https://wejgpnn03e.execute-api.us-east-1.amazonaws.com/Qa/Api/Mailing",
            TestName = "CreateContainerSuccess",
            Description = "Test for successful creation of mailing container."
            )]
        public void CreateContainerSuccess(string url)
        {
            var containerId = CreateMailingContainer("CreateContainerSuccessTest", _mailType, _product, _validity, url);
            Assert.AreNotEqual(Guid.Empty, containerId);
        }

        private Guid CreateMailingContainer(string name, string mailType, string product, int validity, string url)
        {
            Fake<SettingsKeyInfo, SettingsKeyInfoProvider>()
                .WithData(
                new SettingsKeyInfo
                {
                    KeyName = $"{_urlSetting}",
                    KeyValue = url
                }
                );

            return ServiceHelper.CreateMailingContainer(name, mailType, product, validity);
        }
    }
}
