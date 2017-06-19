namespace Kadena.Dto.Settings
{
    public class SettingsAddressesDto
    {
        public object Billing { get; set; }
        public AddressListDto Shipping { get; set; }
        public AddressDialogDto Dialog { get; set; }
    }
}
