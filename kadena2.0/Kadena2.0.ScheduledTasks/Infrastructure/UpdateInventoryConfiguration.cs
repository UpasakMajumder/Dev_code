namespace Kadena.ScheduledTasks.Infrastructure
{
    public class UpdateInventoryConfiguration : IConfigurationSection
    {
        public string ErpClientId { get; set; }
        public string InventoryUpdateServiceEndpoint { get; set; }
    }
}