using CMS.EventLog;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoLogger : IKenticoLogger
    {
        public void LogException(string source, Exception ex)
        {
            EventLogProvider.LogException(source, "EXCEPTION", ex);
        }

        public void LogError(string source, string error)
        {
            EventLogProvider.LogEvent(EventType.ERROR, source, "ERROR", error);
        }

        public void LogInfo(string source, string eventCode, string info)
        {
            EventLogProvider.LogInformation(source, eventCode, eventDescription: info);
        }
    }
}