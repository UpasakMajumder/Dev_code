using Kadena.WebAPI.Contracts;
using AutoMapper;
using System.Data;
using Kadena.WebAPI.Models.Search;
using System.Collections.Generic;
using System.Linq;
using CMS.Helpers;
using System.Web;
using System.Text.RegularExpressions;

namespace Kadena.WebAPI.Services
{
    public class SearchService : ISearchService
    {
        private readonly IMapper mapper;
        private readonly IKenticoResourceService resources;
        private readonly IKenticoSearchService kenticoSearch;
        private readonly IKenticoProviderService kenticoProvider;

        public SearchService(IMapper mapper, IKenticoResourceService resources, IKenticoSearchService kenticoSearch, IKenticoProviderService kenticoProvider)
        {
            this.mapper = mapper;
            this.resources = resources;
            this.kenticoSearch = kenticoSearch;
            this.kenticoProvider = kenticoProvider;
        }

        public SearchResultPage Search(string phrase, int results = 100)
        {
            var searchResultPages = SearchPages(phrase, results);
            var searchResultProducts = SearchProducts(phrase, results);

            return new SearchResultPage()
            {
                Pages = searchResultPages,
                Products = searchResultProducts
            };
        }

        public AutocompleteResponse Autocomplete(string phrase, int results = 3)
        {
            var searchResultPages = SearchPages(phrase, results);
            var searchResultProducts = SearchProducts(phrase, results);

            var result = new AutocompleteResponse()
            {
                Pages = new AutocompletePages()
                {
                    Url = $"/serp?phrase={HttpUtility.UrlEncode(phrase)}&tab=pages",
                    Items = mapper.Map<List<AutocompletePage>>(searchResultPages)
                },
                Products = new AutocompleteProducts()
                {
                    Url = $"/serp?phrase={HttpUtility.UrlEncode(phrase)}&tab=products",
                    Items = searchResultProducts.Select(p => new AutocompleteProduct()
                        {
                            Id = p.Id,
                            Category = p.Category,
                            Image = p.ImgUrl,
                            Stock = p.Stock,
                            Title = p.Title,
                            Url = kenticoProvider.GetDocumentUrl(p.Id)
                        }
                    ).ToList()
                },
                Message = string.Empty
            };

            result.UpdateNotFoundMessage(resources.GetResourceString("No results found")); //TODO res string
            return result;
        }

        public List<ResultItemPage> SearchPages(string phrase, int results)
        {
            var searchResultPages = new List<ResultItemPage>();
            var datarowsResults = kenticoSearch.Search(phrase, "KDA_PagesIndex", "/%", results, true);
            
            foreach (DataRow dr in datarowsResults)
            {
                int documentId = GetDocumentId(dr[0]);                

                var resultItem = new ResultItemPage()
                {
                    Id = documentId,
                    Text = Regex.Replace(dr[5].ToString(), @"<[^>]+>|&nbsp;", "").Trim(),
                    Title = dr[4].ToString(),
                    Url = kenticoProvider.GetDocumentUrl(documentId)
                };

                searchResultPages.Add(resultItem);
            }

            return searchResultPages;
        }

        public List<ResultItemProduct> SearchProducts(string phrase, int results)
        {
            var searchResultProducts = new List<ResultItemProduct>();
            var datarowsResults = kenticoSearch.Search(phrase, "KDA_ProductsIndex", "/Products/%", results,  true);

            foreach (DataRow dr in datarowsResults)
            {
                int documentId = GetDocumentId(dr[0]);
                var resultItem = new ResultItemProduct()
                {
                    Id = documentId,
                    Title = dr[4].ToString(),
                    Breadcrumbs = kenticoProvider.GetBreadcrumbs(documentId),
                    IsFavourite = false,
                };

                var product = kenticoProvider.GetProductByDocumentId(documentId);
                if (product != null)
                {
                    resultItem.ImgUrl = product.SkuImageUrl;
                    resultItem.Category = product.Category;
                    resultItem.Stock = new Stock()
                    {
                        Text = $"{product.StockItems} pcs in stock",
                        Type = product.Availability
                    };
                    resultItem.UseTemplateBtn = new UseTemplateBtn()
                    {
                        Text = "Go to detail", // TODO configurable
                        Url = product.DocumentUrl
                    };
                }


                searchResultProducts.Add(resultItem);
            }

            return searchResultProducts;
        }

        private int GetDocumentId(object o)
        {
            int documentId = 0;
            var parsedId = o.ToString().Split(new char[]{';'})?[0].Split(new char[] {'_'})[0];
            int.TryParse(parsedId, out documentId);
            return documentId;
        }
    }
}
