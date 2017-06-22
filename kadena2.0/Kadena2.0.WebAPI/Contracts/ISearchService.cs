using Kadena.WebAPI.Models.Search;

namespace Kadena.WebAPI.Contracts
{
    public interface ISearchService
    {
        SearchResultPage Search(string phrase);
    }
}