namespace Kadena.Models.SiteSettings.Permissions
{
    public static class ModulePermissions
    {
        public static KadenaOrders KadenaOrdersModule { get; } = new KadenaOrders();
        public static KadenaUserSettings KadenaUserSettingsModule { get; } = new KadenaUserSettings();
    }
}
