using System.Collections.Generic;

namespace Kadena.WebAPI.Models.Search
{
    public class SearchResultPage
    {
        public IList<ResultItemProduct> products { get; set; }
        public IList<ResultItemPage> pages { get; set; }
    }
}
