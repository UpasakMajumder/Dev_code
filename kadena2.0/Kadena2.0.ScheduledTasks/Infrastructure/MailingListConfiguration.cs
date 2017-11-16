namespace Kadena.ScheduledTasks.Infrastructure
{
    public class MailingListConfiguration : IConfigurationSection
    {
        public string MailingServiceUrl { get; set; }
        public int? DeleteMailingListsPeriod { get; set; }
    }
}