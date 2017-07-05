using System.Collections.Generic;

namespace Kadena.WebAPI.Models.Settings
{
    public class DialogField
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public List<object> Values { get; set; }
        public string Type { get; set; }
        public bool IsOptional { get; set; } = false;
    }
}