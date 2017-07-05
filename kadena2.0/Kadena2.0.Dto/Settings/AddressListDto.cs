using System.Collections.Generic;

namespace Kadena.Dto.Settings
{
    public class AddressListDto
    {
        public string Title { get; set; }
        public PageButtonDto AddButton { get; set; }
        public string EditButtonText { get; set; }
        public string RemoveButtonText { get; set; }
        public List<AddressDto> Addresses { get; set; }
    }
}
