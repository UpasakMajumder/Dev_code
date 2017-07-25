namespace Kadena.AWSLogging
{
    public interface IAWSLoggerCore
    {
        void Close();
        void AddMessage(string message);
        void StartMonitor();
    }
}