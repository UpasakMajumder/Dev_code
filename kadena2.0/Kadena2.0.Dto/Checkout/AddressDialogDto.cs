using Kadena.Dto.Settings;
using System.Collections.Generic;

namespace Kadena.Dto.Checkout
{
    public class AddressDialogDto
    {
        public string Title { get; set; }
        public string DiscardBtnLabel { get; set; }
        public string SubmitBtnLabel { get; set; }
        public string RequiredErrorMessage { get; set; }
        public string SaveAddressCheckbox { get; set; }
        public IEnumerable<DialogFieldDto> Fields { get; set; }
    }
}