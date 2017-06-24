using System.Collections.Generic;

namespace Kadena.WebAPI.Models.Search
{
    public class AutocompletePages
    {
        public string Url { get; set; }
        public IList<AutocompletePage> Items { get; set; }
    }
}
