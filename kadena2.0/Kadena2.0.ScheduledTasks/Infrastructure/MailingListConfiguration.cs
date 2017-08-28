namespace Kadena.ScheduledTasks.Infrastructure
{
    public class MailingListConfiguration : IConfigurationSection
    {
        public string DeleteMailingListsByValidToDateURL { get; set; }
        public int? DeleteMailingListsPeriod { get; set; }
    }
}