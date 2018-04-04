namespace Kadena.ScheduledTasks.Infrastructure
{
    public interface IConfigurationSection { }

    public interface IConfigurationProvider
    {
        T Get<T>(int siteId) where T : IConfigurationSection, new();
    }
}