using System.Collections.Generic;

namespace Kadena.Dto.Settings
{
    public class AddressListDto
    {
        public string Title { get; set; }
        public int AllowAddresses { get; set; }
        public PageButtonDto AddButton { get; set; }
        public PageButtonDto EditButton { get; set; }
        public PageButtonDto RemoveButton { get; set; }
        public DefaultAddressDto DefaultAddress { get; set; }
        public List<AddressDto> Addresses { get; set; }
    }
}
