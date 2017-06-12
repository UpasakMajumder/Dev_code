using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kadena.WebAPI.Contracts
{
    public interface IKenticoLogger
    {
        void LogException(string source, Exception ex);
        void LogError(string source, string error);
        void LogInfo(string source, string eventCode, string info);
    }
}
