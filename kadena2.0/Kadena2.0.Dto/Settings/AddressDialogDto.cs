using System.Collections.Generic;

namespace Kadena.Dto.Settings
{
    public class AddressDialogDto
    {
        public DialogTypeDto Types { get; set; }
        public DialogButtonDto Buttons { get; set; }
        public List<DialogFieldDto> Fields { get; set; }
    }
}
