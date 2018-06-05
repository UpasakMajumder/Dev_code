using Amazon;
using Amazon.Runtime;
using CMS;
using CMS.Base;
using CMS.DataEngine;
using CMS.EventLog;
using Kadena.AWSLogging;
using System;
using System.Text;

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
            var logGroup = SettingsKeyInfoProvider.GetValue("KDA_AWS_LogGroup");
            var regionSettingsKey = SettingsKeyInfoProvider.GetValue("KDA_AWS_RegionEndpoint");
            var region = RegionEndpoint.GetBySystemName(SettingsKeyInfoProvider.GetValue("KDA_AWS_RegionEndpoint"));

            // to prevent error when not filled in kentico
            if (region.DisplayName == "Unknown")
                region = RegionEndpoint.USEast1;

            return new AWSLoggerConfig("kenticoLogs")
            {
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
                var message = GetLogEventDetails(record);
                logger.AddMessage(message);
            }
        }

        private string GetLogEventBrief(EventLogInfo record)
        {
            return $"{record.EventCode} | {record.EventDescription} | {record.EventUrl} | {record.Exception?.ToString() ?? "<no exception>"}";
        }

        private string GetLogEventDetails(EventLogInfo record)
        {
            if (record == null)
                return "<null>";

            var sb = new StringBuilder();

            sb.AppendLine($"Time: {record.EventTime}")
              .AppendLine($"Source: {record.Source}")
              .AppendLine($"Type: {record.EventType}")
              .AppendLine($"Code: {record.EventCode}")
              .AppendLine($"ID: {record.EventID}")
              .AppendLine($"User ID: {record.UserID}")
              .AppendLine($"User name: {record.UserName}")
              .AppendLine($"IP address: {record.IPAddress}")
              .AppendLine($"Description: {record.EventDescription}")
              .AppendLine($"Machine name: {record.EventMachineName}")
              .AppendLine($"Event URL: {record.EventUrl}")
              .AppendLine($"URL referrer: {record.EventUrlReferrer}")
              .AppendLine($"User agent: {record.EventUserAgent}");

            return sb.ToString();
        }
    }
}