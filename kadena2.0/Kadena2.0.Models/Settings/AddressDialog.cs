using System.Collections.Generic;

namespace Kadena.Models.Settings
{
    public class AddressDialog
    {
        public DialogType Types { get; set; }
        public DialogButton Buttons { get; set; }
        public List<DialogField> Fields { get; set; }
    }
}