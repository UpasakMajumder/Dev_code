using System.Collections.Generic;

namespace Kadena.Dto.Checkout
{
    public class DeliveryAddressesDTO 
    {
        public bool IsDeliverable { get; set; }
        public bool AvailableToAdd { get; set; }
        public string UnDeliverableText { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public NewAddressButtonDTO NewAddress { get; set; }
        public string EmptyMessage { get; set; }
        public List<DeliveryAddressDTO> Items { get; set; }
        public AddressDialogDto DialogUI { get; set; }
    }
}
