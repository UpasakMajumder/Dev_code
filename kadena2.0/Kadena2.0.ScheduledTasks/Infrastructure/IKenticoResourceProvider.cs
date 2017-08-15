namespace Kadena.ScheduledTasks.Infrastructure
{
    public interface IKenticoResourceProvider
    {
        string GetSettingsByKey(string siteName, string key);
    }
}