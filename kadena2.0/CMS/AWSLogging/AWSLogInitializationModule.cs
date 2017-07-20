using Amazon;
using Amazon.Runtime;
using CMS;
using CMS.Base;
using CMS.DataEngine;
using CMS.EventLog;
using Kadena.AWSLogging;
using System;

[assembly: RegisterModule(typeof(AWSLogInitializationModule))]

namespace Kadena.AWSLogging
{
    public class AWSLogInitializationModule : Module
    {
        private static IAWSLoggerCore logger = new AWSLoggerCore(CreateConfig(), "kenticoLog");

        // Module class constructor, the system registers the module under the name "CustomInit"
        public AWSLogInitializationModule() : base("AWSLogInitializationModule")
        {

        }

        // Contains initialization code that is executed when the application starts
        protected override void OnInit()
        {
            base.OnInit();
            EventLogEvents.LogEvent.After += LogEvent_After;
        }

        private static AWSLoggerConfig CreateConfig()
        {
            var accessKey = SettingsKeyInfoProvider.GetValue("KDA_AWS_RegionEndpoint");
            var accessSecret = SettingsKeyInfoProvider.GetValue("KDA_AWS_AccessSecret");
            var logGroup = SettingsKeyInfoProvider.GetValue("KDA_AWS_LogGroup");
            var region = RegionEndpoint.GetBySystemName(SettingsKeyInfoProvider.GetValue("KDA_AWS_RegionEndpoint"));

            return new AWSLoggerConfig("kenticoLogs")
            {
                Credentials = new BasicAWSCredentials(accessKey, accessSecret),
                BatchPushInterval = TimeSpan.FromSeconds(10),
                MonitorSleepTime = TimeSpan.FromSeconds(1),
                BatchSizeInBytes = 50000,
                LogGroup = logGroup,
                MaxQueuedMessages = 10,
                RegionEndpoint = region
            };
        }

        private void LogEvent_After(object sender, LogEventArgs e)
        {
            var record = e.Event;
            if (record.EventType.EqualsCSafe("E"))
            {
                logger.AddMessage($"{record.EventCode} | {record.EventDescription} | {record.EventUrl} | {record.Exception?.ToString() ?? "<no exception>"}");
            }
        }
    }
}