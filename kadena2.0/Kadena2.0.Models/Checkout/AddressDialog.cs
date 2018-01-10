using Kadena.Models.Settings;
using System.Collections.Generic;

namespace Kadena.Models.Checkout
{
    public class AddressDialog
    {
        public string Title { get; set; }
        public string DiscardBtnLabel { get; set; }
        public string SubmitBtnLabel { get; set; }
        public string RequiredErrorMessage { get; set; }
        public IEnumerable<DialogField> Fields { get; set; }
        public string SaveAddressCheckbox { get; set; }
    }
}