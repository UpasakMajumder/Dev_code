using CMS.EventLog;
using Kadena.WebAPI.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kadena.WebAPI.Services
{
    public class KenticoLogger : IKenticoLogger
    {
        public void LogException(string source, Exception ex)
        {
            EventLogProvider.LogException(source, "EXCEPTION", ex);
        }

        public void LogError(string source, string error)
        {
            EventLogProvider.LogInformation(source, "ERROR", error);
        }

        public void LogInfo(string source, string eventCode, string info)
        {
            EventLogProvider.LogInformation(source, eventCode, eventDescription: info);
        }
    }
}