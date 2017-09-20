using CMS.DataEngine;
using CMS.Tests;
using Kadena.Old_App_Code.Helpers;
using Kadena.Old_App_Code.Kadena.KSource;
using NUnit.Framework;
using System.Collections.Generic;

namespace Kadena.Tests
{
    class GetProjectsTest : UnitTests
    {
        private string _urlSetting = "KDA_GetProjectsUrl";
        private string _workgroupNameSetting = "KDA_WorkgroupName";
        private const string _nooshApiSettingKey = "KDA_NooshApi";
        private const string _nooshTokenSettingKey = "KDA_NooshToken";

        [TestCase("Abbott Nutrition"
            , "https://acdfsip0c6.execute-api.us-east-1.amazonaws.com/Qa/Api/bid"
            , TestName = "GetProjectsSuccess"
            , Description = "Testing for successful result from request.")]
        public void GetProjectsSuccess(string workgroupName, string url)
        {
            var projects = CallService(workgroupName, url);
            Assert.IsNotNull(projects);
        }

        private IEnumerable<ProjectData> CallService(string workgroupName, string url)
        {
            Fake<SettingsKeyInfo, SettingsKeyInfoProvider>()
            .WithData(
                new SettingsKeyInfo { KeyName = $"{_workgroupNameSetting}", KeyValue = workgroupName },
                new SettingsKeyInfo
                {
                    KeyName = $"{_urlSetting}",
                    KeyValue = url
                },
                new SettingsKeyInfo
                {
                    KeyName = $"{_nooshApiSettingKey}",
                    KeyValue = "https://api.scd.noosh.com/api/v1/"
                },
                new SettingsKeyInfo
                {
                    KeyName = $"{_nooshTokenSettingKey}",
                    KeyValue = "IEKdhpOWS5IEq2aQHAp1ivCtEnlW1f1Jf0JhbbHsbN1IToFoqo5SrInnH5Kw7cnb"
                }
            );
            return BidServiceHelper.GetProjects();
        }
    }
}
