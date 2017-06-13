using System.Collections.Generic;

namespace Kadena.Dto.Settings
{
    class AddressBlockDto
    {
        public string Title { get; set; }
        public ButtonDto AddButton { get; set; }
        public string EditButtonText { get; set; }
        public string RemoveButtonText { get; set; }
        public List<AddressDto> Addresses { get; set; }
    }
}
