using System.Collections.Generic;

namespace Kadena.Dto.Settings
{
    public class EditorDto
    {
        public EditorTypeDto Types { get; set; }
        public EditorButtonDto Buttons { get; set; }
        public List<EditorFieldDto> Fields { get; set; }
    }
}
