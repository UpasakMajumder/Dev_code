namespace Kadena.WebAPI.Models.Settings
{
    public class SettingsAddresses
    {
        public object Billing { get; set; }
        public AddressList Shipping { get; set; }
        public AddressDialog Dialog { get; set; }
    }
}