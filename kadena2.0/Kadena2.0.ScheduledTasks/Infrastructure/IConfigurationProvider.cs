namespace Kadena.ScheduledTasks.Infrastructure
{
    public interface IConfigurationSection { }

    public interface IConfigurationProvider
    {
        T Get<T>(string siteName) where T : IConfigurationSection, new();
    }
}