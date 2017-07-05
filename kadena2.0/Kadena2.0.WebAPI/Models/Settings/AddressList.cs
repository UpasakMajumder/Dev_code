using System.Collections.Generic;

namespace Kadena.WebAPI.Models.Settings
{
    public class AddressList
    {
        public string Title { get; set; }
        public PageButton AddButton { get; set; }
        public string EditButtonText { get; set; }
        public string RemoveButtonText { get; set; }
        public List<DeliveryAddress> Addresses { get; set; }
    }
}