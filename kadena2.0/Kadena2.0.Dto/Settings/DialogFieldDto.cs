using System.Collections.Generic;

namespace Kadena.Dto.Settings
{
    public class DialogFieldDto
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public List<object> Values { get; set; }
        public string Type { get; set; }
    }
}
