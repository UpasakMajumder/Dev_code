using System.Collections.Generic;

namespace Kadena.Models.Settings
{
    public class AddressList
    {
        public string Title { get; set; }
        public int AllowAddresses { get; set; }
        public PageButton AddButton { get; set; }
        public PageButton EditButton { get; set; }
        public PageButton RemoveButton { get; set; }
        public List<DeliveryAddress> Addresses { get; set; }
    }
}