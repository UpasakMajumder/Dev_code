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
        public void ExceptionTest()
        {
            using (var stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine("Column1, Column2,Column3,Column4");
                    writer.WriteLine("Data1, Data2,Data3,Data4");
                    writer.Flush();

                    var argExc = Assert.Catch(typeof(ArgumentException)
                                            , () => ServiceHelper.UploadFile(null, "name"))
                        as ArgumentException;
                    Assert.AreEqual("fileStream", argExc.ParamName);

                    argExc = Assert.Catch(typeof(ArgumentException)
                                            , () => ServiceHelper.UploadFile(stream, null))
                        as ArgumentException;
                    Assert.AreEqual("fileName", argExc.ParamName);

                    var exc = Assert.Catch(typeof(InvalidOperationException)
                        , () => ServiceHelper.UploadFile(stream, "name"));
                    Assert.AreEqual("CustomerName not specified. Check settings for your site.", exc.Message);
                    
                    Fake<SettingsKeyInfo, SettingsKeyInfoProvider>()
                        .WithData(
                        new SettingsKeyInfo { KeyName = $"{_customerNameSetting}", KeyValue = "actum" },
                        new SettingsKeyInfo { KeyName = $"{_urlSetting}", KeyValue = "http://" }
                        );
                    exc = Assert.Catch(typeof(InvalidOperationException)
                        , () => ServiceHelper.UploadFile(stream, "name"));
                    Assert.AreEqual("Url for file uploading is not in correct format. Check settings for your site.", exc.Message);
                }
            }
        }
    }
}
