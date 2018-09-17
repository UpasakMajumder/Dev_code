using System.Collections.Generic;

namespace Kadena.Dto.Checkout
{
    public class PaymentMethodDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public bool Disabled { get; set; }
        public bool Checked { get; set; }
        public bool HasInput { get; set; }
        public string InputPlaceholder { get; set; }
        public bool ApprovalRequired { get; set; }
        public IList<StoredCardDto> Items { get; set; }
    }
}
