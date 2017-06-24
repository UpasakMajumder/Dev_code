using Kadena.WebAPI.Models.Search;
using System.Collections.Generic;

namespace Kadena.WebAPI.Contracts
{
    public interface ISearchService
    {
        AutocompleteResponse Autocomplete(string phrase);
        SearchResultPage Search(string phrase);
        List<ResultItemProduct> SearchProducts(string phrase);
        List<ResultItemPage> SearchPages(string phrase);
    }
}