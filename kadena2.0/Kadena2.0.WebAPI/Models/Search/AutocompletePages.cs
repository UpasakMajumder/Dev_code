using System.Collections.Generic;

namespace Kadena.WebAPI.Models.Search
{
    public class AutocomletePages
    {
        public string Url { get; set; }
        public IList<AutocompletePage> Items { get; set; }
    }
}
