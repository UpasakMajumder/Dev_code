using Kadena.WebAPI.Contracts;
using AutoMapper;
using System.Data;
using Kadena.WebAPI.Models.Search;
using System.Collections.Generic;

namespace Kadena.WebAPI.Services
{
    public class SearchService : ISearchService
    {
        private readonly IMapper mapper;
        private readonly IKenticoResourceService resources; // todo check needed ?
        private readonly IKenticoSearchService kenticoSearch;

        public SearchService(IMapper mapper, IKenticoResourceService resources, IKenticoSearchService kenticoSearch)
        {
            this.mapper = mapper;
            this.resources = resources;
            this.kenticoSearch = kenticoSearch;
        }

        public SearchResultPage Search(string phrase)
        {
            var datasetResults = kenticoSearch.Search(phrase);

            if (datasetResults == null || datasetResults.Tables.Count == 0) // todo refine checking for empty
            {
                return null;
            }

            var searchResultPages = new List<ResultItemPage>();
            var searchResultProducts = new List<ResultItemProduct>();

            foreach (DataTable table in datasetResults.Tables)
            {
                foreach (DataRow dr in table.Rows)
                {
                    var resultItem = new ResultItemPage()
                    {
                        Id  = 0,
                        Text = "adasdasd",
                        Title = dr[4].ToString(),
                        Url = "sdfsdf"
                    };

                    searchResultPages.Add(resultItem);

                    //ImageUrl = dr[7].ToString().Replace("~", "")
                    // var nodeID = Convert.ToInt32(((dr[0].ToString()).Split(";".ToCharArray())[1]).Split("_".ToCharArray())[0]);
                    // var node = tree.SelectSingleNode(nodeID, LocalizationContext.CurrentCulture.CultureCode);
                    // resultItem.Url = node.AbsoluteURL;
                    // result.SearchResult.Add(resultItem);
                }
            }

            return new SearchResultPage()
            {
                Pages = searchResultPages,
                Products = searchResultProducts
            };
        }
    }
}
