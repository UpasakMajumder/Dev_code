using System.Collections.Generic;

namespace Kadena.Models.Search
{
    public class AutocompleteProducts
    {
        public string Url { get;set;}
        public IList<AutocompleteProduct> Items { get; set; }
        
    }
}
