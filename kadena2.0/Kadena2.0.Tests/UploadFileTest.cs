using System;
using Kadena.Old_App_Code.Helpers;
using CMS.Tests;
using NUnit.Framework;
using CMS.DataEngine;
using System.IO;

namespace Kadena.Tests
{
    class UploadFileTest : UnitTests
    {
        private string _customerNameSetting = "KDA_CustomerName";
        private string _urlSetting = "KDA_LoadFileUrl";

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

        [TestCase("",
            "http://",
            TestName = "UploadFileIncorrectCustomer",
            Description = "Test for exception upon requesting with incorrect customer's name.")]
        public void UploadFileIncorrectCustomer(string customerName, string url)
        {
            var exc = Assert.Catch(typeof(InvalidOperationException)
                                        , () => UploadFile(customerName, url));
            Assert.AreEqual("CustomerName not specified. Check settings for your site.", exc.Message);
        }

        [TestCase("actum",
            "http://",
            TestName = "UploadFileIncorrectUrl",
            Description = "Test for exception upon requesting to incorrect url.")]
        public void UploadFileIncorrectUrl(string customerName, string url)
        {
            var exc = Assert.Catch(typeof(InvalidOperationException)
                                        , () => UploadFile(customerName, url));
            Assert.AreEqual("Url for file uploading is not in correct format. Check settings for your site.", exc.Message);
        }

        [TestCase("actum",
            "http://example.com",
            TestName = "UploadFileFail",
            Description = "Test for exception upon requesting url not designed for this task.")]
        public void NotMicroserviceCallTest(string customerName, string url)
        {
            var exc = Assert.Catch(typeof(InvalidOperationException)
                                        , () => UploadFile(customerName, url));
            Assert.AreEqual("Response from microservice is not in correct format.", exc.Message);
        }

        [TestCase("actum",
            "https://eauydb7sta.execute-api.us-east-1.amazonaws.com/Prod/Api/File",
            TestName = "UploadFileSuccess")]
        public void MicroserviceCallTest(string customerName, string url)
        {
            var fileId = UploadFile(customerName, url);
            TestContext.WriteLine($"File Id {fileId}");
            Assert.AreNotEqual(Guid.Empty, fileId);
        }

        private Guid UploadFile(string customerName, string url)
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
