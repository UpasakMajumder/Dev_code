using System;
using Kadena.Old_App_Code.Helpers;
using CMS.Tests;
using NUnit.Framework;
using CMS.DataEngine;
using System.IO;
using CMS.SiteProvider;

namespace Kadena.Tests
{
    class UploadFileTest : IntegrationTests
    {
        private string _urlSetting = "KDA_LoadFileUrl";

        [SetUp]
        public void Init()
        {
            SiteContext.CurrentSite = SiteInfoProvider.GetSiteInfo(1);
        }

        [Test]
        public void UploadFileEmptyName()
        {
            using (var stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine("Column1, Column2,Column3,Column4");
                    writer.WriteLine("Data1, Data2,Data3,Data4");
                    writer.Flush();

                    var exc = Assert.Catch(typeof(ArgumentException)
                                            , () => ServiceHelper.UploadFile(stream, null))
                        as ArgumentException;
                    Assert.AreEqual("fileName", exc.ParamName);

                    exc = Assert.Catch(typeof(ArgumentException)
                                            , () => ServiceHelper.UploadFile(stream, string.Empty))
                        as ArgumentException;
                    Assert.AreEqual("fileName", exc.ParamName);

                    exc = Assert.Catch(typeof(ArgumentException)
                                            , () => ServiceHelper.UploadFile(stream, "   "))
                        as ArgumentException;
                    Assert.AreEqual("fileName", exc.ParamName);
                }
            }
        }

        [Test]
        public void UploadFileEmptyStream()
        {
            using (var stream = new MemoryStream())
            {
                var argExc = Assert.Catch(typeof(ArgumentException)
                                        , () => ServiceHelper.UploadFile(null, "name"))
                    as ArgumentException;
                Assert.AreEqual("fileStream", argExc.ParamName);

                argExc = Assert.Catch(typeof(ArgumentException)
                                        , () => ServiceHelper.UploadFile(stream, "name"))
                    as ArgumentException;
                Assert.AreEqual("fileStream", argExc.ParamName);
            }
        }

        [TestCase("http://",
            TestName = "UploadFileIncorrectUrl",
            Description = "Test for exception upon requesting to incorrect url.")]
        public void UploadFileIncorrectUrl(string url)
        {
            var exc = Assert.Catch(typeof(InvalidOperationException)
                                        , () => UploadFile(url));
            Assert.AreEqual("Url for file uploading is not in correct format. Check settings for your site.", exc.Message);
        }

        [TestCase("http://example.com",
            TestName = "UploadFileFail",
            Description = "Test for exception upon requesting url not designed for this task.")]
        public void NotMicroserviceCallTest(string url)
        {
            var exc = Assert.Catch(typeof(InvalidOperationException)
                                        , () => UploadFile(url));
            Assert.AreEqual("Response from microservice is not in correct format.", exc.Message);
        }

        [TestCase("https://eauydb7sta.execute-api.us-east-1.amazonaws.com/Qa/Api/File",
            TestName = "UploadFileSuccess")]
        public void MicroserviceCallTest(string url)
        {
            var fileId = UploadFile(url);
            TestContext.WriteLine($"File Id {fileId}");
            Assert.IsNotEmpty(fileId);
        }

        private string UploadFile(string url)
        {
            Fake<SettingsKeyInfo, SettingsKeyInfoProvider>()
                        .WithData(
                            new SettingsKeyInfo
                            {
                                KeyName = $"{_urlSetting}",
                                KeyValue = url
                            }
                        );


            using (var stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine("Column1, Column2,Column3,Column4");
                    writer.WriteLine("Data1, Data2,Data3,Data4");
                    writer.Flush();
                    return ServiceHelper.UploadFile(stream, "unittestfile.csv");
                }
            }
        }
    }
}
