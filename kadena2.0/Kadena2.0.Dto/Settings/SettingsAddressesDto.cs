using System;

namespace Kadena.Dto.Settings
{
    public class SettingsAddressesDto
    {
        public AddressBlockDto Billing { get; set; }
        public AddressBlockDto Shipping { get; set; }
        public EditorDto Dialog { get; set; }
    }
}
