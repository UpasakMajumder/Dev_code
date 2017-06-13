using System.Collections.Generic;

namespace Kadena.Dto.Settings
{
    class EditorDto
    {
        public List<EditorTypeDto> Types { get; set; }
        public List<EditorButtonDto> Buttons { get; set; }
        public List<EditorFieldDto> Fields { get; set; }
    }
}
