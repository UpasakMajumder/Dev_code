using System;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoLogger
    {
        void LogException(string source, Exception ex);
        void LogError(string source, string error);
        void LogInfo(string source, string eventCode, string info);
    }
}
