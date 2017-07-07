using System.Collections.Generic;

namespace Kadena.Models.Search
{
    public class SearchResultPage
    {
        public IList<ResultItemProduct> Products { get; set; }
        public IList<ResultItemPage> Pages { get; set; }
    }
}
